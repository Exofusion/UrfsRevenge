using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class LevelManager : MonoBehaviour {
	public GUIText debugText;

	public GameObject currSegment;
	public GameObject prevSegment;
	public GameObject finalSegment;
	public GameObject enemyChampion;
	private Scrollbar progressBar;
	private GameObject segmentSource;

	public SpriteRenderer playerAvatar;
	public PlayerScript player;
	public TextMesh levelText;
	public GameObject deathScreen;
	public GameObject pauseScreen;
	public Text maxLevel;
	public Text waffleText;

	public float segmentSize = 30;

	private JSONNode matchData;
	private JSONNode frameData;

	public static int numWaffles;
	public static bool urfKilled;

	public float meleeMinionRate = 0.2f;
	public float casterMinionRate = 0.15f;
	public float cannonMinionRate = 0.05f;

	private int currentLevel;

	public static int currentPlayerDamage;
	public static int currentMinionStat;
	private static float playerDmgGrowth;
	private static float minionGrowth;
	private static float playerHealthGrowth;

	public static float currentCritChance;
	private static float critChanceGrowth;

	private static float[] enemyHealthGrowth;
	private static float[] enemyDamageGrowth;

	public static int meleeMinionKillCount;
	public static int casterMinionKillCount;
	public static int cannonMinionKillCount;
	public static int enemyChampionKillCount;
	public static int shotsFired;
	public static int shotsHit;

	private GameObject meleeMinion;
	private GameObject casterMinion;
	private GameObject cannonMinion;

	public int maxSections = 0;
	private int currentSection = 0;
	public static bool finalBoss;

	public int championIndex;

	void Awake() {
		Time.timeScale = 1;
		meleeMinionKillCount = 0;
		casterMinionKillCount = 0;
		cannonMinionKillCount = 0;
		enemyChampionKillCount = 0;

		numWaffles = 0;
		finalBoss = false;
		urfKilled = false;
		shotsFired = 0;
		shotsHit = 0;
		currentPlayerDamage = 0;
		currentMinionStat = 0;

		progressBar = GameObject.Find ("ProgressBar").GetComponent<Scrollbar> ();
		segmentSource = Instantiate (currSegment, currSegment.transform.position, currSegment.transform.rotation) as GameObject;
		segmentSource.name = "SegmentSource";
		meleeMinion = GameObject.Find ("SegmentSource/MeleeMinion");
		casterMinion = GameObject.Find ("SegmentSource/CasterMinion");
		//cannonMinion = GameObject.Find ("SegmentSource/CannonMinion");

		// Cannon will be disabled in the beginning, so we can't use Find()
		foreach(Transform t in segmentSource.transform)
		{
			if (t.name == "CannonMinion")
			{
				cannonMinion = t.gameObject;
				cannonMinion.SetActive (true);
				break;
			}
		}
		segmentSource.SetActive (false);

		if (GameManager.instance != null) {
			matchData = GameManager.instance.GetMatchData ();
			frameData = matchData["timeline"]["frames"];
			maxSections = GetNumFrames (matchData);
			championIndex = GameManager.instance.selectedChampIndex;
			
			maxLevel.text = "Level "+((maxSections/2)+1+(maxSections%2));
			playerDmgGrowth = 2+(GameManager.instance.GetTotalDamageDealtToChamps(championIndex)/150f)/maxSections;
			currentPlayerDamage = (int)playerDmgGrowth+1;
			minionGrowth = 5+(GameManager.instance.GetMinionsKilled(championIndex)*1.5f)/maxSections;
			currentMinionStat = (int)minionGrowth;
			playerHealthGrowth = (GameManager.instance.GetTotalDamageTaken (championIndex)/100f)/maxSections;
			critChanceGrowth = ((GameManager.instance.GetLargestCriticalStrike(championIndex)/1000f)/maxSections)+0.01f;
			currentCritChance = critChanceGrowth;
			
			enemyHealthGrowth = new float[10];
			enemyDamageGrowth = new float[10];
			for(int i=0; i<GameManager.instance.champIds.Length; i++)
			{
				enemyHealthGrowth[i] = (GameManager.instance.GetTotalDamageTaken (i)/50)/maxSections;
				enemyDamageGrowth[i] = (GameManager.instance.GetTotalDamageDealtToChamps(i)/100)/maxSections;
			}
			
			Texture2D avatarTexture = GameManager.instance.champImages[championIndex];
			playerAvatar.sprite = Sprite.Create (avatarTexture,
			                                     new Rect (0, 0, avatarTexture.width, avatarTexture.height),
			                                     new Vector2 (0.5f, 0.5f));
			
			if (StaticData.ChampionAvatarFlipLookup.Contains(GameManager.instance.champIds[championIndex]))
			{
				Vector3 theScale = playerAvatar.transform.localScale;
				theScale.x *= -1;
				playerAvatar.transform.localScale = theScale;
			}
		}
	}

	// Use this for initialization
	void Start () {
	}

	public void RestartLevel()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public void ExitToMainMenu()
	{
		//GameManager.instance.Reset();
		Application.LoadLevel ("MainMenu");
	}

	void ShowPauseScreen()
	{
		Time.timeScale = 0;
		pauseScreen.SetActive (true);
		
		GameObject obj = GameObject.Find (pauseScreen.name+"/MatchIDText");
		obj.GetComponent<Text>().text = GameManager.instance.matchID.ToString ();
	}

	public void ClosePauseScreen()
	{
		Time.timeScale = 1;
		pauseScreen.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!deathScreen.activeSelf && Input.GetKeyDown (KeyCode.Escape)) {
			if (pauseScreen.activeSelf)
				ClosePauseScreen ();
			else
				ShowPauseScreen ();
		}

		if (player.health <= 0) {
			Time.timeScale = 0;
			deathScreen.SetActive (true);

			int meleeMinionScore,
				casterMinionScore,
				cannonMinionScore,
				enemyChampScore,
				waffleScore,
				finalScore;
			float accuracy = ((float)LevelManager.shotsHit/LevelManager.shotsFired);

			meleeMinionScore = LevelManager.meleeMinionKillCount * 5;
			casterMinionScore = LevelManager.casterMinionKillCount * 10;
			cannonMinionScore = LevelManager.cannonMinionKillCount * 25;
			enemyChampScore = LevelManager.enemyChampionKillCount * 50;
			waffleScore = LevelManager.numWaffles * 50;

			finalScore = (int)(((meleeMinionScore+
			                     casterMinionScore+
			                     cannonMinionScore+
			                     enemyChampScore)*accuracy)+
			                   waffleScore);

			GameObject obj = GameObject.Find (deathScreen.name+"/MeleeMinions/Count");
			obj.GetComponent<Text>().text = LevelManager.meleeMinionKillCount.ToString ();

			obj = GameObject.Find (deathScreen.name+"/CasterMinions/Count");
			obj.GetComponent<Text>().text = LevelManager.casterMinionKillCount.ToString ();

			obj = GameObject.Find (deathScreen.name+"/CannonMinions/Count");
			obj.GetComponent<Text>().text = LevelManager.cannonMinionKillCount.ToString ();

			obj = GameObject.Find (deathScreen.name+"/EnemyChampions/Count");
			obj.GetComponent<Text>().text = LevelManager.enemyChampionKillCount.ToString ();
			
			obj = GameObject.Find (deathScreen.name+"/EnemyScore/Count");
			obj.GetComponent<Text>().text = (meleeMinionScore+
			                                 casterMinionScore+
			                                 cannonMinionScore+
			                                 enemyChampScore).ToString ();
			
			obj = GameObject.Find (deathScreen.name+"/Accuracy/Count");
			obj.GetComponent<Text>().text = accuracy.ToString ("#0.##%");
			
			obj = GameObject.Find (deathScreen.name+"/WaffleBonus/Count");
			obj.GetComponent<Text>().text = LevelManager.numWaffles.ToString ();
			
			obj = GameObject.Find (deathScreen.name+"/FinalScore/Count");
			obj.GetComponent<Text>().text = finalScore.ToString ();

			obj = GameObject.Find (deathScreen.name+"/MatchIDText");
			obj.GetComponent<Text>().text = GameManager.instance.matchID.ToString ();
			
			Image champAvatar = GameObject.Find (deathScreen.name+"/ChampImage").GetComponent<Image>();
			Texture2D champAvatarTexture = GameManager.instance.champImages[GameManager.instance.selectedChampIndex];
			champAvatar.sprite = Sprite.Create (champAvatarTexture,
			                                    new Rect (0, 0, champAvatarTexture.width, champAvatarTexture.height),
			                                    new Vector2 (0.5f, 0.5f));
			
			if (urfKilled)
			{
				obj = GameObject.Find (deathScreen.name+"/ResultText");
				obj.GetComponent<Text>().text = "Victory!";
			}
		}
	}

	int GetNumFrames( JSONNode root )
	{
		return root["timeline"] ["frames"].AsArray.Count;
	}
	
	int GetFirstChampion( JSONNode root )
	{
		return root["participants"][0]["championId"].AsInt;
	}
	
	string GetMatchType( JSONNode root )
	{
		return root["matchType"];
	}

	public void LoadNextSegment(){
		/*
		currentSection += maxSections;
		player.health += (int)(playerHealthGrowth * currentSection) * maxSections;
		player.maxHealth += (int)(playerHealthGrowth * currentSection) * maxSections;
		*/

		currentSection ++;
		Vector3 newPosition = currSegment.transform.position;
		newPosition.x += segmentSize;
		Destroy (prevSegment);
		
		prevSegment = currSegment;
		if (currentSection >= maxSections)
		{
			currSegment = finalSegment;
			currSegment.transform.position = newPosition;
			finalBoss = true;
		}
		else
		{
			currSegment = Instantiate (segmentSource, newPosition, segmentSource.transform.rotation) as GameObject;
		}
		currSegment.SetActive(true);

		// Only spawn the default cannon minion every other wave
		foreach(Transform t in currSegment.transform)
		{
			if (t.name == "CannonMinion")
			{
				if (currentSection % 2 == 1)
					t.gameObject.SetActive (true);
				else
					t.gameObject.SetActive (false);

				break;
			}
		}

		int numMeleeMinions = (int)(meleeMinionRate * currentSection);
		int numCasterMinions = (int)(casterMinionRate * currentSection);
		int numCannonMinions = (int)(cannonMinionRate * currentSection);

		for (int i=0; i<numMeleeMinions; i++)
		{
			Vector3 minionPos = meleeMinion.transform.position;
			minionPos.x = currSegment.transform.position.x + (minionPos.x-segmentSource.transform.position.x);
			GameObject newMinion = Instantiate (meleeMinion, minionPos, meleeMinion.transform.rotation) as GameObject;
			newMinion.transform.parent = currSegment.transform;
			newMinion.SetActive (true);
		}
		
		for (int i=0; i<numCasterMinions; i++)
		{
			Vector3 minionPos = casterMinion.transform.position;
			minionPos.x = currSegment.transform.position.x + (minionPos.x-segmentSource.transform.position.x);
			GameObject newMinion = Instantiate (casterMinion, minionPos, casterMinion.transform.rotation) as GameObject;
			newMinion.transform.parent = currSegment.transform;
			newMinion.SetActive (true);
		}

		for (int i=0; i<numCannonMinions; i++)
		{
			Vector3 minionPos = cannonMinion.transform.position;
			minionPos.x = currSegment.transform.position.x + (minionPos.x-segmentSource.transform.position.x);
			GameObject newMinion = Instantiate (cannonMinion, minionPos, cannonMinion.transform.rotation) as GameObject;
			newMinion.transform.parent = currSegment.transform;
			newMinion.SetActive (true);
		}

		ParseFrameData(currentSection);
		currentSection++;
		ParseFrameData(currentSection);
		currentLevel = ((int)(currentSection/2) + 1);
		levelText.text = currentLevel.ToString ();
		progressBar.value = (float)(currentSection)/maxSections;
		currentPlayerDamage = (int)(playerDmgGrowth*currentLevel);
		currentMinionStat = (int)(minionGrowth*currentLevel);
		currentCritChance = critChanceGrowth*currentLevel;

		int addedHealth = (int)(playerHealthGrowth*currentLevel);
		player.maxHealth += addedHealth;
		player.health += addedHealth;
	}

	void ParseFrameData(int section)
	{
		// Add champion objects
		int thisParticipantId = GameManager.instance.selectedChampIndex + 1;
		JSONNode frameNodeEvents = frameData[section]["events"];

		foreach (JSONNode e in frameNodeEvents.AsArray) {
			if (e["eventType"].ToString () == "\"CHAMPION_KILL\""){
				if (e["killerId"].AsInt == thisParticipantId){
					AddEnemyChampion (e, section);
				}
				else{
					foreach(JSONNode p in e["assistingParticipantIds"].AsArray){
						if (p.AsInt == thisParticipantId){
							AddEnemyChampion (e, section);
						}
					}
				}
			}
		}
	}

	void AddEnemyChampion(JSONNode killEvent, int section)
	{
		// Find when this kill occurred this section
		int firstOrSecond = section % 2;
		int victimId = killEvent["victimId"].AsInt-1;
		float frameInterval = GameManager.instance.frameInterval;
		float beginTime = (currentSection-1)*frameInterval;
		float localTime = killEvent ["timestamp"].AsInt - beginTime;
		float localPercent = localTime / frameInterval;
		float localSegmentPosition = ((segmentSize/2) * localPercent) + currSegment.transform.position.x;

		if (firstOrSecond == 0)
			localSegmentPosition += (segmentSize / 2);

		Vector3 newPosition = new Vector3(localSegmentPosition,
		                                  meleeMinion.transform.position.y,
		                                  meleeMinion.transform.position.z);
		GameObject newEnemyChampion = Instantiate (enemyChampion, newPosition, enemyChampion.transform.rotation) as GameObject;
		newEnemyChampion.transform.parent = currSegment.transform;
		Texture2D victimTexture = GameManager.instance.champImages[victimId];
		EnemyChampionAI enemyChampionAI = newEnemyChampion.GetComponentInChildren<EnemyChampionAI> ();
		enemyChampionAI.SetChampionAvatar (victimTexture, StaticData.ChampionAvatarFlipLookup.Contains(GameManager.instance.champIds [victimId]));
		enemyChampionAI.SetChampionBullet (StaticData.ChampionIdDictionary [GameManager.instance.champIds [victimId]]);
		enemyChampionAI.baseHealth = (int)(enemyHealthGrowth [victimId] * section) + 100;
		enemyChampionAI.currentHealth = enemyChampionAI.baseHealth;
		enemyChampionAI.castDamage = (int)((enemyDamageGrowth [victimId] * section)/2) + 25;
	}
}
