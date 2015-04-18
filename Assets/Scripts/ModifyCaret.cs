using UnityEngine;
using System.Collections;

public class ModifyCaret : MonoBehaviour {
	private bool modifyCaret = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (modifyCaret) {
			GameObject caret = GameObject.Find (gameObject.name+" Input Caret");
			if (caret != null) {
				Vector3 localPos = caret.GetComponent<RectTransform>().localPosition;
				localPos.y = -6;
				caret.GetComponent<RectTransform>().localPosition = localPos;
				modifyCaret = false;
				Debug.Log ("Caret modified");
			}
		}
	}
}
