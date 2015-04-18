using UnityEngine;
using System.Collections;

public class Tiling : MonoBehaviour {
	public int offsetX = 3;
	public Transform tile;

	private Transform rightClone = null;
	private Transform leftClone = null;

	public float spriteWidth;
	private float overlap = 0.15f;
	private Camera cam;

	void Awake() {
		cam = Camera.main;
	}

	// Use this for initialization
	void Start () {
		SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer> ();
		spriteWidth = spriteRenderer.sprite.bounds.size.x*Mathf.Abs(tile.transform.localScale.x);
	}
	
	// Update is called once per frame
	void Update () {
		float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;
		float edgeVisibleRight = (tile.transform.position.x + spriteWidth/2f) - camHorizontalExtend;
		float edgeVisibleLeft = (tile.transform.position.x - spriteWidth/2f) + camHorizontalExtend;

		if (cam.transform.position.x >= edgeVisibleRight - offsetX)
		{
			if (rightClone == null) {
				rightClone = MakeNewClone (1);
			}
			else {
				float edgeVisibleRightClone = (rightClone.transform.position.x + spriteWidth/2) - camHorizontalExtend;

				if (cam.transform.position.x >= edgeVisibleRightClone - offsetX)
				{
					if (leftClone != null)
					{
						leftClone.transform.position = new Vector3(tile.transform.position.x,
						                                           tile.transform.position.y,
						                                           tile.transform.position.z);
					}

					tile.transform.position = new Vector3(rightClone.transform.position.x,
					                                      rightClone.transform.position.y,
					                                      rightClone.transform.position.z);

					rightClone.transform.position = new Vector3(rightClone.transform.position.x+spriteWidth,
					                                            rightClone.transform.position.y,
					                                            rightClone.transform.position.z);
				}
			}
		}
		else if (cam.transform.position.x <= edgeVisibleLeft + offsetX)
		{
			if (leftClone == null) {
				leftClone = MakeNewClone (-1);
			}
			else {
				float edgeVisibleLeftClone = (leftClone.transform.position.x - spriteWidth/2) + camHorizontalExtend;
				
				if (cam.transform.position.x <= edgeVisibleLeftClone + offsetX)
				{
					if (rightClone != null)
					{
						rightClone.transform.position = new Vector3(tile.transform.position.x,
						                                            tile.transform.position.y,
						                                            tile.transform.position.z);
					}

					tile.transform.position = new Vector3(leftClone.transform.position.x,
					                                      leftClone.transform.position.y,
					                                      leftClone.transform.position.z);
					
					leftClone.transform.position = new Vector3(leftClone.transform.position.x-spriteWidth,
					                                           leftClone.transform.position.y,
					                                           leftClone.transform.position.z);
				}
			}
		}
	}

	Transform MakeNewClone(int rightOrLeft)
	{
		Vector3 clonePosition = new Vector3 (tile.transform.position.x + (spriteWidth - overlap) * rightOrLeft,
		                                     tile.transform.position.y,
		                                     tile.transform.position.z);
		Transform clone = Instantiate (tile.transform, clonePosition, tile.transform.rotation) as Transform;

		clone.parent = tile.transform.parent;
		return clone;
	}
}
