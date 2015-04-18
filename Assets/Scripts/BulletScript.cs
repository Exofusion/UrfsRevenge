using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	public TextMesh damageTextPrefab;

	public int damage;
	public bool crit;


	// Use this for initialization
	void Start () {
		Destroy (gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.tag == "Player" && gameObject.tag == "EnemyBullet") {
			PlayerScript player = collision.gameObject.GetComponent<PlayerScript> ();
			player.TakeDamage (damage);
		}
		if (collision.gameObject.tag != gameObject.tag) {
			if (collision.gameObject.tag == "Enemy")
			{
				LevelManager.shotsHit++;
			}
			Destroy (gameObject);
		}
	}

	public void DisplayDamage( Vector3 textPosition, bool inverted = false )
	{
		TextMesh damageText = Instantiate (damageTextPrefab, textPosition, Quaternion.identity) as TextMesh;
		DamageTextScript damageScript = damageText.GetComponent<DamageTextScript> ();

		if (crit) {
			damageText.color = Color.red;
			damageText.transform.localScale = damageText.transform.localScale*1.25f;
		}
		/*else
			damageText.color = Color.white*/;

		damageText.text = damage.ToString ();
		damageScript.inverted = inverted;
	}
}
