using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using SimpleJSON;

public class APIManager : MonoBehaviour {
	public static APIManager instance = null;
	//private string hostname = "http://riotchallenge-env.elasticbeanstalk.com/match.php";
	private string hostname = "http://urfsrevenge.com";
	//private string hostname = "http://riotchallenge.appspot.com";

	void Start() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad (gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator FetchMatch(string server, string matchId = "random")
	{
		/*
		string url = "http://rainbowhash.com/riot/match.php?server=" + server +
					 "&match=" + matchId;
					 */

		string url;

		if (matchId == "random") {
			url = hostname+"/random_match";
		} else {
			url = hostname+"/match?server=" + server + "&match=" + matchId;
		}

		Debug.Log (url);

		WWW www = new WWW (url);
		
		while( !www.isDone )
		{
			yield return null;
		}

		// Check response
		string data = www.text;

		/*
		if(www.responseHeaders.Count > 0) {
			foreach(KeyValuePair<string, string> entry in www.responseHeaders) {
				Debug.Log(entry.Value + "=" + entry.Key);
			}
		}*/

		JSONNode jsonData = JSON.Parse (data);
		if (jsonData != null) {
			if (jsonData ["timeline"] != null) {
				Debug.Log ("Match and timeline data retrieved");
				GameManager.instance.matchID = jsonData ["matchId"].AsInt;
				GameManager.instance.frameInterval = jsonData ["timeline"] ["frameInterval"].AsInt;
				GameManager.instance.SetMatchData (jsonData);
			} else {
				Debug.Log ("No timeline data");
			}
		} else {
			Debug.Log ("Match not found.");
		}
	}

	public IEnumerator FetchHistory(string server, string summonerName)
	{
		string url = hostname+"/summoner_history?server=" + server + "&name=" + WWW.EscapeURL (summonerName);
		
		Debug.Log (url);
		
		WWW www = new WWW (url);
		
		while( !www.isDone )
		{
			yield return null;
		}
		
		// Check response
		string data = www.text;
		
		JSONNode jsonData = JSON.Parse (data);
		if (jsonData != null) {
			if (jsonData ["summonerId"] != null) {
				Debug.Log ("Summoner history retrieved");
				GameManager.instance.SetHistoryData (jsonData);
			} else {
				Debug.Log ("No summoner history data");
			}
		} else {
			Debug.Log ("Error parsing JSON");
		}
	}
}
