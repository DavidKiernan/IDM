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

public class BombCS : MonoBehaviour {

	private Vector3 bombPos;
	
	public GameObject parentObj;
	private GameManagerCS gm;
	
	public GameObject explosionPrefab;
	private GameObject explosionObject;
	
	public float explosionTime;
	
	void Awake()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
	}
	
	// Use this for initialization
	void Start () 
	{
		parentObj = this.transform.parent.gameObject; 
		this.transform.parent = null;
	}

	// Update is called once per frame
	void Update () 
	{
		MoveBomb ();
	}

	void MoveBomb()
	{
		bombPos = this.transform.position;  // position of bomb is based on the postion
		bombPos.y -= gm.bombSpeed;			// postion on y axis is the speed set in the game manager
		if (bombPos.y < gm.missileMin) {
			DestroyBomb ();					// destroy the bomb once it is lower than the min value for the missile in game manager
		}
		
		this.transform.position = bombPos;  // otherwise the postion of the bomb is this now postion.
	}

	void DestroyBomb()
	{
		if (parentObj != null)  // bomb is present
		{
			parentObj.GetComponent<SaucerCS>().bombMade = false;   
		}
		MakeExplosion(this.transform.position); // make the explosion happen at this postion
		Destroy(this.gameObject);		// destroy this game object
	}

	void MakeExplosion(Vector3 explosionPos)
	{
		explosionObject = (GameObject)Instantiate(explosionPrefab, explosionPos, Quaternion.identity); // get the explosion prefab, postion and the "no rotation" - the object is perfectly aligned with the world or parent axes.
		explosionObject.name = "Explosion";  // get the object with the "Explosion"
		Destroy(explosionObject, explosionTime);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")		// hit the player destroy the player, player loses life and set to true that player is dead.
		{
			// For debugging purposes
			//Debug.Log("Bullet hit: " + other.gameObject.name);
			gm.playerDead = true;
			gm.playerLives--;
			Destroy(other.gameObject);
			DestroyBomb();
		}
		else if(other.gameObject.tag == "Shelter") // or hit the shelter destroy it and the bomb
		{
			Destroy(other.gameObject);
			DestroyBomb();
		}
	}
}
