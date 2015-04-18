using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class MenuControllerScript : MonoBehaviour {
	public Button fetchButton;
	public InputField matchInput;
	public InputField summonerInput;
	public GameObject matchHistory;
	public GameObject controls;

	public Image champ0button;
	public Image champ1button;
	public Image champ2button;
	public Image champ3button;
	public Image champ4button;
	public Image champ5button;
	public Image champ6button;
	public Image champ7button;
	public Image champ8button;
	public Image champ9button;

	private Image[] champButtons;

	private bool fetchingMatch = false;
	private bool fetchingHistory = false;

	// Use this for initialization
	void Start () {
		champButtons = new Image[10];
		champButtons [0] = champ0button;
		champButtons [1] = champ1button;
		champButtons [2] = champ2button;
		champButtons [3] = champ3button;
		champButtons [4] = champ4button;
		champButtons [5] = champ5button;
		champButtons [6] = champ6button;
		champButtons [7] = champ7button;
		champButtons [8] = champ8button;
		champButtons [9] = champ9button;
	}
	
	// Update is called once per frame
	void Update () {
		if (fetchingMatch && (GameManager.instance.GetMatchData () != null)) {
			for (int i=0; i < champButtons.Length; i++) {
				if (champButtons [i].sprite == null && GameManager.instance.champImages != null) {
					Texture2D champTexture = GameManager.instance.champImages [i];
					if (champTexture != null) {
						champButtons [i].sprite = Sprite.Create (champTexture,
					                                        	 new Rect (0, 0, champTexture.width, champTexture.height),
					                                        	 new Vector2 (0.5f, 0.5f));
						champButtons [i].GetComponent<Button> ().interactable = true;
					}
				}
			}

			matchInput.text = GameManager.instance.matchID.ToString ();
			fetchingMatch = false;
		}

		if (fetchingHistory && (GameManager.instance.GetHistoryData () != null)) {
			JSONNode gameData = GameManager.instance.GetHistoryData ();
			matchHistory.SetActive (true);

			int matchNum = 0;
			foreach(JSONNode match in gameData["games"].AsArray)
			{
				GameObject gameLengthText = GameObject.Find (matchHistory.name+"/Panel"+matchNum+"/GameLength");
				int timePlayed = match["stats"]["timePlayed"].AsInt;
				gameLengthText.GetComponent<Text>().text = Mathf.Floor (timePlayed/60)+":"+(timePlayed%60).ToString ("D2");
				GameObject gameTypeText = GameObject.Find (matchHistory.name+"/Panel"+matchNum+"/GameType");
				gameTypeText.GetComponent<Text>().text = match["gameMode"];
				int blueIndex = 0;
				int purpleIndex = 0;
				GameObject iconObject;

				// Add the summoner's champion first
				if (match["teamId"].AsInt == 100)
				{
					iconObject = GameObject.Find (matchHistory.name+"/Panel"+matchNum+"/Blue/Champ0");
					blueIndex++;
				}
				else
				{
					iconObject = GameObject.Find (matchHistory.name+"/Panel"+matchNum+"/Purple/Champ0");
					purpleIndex++;
				}
				AddChampToPanel (iconObject, match["championId"].AsInt);

				foreach(JSONNode player in match["fellowPlayers"].AsArray)
				{
					if (player["teamId"].AsInt == 100) {
						iconObject = GameObject.Find (matchHistory.name+"/Panel"+matchNum+"/Blue/Champ"+blueIndex);
						blueIndex++;
					}
					else{
						iconObject = GameObject.Find (matchHistory.name+"/Panel"+matchNum+"/Purple/Champ"+purpleIndex);
						purpleIndex++;
					}
					AddChampToPanel (iconObject, player["championId"].AsInt);
				}
				matchNum++;
			}
			fetchingHistory = false;
		}
	}

	void AddChampToPanel(GameObject iconObject, int championId)
	{
		Image iconSprite = iconObject.GetComponent<Image>();
		Texture2D iconTexture = GameManager.instance.GetChampIconTextureById (championId);
		iconSprite.sprite = Sprite.Create (iconTexture,
		                                   new Rect (0, 0, iconTexture.width, iconTexture.height),
		                                   new Vector2 (0.5f, 0.5f));
	}

	void PrepareForFetchingMatch()
	{
		fetchingMatch = true;
		
		GameManager.instance.champImages = new Texture2D[10]; // magic number
		for(int i=0; i < champButtons.Length; i++) {
			champButtons[i].GetComponent<Button>().interactable = false;
			champButtons[i].sprite = new Sprite();
		}
		
		GameManager.instance.Reset ();
	}

	public void FetchMatch(bool specificMatch = false){
		PrepareForFetchingMatch ();
		if (specificMatch) {
			StartCoroutine(APIManager.instance.FetchMatch (GameManager.instance.server, matchInput.text));
		} else {
			StartCoroutine(APIManager.instance.FetchMatch (GameManager.instance.server));
		}
	}

	public void FetchMatchFromHistory(int historyIndex)
	{
		CloseHistory ();
		PrepareForFetchingMatch ();
		JSONNode data = GameManager.instance.GetHistoryData ();
		if (data != null) {
			StartCoroutine(APIManager.instance.FetchMatch (GameManager.instance.server, data["games"][historyIndex]["gameId"]));
		}
	}

	public void PlayGame(int champIndex){
		GameManager.instance.selectedChampIndex = champIndex;
		Application.LoadLevel ("LevelScene");
	}

	public void LoadSummoner()
	{
		fetchingHistory = true;
		GameManager.instance.SetHistoryData (null);
		StartCoroutine (APIManager.instance.FetchHistory (GameManager.instance.server, summonerInput.text));
	}

	public void CloseHistory()
	{
		matchHistory.SetActive (false);
	}

	public void OpenControls()
	{
		controls.SetActive (true);
	}

	public void CloseControls()
	{
		controls.SetActive (false);
	}
}
