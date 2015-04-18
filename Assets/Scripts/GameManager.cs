using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	private JSONNode matchData;
	private JSONNode historyData;
	public int frameInterval;
	public int matchID;
	public int selectedChampIndex;
	public string server;

	public string[] champNames;
	public int[] champIds;
	public Texture2D[] champImages;
	public Texture2D champProjectile;

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		Reset ();
	}

	public JSONNode GetMatchData(){
		return matchData;
	}

	public void SetMatchData(JSONNode data){
		matchData = data;
		LoadTextures ();
	}
	
	public JSONNode GetHistoryData(){
		return historyData;
	}
	
	public void SetHistoryData(JSONNode data){
		historyData = data;
	}

	public void ClearMatchData(){
		matchData = null;
	}
	
	public bool GameReady(){
		return (matchData != null);
	}

	public int GetTotalDamageDealtToChamps(int champIndex)
	{
		return matchData["participants"][champIndex]["stats"]["totalDamageDealtToChampions"].AsInt;
	}

	public int GetMinionsKilled(int champIndex)
	{
		return matchData ["participants"] [champIndex] ["stats"] ["minionsKilled"].AsInt;
	}
	
	public int GetTotalDamageTaken(int champIndex)
	{
		return matchData ["participants"] [champIndex] ["stats"] ["totalDamageTaken"].AsInt;
	}
	
	public int GetLargestCriticalStrike(int champIndex)
	{
		return matchData ["participants"] [champIndex] ["stats"] ["largestCriticalStrike"].AsInt;
	}
	
	public void LoadTextures()
	{
		for (int i = 0; i < champImages.Length; i++)
		{
			champIds[i] = matchData ["participants"] [i] ["championId"].AsInt;
			if (champIds[i] != 0)
			{
				string champName = StaticData.ChampionIdDictionary[champIds[i]];
				if (champName != null){
					champNames[i] = champName;
					champImages[i] = GetChampIconTextureByFilename(champName);
					// ApplyChampIconTexture(i, champName);
				}
				else{
					Debug.LogError ("Could not resolve champion ID: "+champIds[i]);
				}
			}
		}
	}

	public Texture2D GetChampIconTextureByFilename(string filename)
	{
		Texture2D champTexture = Resources.Load ("ChampIcons/" + filename) as Texture2D;
		if (champTexture == null) {
			Debug.Log ("Failed to load texture for: "+filename);
		}
		return champTexture;
	}

	public Texture2D GetChampIconTextureById(int champId)
	{
		string champName = StaticData.ChampionIdDictionary[champId];
		return GetChampIconTextureByFilename (champName);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Reset(){
		matchData = null;
		selectedChampIndex = 0;
		champImages = new Texture2D[10]; // magic number
		champNames = new string[10];
		champIds = new int[10];
		champProjectile = null;
	}
}
