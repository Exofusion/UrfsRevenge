using UnityEngine;
using System.Collections;

public class PlayerShooterScript : MonoBehaviour {
	public GameObject bullet;
	public Transform character;

	public Vector2 thrust;
	public int baseDamage = 20;
	public int variance;

	private float fireDelay = 0.3f;
	private float lastFire;

	private GameManager gameManager;
	private string champName;

	private bool textureMissing = false;
	private bool textureCritMissing = false;
	private Texture2D particleTexture;
	private Texture2D particleCritTexture;

	// Use this for initialization
	void Start () {
		lastFire = 0;

		if (GameManager.instance != null) {
			gameManager = GameManager.instance;
			champName = gameManager.champNames[gameManager.selectedChampIndex];
		}

		particleTexture = Resources.Load ("ChampBasicProjectiles/"+champName) as Texture2D;
		particleCritTexture = Resources.Load ("ChampCritProjectiles/"+champName) as Texture2D;
		
		if (particleTexture == null)
		{
			Debug.LogError ("Could not locate champion projectile for: "+champName);
			textureMissing = true;
		}
		if (particleCritTexture == null)
		{
			Debug.LogError ("Could not locate champion critical strike projectile for: "+champName);
			textureCritMissing = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time-lastFire > fireDelay) && (Input.GetKey (KeyCode.F) || Input.GetKey (KeyCode.J))) {
			GameObject newBullet = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
			BulletScript newBulletScript = newBullet.GetComponent<BulletScript>();
			int direction = 1;

			if (character.localScale.x < 0)
			{
				Vector3 theScale = newBullet.transform.localScale;
				theScale.x *= -1;
				newBullet.transform.localScale = theScale;
				direction = -1;
			}

			newBullet.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2(direction*thrust.x,0f), ForceMode2D.Impulse);
			
			//Test different sizes
			newBullet.transform.localScale *= 1.5f;
			int newBulletDmg = Random.Range (LevelManager.currentPlayerDamage, LevelManager.currentPlayerDamage+variance);

			// Decide whether this projectile will critically strike
			if ( Random.Range (0f,1f) <= LevelManager.currentCritChance )
			{
				if (!textureCritMissing)
				{
					if (newBullet.GetComponent<SpriteRenderer>().sprite != null){
						newBullet.GetComponent<SpriteRenderer>().sprite = Sprite.Create (particleCritTexture,
						                                                                 new Rect(0, 0, particleTexture.width, particleTexture.height),
						                                                                 new Vector2(0.5f,0.5f));
					}
					else{
						Debug.LogError ("Cannot locate SpriteRenderer");
					}
				}
				newBulletScript.damage = (int)(newBulletDmg*1.5f);
				newBulletScript.crit = true;
			}
			else
			{
				if (!textureMissing)
				{
					if (newBullet.GetComponent<SpriteRenderer>().sprite != null){
						newBullet.GetComponent<SpriteRenderer>().sprite = Sprite.Create (particleTexture,
						                                                                 new Rect(0, 0, particleTexture.width, particleTexture.height),
						                                                                 new Vector2(0.5f,0.5f));
					}
					else{
						Debug.LogError ("Cannot locate SpriteRenderer");
					}
				}
				newBulletScript.damage = newBulletDmg;
				newBulletScript.crit = false;
			}

			lastFire = Time.time;
			LevelManager.shotsFired++;
		}
	}
}
