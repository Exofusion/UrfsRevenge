using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	public GameObject gameManager;
	public GameObject apiManager;

	void Start () {
		if (GameManager.instance == null)
			Instantiate (gameManager);

		if (APIManager.instance == null)
			Instantiate (apiManager);
	}
}
