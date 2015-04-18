using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]

public class SimpleCasterMinionAI : MonoBehaviour {
	public Transform target;
	public float maxForceAmount = 15f;
	public ForceMode2D forceMode;
	public GameObject bullet;
	public GameObject waffle;

	private Rigidbody2D rb;
	private bool flipped = false;
	private float speedModifier;
	private float turnModifier;

	private float lastFlipped = 0;
	public float maxCastRange = 5f;
	public Vector2 castAngle;
	private float castRange;
	private int currentHealth;

	public int damage = 6;
	private float lastDamageTime = 0;
	private float damageDelay = 1.5f;

	void Awake() {
	}

	// Use this for initialization
	void Start () {
		speedModifier = Random.Range (maxForceAmount/2f, maxForceAmount);
		castRange = Random.Range (maxCastRange/2f, maxCastRange);
		turnModifier = Random.Range (1f, 2f);
		rb = GetComponent<Rigidbody2D> ();
		currentHealth = (int)(LevelManager.currentMinionStat * 3) + 25;
		damage = (int)(LevelManager.currentMinionStat / 4) + 5;
		lastDamageTime = Time.time + Random.Range (0f, damageDelay);
		
		castAngle.x += Random.Range (-2f, 2f);
		castAngle.y += Random.Range (-2f, 2f);
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
			LevelManager.casterMinionKillCount++;
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
		}/* else if (coll.gameObject.tag == "Player" && (Time.time-lastDamageTime > damageDelay)) {
			PlatformerCharacter2D player = coll.gameObject.GetComponent<PlatformerCharacter2D>();
			player.TakeDamage (damage);
			lastDamageTime = Time.time;
		}*/
	}

	void FixedUpdate() {
		lastFlipped += Time.fixedDeltaTime;

		Vector3 playerPositionDiff = target.position - transform.position;
		int moveDirection = 0;

		if (flipped)
			moveDirection = 1;
		if (!flipped)
			moveDirection = -1;

		if (Mathf.Abs(playerPositionDiff.x) > castRange) {
			rb.AddForce (new Vector2 (moveDirection, 0) * speedModifier,
		             	              forceMode);
		}

		// Flip the minion if they are facing away from the player past a certain distance
		// Also flip if they are just stuck on the player, with a random chance to turn
		if (!flipped && playerPositionDiff.x >= turnModifier ||
		    flipped && playerPositionDiff.x < -turnModifier /*||
		    Mathf.Abs (playerPositionDiff.x) < 1f && (Random.Range(0,50) == 0)*/) {
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
		newBulletScript.damage = damage;
		int direction = 1;

		if (!flipped)
		{
			Vector3 theScale = newBullet.transform.localScale;
			theScale.x *= -1;
			newBullet.transform.localScale = theScale;
			direction = -1;
		}
		
		newBullet.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2(direction*castAngle.x,castAngle.y), ForceMode2D.Impulse);

		//Test different sizes
		//newBullet.transform.localScale *= 1.5f;
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
