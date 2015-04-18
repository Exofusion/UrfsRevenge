using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryboardScript : MonoBehaviour {
	private int currSlide = 0;
	private int maxSlide = 6;
	private Vector3 targetPosition;
	public Text playButton;
	public SpriteRenderer firstSlide;

	// Use this for initialization
	void Start () {
		targetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (targetPosition != transform.position) {
			transform.position += (targetPosition-transform.position) * Time.deltaTime * 3;

		}
	}

	public void NextSlide()
	{
		if (currSlide < maxSlide) {
			targetPosition += new Vector3 (firstSlide.bounds.extents.x*2, 0);
			currSlide++;

			if (currSlide == maxSlide)
				playButton.text = "PLAY";
		}
	}
	
	public void PrevSlide()
	{
		if (currSlide > 0) {
			targetPosition -= new Vector3 (firstSlide.bounds.extents.x*2, 0);
			currSlide--;
		}
	}

	public void Play()
	{
		Application.LoadLevel ("MainMenu");
	}
}
