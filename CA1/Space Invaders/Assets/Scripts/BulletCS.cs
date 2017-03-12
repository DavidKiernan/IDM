/* Used gamesplusjames endless runner tutorials as a guide
* https://www.youtube.com/watch?v=GrQalFLtQT4&list=PLiyfvmtjWC_XmdYfXm2i1AQ3lKrEPgc9-
* Accessed on 28 February 2017 
* 
* Unity Documentation [https://docs.unity3d.com/Manual/index.html] last Accessed 11 March 2017
* 
* Unity Space Shooter tutorial Accessed on 8 March 2017
* https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial
*/
using UnityEngine;
using System.Collections;

public class BulletCS : MonoBehaviour {

    private Vector3 bulletPos;
    public ShooterCS shooter;    // Shooter script for access to it methods
    private GameManagerCS gm;

    public GameObject explosionPrefab;
    private GameObject explosionObject;

    public float explosionTime;

	public AudioClip impactSound;
	public AudioClip bombimpactSound;

	//Awake is used to initialize any variables or game state before the game starts. 
	//Awake is called only once during the lifetime of the script instance. 
	//Awake is called after all objects are initialized so you can safely speak to other objects or query them 

    void Awake()
    {
        shooter = GameObject.Find("Player").GetComponent<ShooterCS>();
        gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
    }
		
	
	// Update is called once per frame
	void Update () {
        MoveBullet();
	}

    void MoveBullet() // same as the move bomb in bomb script
    {
        bulletPos = this.transform.position;
        bulletPos.y += gm.bulletSpeed;
		if (bulletPos.y > gm.saucerLocationOnYaxis) { // bullet is greter than the saucer destroy it
			DestroyBullet ();
		}

        this.transform.position = bulletPos;
       
    }

    void DestroyBullet()
    {
        shooter.bulletMade = false;     // set that a bullet was made to false doesn't exist anymore
        MakeExplosion(this.transform.position);
        Destroy(this.gameObject);
    }

    void MakeExplosion(Vector3 explosionPos)  // same as bomb script
    {
        explosionObject = (GameObject)Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
        explosionObject.name = "Explosion";
        Destroy(explosionObject, explosionTime);
    }

    void OnTriggerEnter2D(Collider2D other) // If bullet hits an something, destroy it
    {
        if (other.gameObject.tag == "Enemy") // hit enemy, destroy enemy if the object has an enemy tag
        {
			// Debugging 
            //Debug.LogError("Bullet hit: " + other.gameObject.name);
            gm.gameScore += other.gameObject.GetComponent<EnemyCS>().value; // add enemy value to the score in the game manager
            Destroy(other.gameObject); // destroy the enemy
			AudioSource.PlayClipAtPoint(impactSound, transform.position, 0.5f); // play impact sound sound
			gm.CheckLevel();  // see what level the player is on
            DestroyBullet();
        }
		else if (other.gameObject.tag == "Saucer")  // hit the saucer
		{
			//Debug.LogError("Bullet hit: " + other.gameObject.name);
			gm.gameScore += other.gameObject.GetComponent<SaucerCS>().value;
			Destroy(other.gameObject);
			AudioSource.PlayClipAtPoint(impactSound, transform.position, 0.5f);
			DestroyBullet();
			gm.saucerMade = false; // saucer is destroyed so set to false
		}
        else if (other.gameObject.tag == "Shelter")  // hit the shelter
        {
            Destroy(other.gameObject);
            DestroyBullet();
        }
    }
}

