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

public class EnemyCS : MonoBehaviour {

    private Vector3 invaderPos;
    public bool moveDown;

    public GameObject missilePrefab;
    private GameObject missileObject;
    private GameManagerCS gm;
    
    public bool isBottom;
    public string hitName;
    
    public bool missileMade;
    public int value;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
    }

	// Use this for initialization
	void Start () {
        InitInvader();
	}
	
    void InitInvader()
    {
        missileMade = false;
        moveDown = false;
        isBottom = false;
    }

	// Update is called once per frame
	void Update () {
		MoveInvader();
	}

    void LateUpdate()
    {
        if (hitName.Contains("Invader"))
        {
            isBottom = false;
        }
        else
        {
            isBottom = true;
        }
    }

    void FixedUpdate()
    {

        CheckBottom();
        if (isBottom && !missileMade && gm.totalMissiles < gm.maxMissiles && Random.Range(0, 10) < 0.1)
        {
			missileMade = true;
            MakeMissile(invaderPos);
        }
    }

    void MoveInvader()
    {
        invaderPos = this.transform.position; // invaders start at this postion
 
        if(gm.invaderDirection == -1) // move left
        {
            if (invaderPos.x < -gm.invaderBounds) // invader x axis postion less than the game manager - bouinds
            {
                invaderPos.x = -gm.invaderBounds;  // postion is now the game manager neg postion
                gm.invaderDirection = 1;  // move the enemy right once the move down a row
                gm.moveDown = true;		// set to true to move a row down
            }
            else 	// increase speed
            {
                invaderPos.x -= gm.invaderSpeed * (gm.maxInvaders/ gm.currentInvaders) * gm.currentLevel * 0.01f;
            }
        }
        else if(gm.invaderDirection == 1) // same as above but going in the opposite direction
        {
            if (invaderPos.x > gm.invaderBounds)
            {
                invaderPos.x = gm.invaderBounds;
                gm.invaderDirection = -1;
                gm.moveDown = true;         
            }
            else   // increase speed
            {
				invaderPos.x += gm.invaderSpeed * (gm.maxInvaders/ gm.currentInvaders) * gm.currentLevel * 0.01f;
            } 
        }

        if (invaderPos.y < gm.invaderBottom)    // enemies hit the bottom of screen
        {
            gm.playerDead = true;   //player is now dead
            gm.playerLives--;      // loses life
            Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);  
            gm.gameOver = true;  // display the game over screen
        }

        if (moveDown) 
        {
            
            invaderPos.y -= 0.25f;  // move enemy down on the y axis
            moveDown = false;  
        }
        this.transform.position = invaderPos;
    }


    void MakeMissile(Vector3 enemyPos)
    {
        missileObject = (GameObject)Instantiate(missilePrefab,new Vector3(enemyPos.x, enemyPos.y - 0.5f, enemyPos.z),Quaternion.identity);
        missileObject.name = "Missile";
        missileObject.transform.parent = this.transform;
        missileMade = true;
        gm.totalMissiles++;
    }

    void CheckBottom()
    {
		// used to detect objects that lie along the path of a ray and is conceptually 
		//like firing a laser beam into the scene and observing which objects are hit by it
        RaycastHit2D hit = Physics2D.Raycast((new Vector2(this.transform.position.x, this.transform.position.y - 0.25f)),-Vector2.up);

        hitName = string.Empty;
        if (hit.collider != null)
        {
            hitName = hit.collider.gameObject.name;
        }
    }

    void OnTriggerEnter2D(Collider2D other)  // same as the bomb script
    {
        if (other.gameObject.tag == "Shelter")
        {
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "Player")
        {
            gm.playerDead = true;
            gm.playerLives--;
            Destroy(other.gameObject);
            gm.gameOver = true;
        }
    }
}
