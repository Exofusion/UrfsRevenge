using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlatformScript : MonoBehaviour {
	private Collider2D parentCollider;
	private Collider2D thisCollider;
	private Collider2D[] playerColliders;

	void Awake() {
		parentCollider = transform.parent.GetComponent<EdgeCollider2D> ();
		playerColliders = GameObject.Find ("Player").GetComponents<Collider2D>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetAxis("Vertical") < -0.5f)
		{			
			foreach (Collider2D coll in playerColliders)
			{
				Physics2D.IgnoreCollision(coll, parentCollider, true);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player")
		{
			foreach (Collider2D coll in playerColliders)
			{
				Physics2D.IgnoreCollision(coll, parentCollider, true);
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "Player")
		{
			Physics2D.IgnoreCollision(collider, parentCollider, false);
		}
	}
}
