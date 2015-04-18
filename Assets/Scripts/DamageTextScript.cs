using UnityEngine;
using System.Collections;

[RequireComponent (typeof(TextMesh))]

public class DamageTextScript : MonoBehaviour {
	public float scrollSpeed = 0.08f;
	public float alpha = 1.5f;
	public float duration = 1.50f;
	public bool inverted = false;

	private TextMesh thisMeshText;

	// Use this for initialization
	void Awake () {
		thisMeshText = gameObject.GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (alpha > 0) {
			float newPositionY;

			if (inverted)
				newPositionY = transform.position.y - scrollSpeed * Time.deltaTime;
			else
				newPositionY = transform.position.y + scrollSpeed * Time.deltaTime;

			transform.position = new Vector3( transform.position.x, newPositionY, transform.position.z);
			alpha -= Time.deltaTime / duration;
			thisMeshText.color = new Color(thisMeshText.color.r,
			                               thisMeshText.color.g,
			                               thisMeshText.color.b,
			                               alpha);
		} else {
			Destroy (gameObject);
		}
	}
}
