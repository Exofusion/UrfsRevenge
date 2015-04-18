using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]

public class UrfBossAI : MonoBehaviour {
	public Transform target;
	public float moveSpeed;
	public ForceMode2D forceMode;
	public GameObject bullet;
	public GameObject waffle;
	public GameObject victoryText;

	private bool flipped = false;
	private float turnModifier = 10f;
	private float lastFlipped = 0;
	private int sideOfPlayer;
	
	public Transform healthBar;
	public Transform healthLeft;
	public TextMesh healthText;

	public int baseHealth;
	public float maxCastRange = 4.5f;
	public Vector2 castAngle;
	private Vector2 currentCastAngle;
	private float castRange;
	public float currentHealth;
	private float lastHealth;

	public int castDamage;
	public int meleeDamage;
	private float lastDamageTime = 0;
	private float damageDelay = 1f;

	void Awake() {
	}

	// Use this for initialization
	void Start () {
		castRange = Random.Range (maxCastRange/2f, maxCastRange);
		lastDamageTime = Time.time + Random.Range (0f, damageDelay);
		target = GameObject.Find ("Player").transform;
		baseHealth = 4000 + (LevelManager.currentMinionStat * 50);
		meleeDamage = (int)(LevelManager.currentMinionStat / 2f);
		castDamage = LevelManager.currentMinionStat;
		currentHealth = baseHealth;
		sideOfPlayer = 1;
		lastFlipped = Time.time;
		lastHealth = 0;

		currentCastAngle = castAngle;
		
		currentCastAngle.x += Random.Range (-3f, 3f);
		currentCastAngle.y += Random.Range (-3f, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0) {
			for (int i=0; i<30; i++)
			{
				Instantiate (waffle, new Vector3 (transform.position.x,
				                                  transform.position.y + GetComponentInChildren<SpriteRenderer> ().sprite.bounds.extents.y,
				                                  transform.position.z), Quaternion.identity);
			}
			LevelManager.enemyChampionKillCount++;
			LevelManager.urfKilled = true;
			victoryText.SetActive( true );
			Destroy (gameObject);
		}

		if (lastHealth != currentHealth) {
			Vector3 theScale = healthLeft.localScale;
			theScale.x = currentHealth / baseHealth;
			healthLeft.localScale = theScale;
			healthText.text = currentHealth.ToString () + " / " + baseHealth.ToString ();
			
			lastHealth = currentHealth;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "PlayerBullet") {
			BulletScript bullet = coll.gameObject.GetComponent<BulletScript> ();
			currentHealth -= bullet.damage;
			Vector3 damagePosition = new Vector3 (transform.position.x,
			                                      transform.position.y - GetComponentInChildren<SpriteRenderer> ().sprite.bounds.extents.y,
			                                      transform.position.z);
			bullet.DisplayDamage( damagePosition, true );
		} else if (coll.gameObject.tag == "Player") {
			PlayerScript player = coll.gameObject.GetComponent<PlayerScript>();
			player.TakeDamage (meleeDamage);
		}
	}

	void FixedUpdate() {
		Vector3 castOffset = new Vector3 (castRange, 0);
		Vector3 playerPositionDiff = (target.position + (sideOfPlayer*castOffset)) - transform.position;

		transform.position += Vector3.Lerp ( Vector3.zero,
		                                     new Vector3(playerPositionDiff.x, 0, 0),
		                                  	 moveSpeed * Time.deltaTime);

		if ((Time.time - lastFlipped) >= turnModifier ) {
			FlipMinion();
		}

		if ((Time.time - lastDamageTime) > damageDelay) {
			FireBullet ();
			lastDamageTime = Time.time;
		}
	}

	void FireBullet()
	{
		GameObject newBullet = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
		BulletScript newBulletScript = newBullet.GetComponent<BulletScript> ();
		newBulletScript.damage = castDamage;
		int direction = 1;

		if (!flipped)
		{
			Vector3 theScale = newBullet.transform.localScale;
			theScale.x *= -1;
			newBullet.transform.localScale = theScale;
			direction = -1;
		}
		
		newBullet.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2(direction*currentCastAngle.x,currentCastAngle.y), ForceMode2D.Impulse);

		//Test different sizes
		newBullet.transform.localScale *= 1.5f;
	}
	
	void FlipMinion()
	{
		transform.localScale = new Vector3 (-transform.localScale.x,
	                                    transform.localScale.y,
	                                    transform.localScale.z);
		flipped = !flipped;
		lastFlipped = Time.time;
		sideOfPlayer = -sideOfPlayer;
		
		Vector3 healthScale = healthBar.localScale;
		healthScale.x *= -1;
		healthBar.localScale = healthScale;

		currentCastAngle = castAngle;
		currentCastAngle.x += Random.Range (-3f, 3f);
		currentCastAngle.y += Random.Range (-3f, 3f);
	}
}
