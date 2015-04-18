using UnityEngine;
using System.Collections;

public class WaffleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (0f, 5f), Random.Range (0f, 5f)), ForceMode2D.Impulse);
		Destroy (gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
