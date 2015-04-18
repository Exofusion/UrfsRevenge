using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]

public class SimpleMeleeMinionAI : MonoBehaviour {
	public Transform target;
	public float maxForceAmount = 1f;
	public ForceMode2D forceMode;
	public GameObject waffle;

	private Rigidbody2D rb;
	private bool flipped = false;
	private float speedModifier;
	private float turnModifier;

	private float lastFlipped = 0;
	private int currentHealth;

	public int damage;
	private float lastDamageTime = 0;
	private float damageDelay = 0.5f;

	void Awake() {
	}

	// Use this for initialization
	void Start () {
		speedModifier = Random.Range (maxForceAmount/2f, maxForceAmount);
		turnModifier = Random.Range (2f, 8f);
		rb = GetComponent<Rigidbody2D> ();
		currentHealth = (int)(LevelManager.currentMinionStat * 4) + 50;
		damage = (int)(LevelManager.currentMinionStat / 2f) + 10;
		lastDamageTime = Time.time + Random.Range (0f, damageDelay);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0) {
			if (Random.value < 0.3f)
			{
				Instantiate (waffle, new Vector3 (transform.position.x,
				                                  transform.position.y + GetComponent<SpriteRenderer> ().sprite.bounds.extents.y,
				                                  transform.position.z), Quaternion.identity);
			}
			LevelManager.meleeMinionKillCount++;
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "PlayerBullet") {
			BulletScript bullet = coll.gameObject.GetComponent<BulletScript> ();
			currentHealth -= bullet.damage;
			Vector3 damagePosition = new Vector3 (transform.position.x,
			                                     transform.position.y + GetComponent<SpriteRenderer> ().sprite.bounds.extents.y,
			                                     transform.position.z);

			bullet.DisplayDamage( damagePosition );
		} else if (coll.gameObject.tag == "Player" && (Time.time-lastDamageTime > damageDelay)) {
			PlayerScript player = coll.gameObject.GetComponent<PlayerScript>();
			player.TakeDamage (damage);
			lastDamageTime = Time.time;
		}
	}

	void FixedUpdate() {
		lastFlipped += Time.fixedDeltaTime;

		Vector3 playerPositionDiff = target.position - transform.position;
		int moveDirection = 0;

		if (flipped)
			moveDirection = 1;
		if (!flipped)
			moveDirection = -1;
		
		rb.AddForce (new Vector2 (moveDirection, 0) * speedModifier,
		             forceMode);

		// Flip the minion if they are facing away from the player past a certain distance
		// Also flip if they are just stuck on the player, with a random chance to turn
		if (!flipped && playerPositionDiff.x >= turnModifier ||
		    flipped && playerPositionDiff.x < -turnModifier ||
		    Mathf.Abs (playerPositionDiff.x) < 1.1f && (Random.Range(0,50) == 0)) {
			FlipMinion();
		}
	}

	void FlipMinion()
	{
		if (lastFlipped > 2f) {
			transform.localScale = new Vector3 (-transform.localScale.x,
		                                    transform.localScale.y,
		                                    transform.localScale.z);
			flipped = !flipped;
			lastFlipped = 0;
		}
	}
}
