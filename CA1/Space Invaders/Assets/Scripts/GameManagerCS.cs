/*
 * Private will hide attribute in the Inspector
 * Public will allow them to be adjusted to what the user will like and also for in game testing & to access the other scripts
 * GameObject PauseMenu is not working correctly player can fire but enemies won't be destoryed.
 * Used gamesplusjames endless runner tutorials as a guide
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
public class GameManagerCS : MonoBehaviour {

    public GameObject playerPrefab;
    private GameObject playerObject;
	public GameObject PauseMenu; 
    public GameObject[] enemyPrefab; // Set which enemy goes where element 1 here corresponds to element 1 in enemyValue
    private GameObject[] enemyObject; // Number of enemies to appear set to 55 
	public int[] enemyValue; // Value score of the enemy ( size is size of array)

    public GameObject groundPrefab;
    private GameObject groundObject;

	public GameObject saucerPrefab;
	private GameObject saucerObject;

	private GameObject bulletObject;
	private GameObject bombObject;
	private GameObject[] missileObject;
	private GameObject[] explosionObject;

    public GameObject shelterPrefab;
    private GameObject[] shelterObject;

    public Vector3 playerPos = Vector3.zero;
    public Vector3 enemyPos = Vector3.zero;
    public Vector3 groundPos = Vector3.zero;
	public Vector3 saucerPos = Vector3.zero;

    public GameObject gameScreenPrefab;
    private GameObject gameScreen;

    private GameObject scoreScreen;
	private GameObject HSPanel;
	private GameObject LevelPanel;
    private GameObject gameGUI;
    public GameObject[] gameLife;

	// Shelter Information
    public float shelterPos = 0f;
    public float shelterSpread = 2.25f;
    private int shelterNr; 

	// Player Information
    public float playerSpeed;
    private int playerDirection;
    public float playerBounds;
	public int playerLives;
	public int startLives;
	public int maxLives;
    public bool playerMade;
	public bool playerDead;
	public float playerRespawn;
	public float bulletSpeed;
	// PSeed is just to slow the player speed
	private float PSpeed = 0;


	// Basic Enemies Information
    public float invaderBounds;
    public float invaderSpeed;
	// ESpeed is just to slow the player speed
	private float ESpeed = 0;
    public int invaderDirection;
    public float invaderBottom;
	public int invaderNr;   // Number of enemies to appear
	public int maxInvaders; // Max number that can appear
	// Enemy shot
	public float missileSpeed;
	public float missilePercent; // Chances of the missle firing
	public int totalMissiles; // total missiles on screen together
	public int maxMissiles;  // Max number of missiles will increase by level
    public float missileMin; // Min number of missiles allowed to fire
    public bool moveDown; // check the enemy has moved
    

	// Special enemy floats across the top Information
	public int saucerType;
	public int saucerScore;
	public bool saucerMade;
	public int saucerDirection;
	public float saucerSpeed;
	public float saucerBounds;
	public float saucerTimer;
	public float saucerLocationOnYaxis;
	public float bombPercent;    
	public float bombSpeed;

   // Game Information
    public int rowSize;
    public float rowSpacing;
    public bool gameOver;
    public bool gameStart;
    public bool gameRunning;
	public bool levelClear;
	public int currentInvaders;
	public float currentLevel;
	public int gameLevel;
	public int gameScore;
	public int highScore;
	public int levelText;

	// Sound Information
	public AudioClip[] invadermarchSound;
	public float marchTimer = 0f;
	public float marchTimer2 = 0f;
	public bool soundPlayed = false;
	
	public AudioClip extralifeSound;
	public AudioClip stageclearSound;
	public AudioClip gameoverSound;

	public int pitchCount = 0; //change the pitch of the sounds

    void Awake()
    {
        enemyObject = new GameObject[55]; // number of enemies
        shelterObject = new GameObject[4]; // Number of shelters to protect the player
        SetupGame();
    }

    // Use this for initialization
	void Start () {
        MakeBackground();
        scoreScreen.SetActive(true);
		LevelPanel.SetActive(true);
        gameGUI.SetActive(false);
		PauseMenu.SetActive (false);
    }

	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.Escape)) // esc key will exit the application
		{
			Application.Quit();
		}
        if (!gameStart && !gameRunning) // game not started & no game running press s key to start
        {
            if (Input.GetKey(KeyCode.S))
			{
                gameStart = true;
            }
        }

	

        if(gameRunning)
        {
			// Pause Game NOT Working Correctly, Not stopping shots Just dont destory

			if (Input.GetKeyDown (KeyCode.P)) { 
				Time.timeScale = 0f; // Freeze the game
				PauseMenu.SetActive (true);
				PSpeed = playerSpeed; // gives the PSpeed the player value before setting it zero.
				playerSpeed = 0;
				ESpeed = invaderSpeed;
				invaderSpeed = 0;
			}

			if (Input.GetKeyDown (KeyCode.R) ) // Pressing the 'R' key will activate this
			{
				
				Time.timeScale = 1f; // Freeze the game
				PauseMenu.SetActive(false);
				playerSpeed = PSpeed;
				invaderSpeed = ESpeed;
			}

			
            if (playerLives <= 0) // Player has no lives remaining
            {
                gameOver = true;
            }

            if (playerDead && !gameOver)  // Used when player takes a hit
            {
                MakePlayer();
				ShowLives(playerLives);
            }

			if (playerObject != null) // Player object call the move player if player is present
			{
				MovePlayer ();
			}

            if (moveDown)   
            {
                foreach (GameObject entity in enemyObject) // get all enemies
                {
                    if (entity != null)   // enemy present then move down is true so they drop down  
                        entity.GetComponent<EnemyCS>().moveDown = true;
                }
                moveDown = false;
            }
				

			if (Time.time > marchTimer && !soundPlayed)
			{
				PlayInvaderMarch(0);
			}

			if (Time.time > marchTimer2 && soundPlayed)
			{
				PlayInvaderMarch2(0);
			}

			if(!saucerMade && Time.time > saucerTimer) // no saucer and time is bigger than sacuer time make one
			{
				MakeSaucer();
			}

			
			if (levelClear)
			{

				AudioSource.PlayClipAtPoint(stageclearSound, transform.position, 2f);
				levelClear = false;
				soundPlayed = true;
				ClearBoard();
				NewLevel();
				AddPlayerLife(); // adds a new life to the playet
			}

            if (gameOver)
            {
				AudioSource.PlayClipAtPoint(gameoverSound, transform.position, 2f);
                ClearBoard();
                scoreScreen.SetActive(true);
				LevelPanel.SetActive(true);
                gameGUI.SetActive(false); // close the game
                gameRunning = false;
				if (gameScore > highScore) { // score for this current becomes the high score while game is running
					highScore = gameScore;
				}
            }
		}
	}


    void SetupGame()
    {
        gameStart = false;
		highScore = 0;
        scoreScreen = GameObject.Find("TitlePanel");
		LevelPanel = GameObject.Find("LevelPanel");
		HSPanel = GameObject.Find("HSPanel");
        gameGUI = GameObject.Find("GameGUI");
		saucerTimer = Time.time + Random.Range(5.0f, 10.0f);  // random time for the saucer to be creted.
    }

    void LateUpdate()
    {
        if(gameStart)
        {
            GameInit();
        }
    }

    void NewLevel()
    {
        gameOver = false;
        playerDead = true;
		saucerMade = false;
        invaderDirection = 1;
        invaderNr = 0;
		currentInvaders = 0;
        totalMissiles = 0;
		maxMissiles = (gameLevel + 1); // increase the max number of enemy shots by 1 on each new level
        shelterNr = 0;
		levelClear = false;
		ClearBoard();
        MakeInvaders();
		levelText = gameLevel;

		maxInvaders = invaderNr;

		currentLevel = (gameLevel + 1) / 10f;

		if(gameScore > highScore)
			highScore = gameScore;

		saucerTimer = Time.time + Random.Range(5.0f, 10f);

        for (int i = 0; i <= 1; i++)
        {
            MakeShelter(new Vector3(shelterSpread + (2.25f * i), shelterPos, 0f));
            MakeShelter(new Vector3(-shelterSpread - (2.25f * i), shelterPos, 0f));
        }
        
        MakeGround(groundPos);
    }
		


    void GameInit()
    {
        gameRunning = true;
        gameStart = false;
        scoreScreen.SetActive(false);
		LevelPanel.SetActive(true);
        gameGUI.SetActive(true);
        gameScore = 0;
		playerLives = 3;
		ShowLives(playerLives);
		CheckLevel();
        NewLevel();
		gameLevel = 0;
		levelText = 0;
    }

    public void ClearBoard()
    {
		DestroyBullet();
		DestroyMissile();
		DestroyBomb();
		DestroyExplosion();
		DestroyEnemies();
        DestroyBases();
        DestroyGround();
		DestroySaucer();
		DestroyPlayer();
		currentInvaders = 0;
    }

    void MakePlayer()
    {
        playerObject = (GameObject)Instantiate(playerPrefab, playerPos, Quaternion.identity);
        playerObject.name = "Player";
        playerMade = true;
        playerDead = false;
    }


	void AddPlayerLife()
	{
		playerLives++;
		AudioSource.PlayClipAtPoint(extralifeSound, transform.position, 2f); // Play the extra life sound
		if (playerLives >= maxLives) playerLives = maxLives; // player can't have more than user set of lives
		ShowLives(playerLives);
		gameLevel++;
		levelText++;
	}

	// Making everything needed for the game, background, scores, enemies, player, shelters etc

    void MakeInvaders()
    {
        MakeInvaderRow(2, 2.0f); 
        MakeInvaderRow(1, 1.5f);
        MakeInvaderRow(1, 1.0f);
        MakeInvaderRow(0, 0.5f);
        MakeInvaderRow(0, 0.0f);
    }

    void MakeInvaderRow(int invaderType, float invaderLoc)  // Put enemies across a row
    {
        float rowStart = rowSize /2 * rowSpacing;
        for (int row = 0; row < rowSize; row++)
        {
            float rowPos = rowStart - (row * rowSpacing);
            MakeInvader(invaderType, new Vector3(rowPos, invaderLoc, enemyPos.z));
        }
    }

    void MakeInvader(int invaderType, Vector3 enemyPos) // creates enemies
    {
        enemyObject[invaderNr] = (GameObject)Instantiate(enemyPrefab[invaderType], enemyPos, Quaternion.identity) as GameObject;
        enemyObject[invaderNr].name = "Invader " + (invaderNr + 1).ToString();
        enemyObject[invaderNr].GetComponent<EnemyCS>().value = enemyValue[invaderType];
        invaderNr++;
		currentInvaders++;
    }

	// Makes the ground bottom of the screen.

    void MakeGround(Vector3 groundPos)
    {
        groundObject = (GameObject)Instantiate(groundPrefab, groundPos, Quaternion.identity);
        groundObject.name = "Ground";
    }

    void MakeBackground()
    {
        gameScreen = (GameObject)Instantiate(gameScreenPrefab, Vector3.zero, Quaternion.identity);
        gameScreen.name = "Background";
    }
	
	void MakeSaucer()
	{
		saucerType = Random.Range (0, 4);
		if (Random.Range (0.0f, 1.0f) < 0.5f) 
		{
			saucerDirection = -1;
		}
		else
		{
			saucerDirection = 1;
		}
		saucerScore = (saucerType + 1) * 50;  // value of the saucers 
		if (saucerType == 2)
		{
			saucerScore = 150;
		}
		if (saucerType == 3)
		{
			saucerScore = 300;
		}

		// Get saucer pos based on the x axis bounds, loctaion in yaxis can be altered in the Inspector
		saucerPos = new Vector3 ((saucerBounds * -saucerDirection), saucerLocationOnYaxis - 0.5f, 0.0f);


		// So as not to keep mass spawn saucer
		if (!saucerMade)
		{
			saucerObject = (GameObject)Instantiate(saucerPrefab, saucerPos, Quaternion.identity);
			saucerObject.GetComponent<SaucerCS>().value = saucerScore; 
			saucerObject.GetComponent<SaucerCS>().type = saucerType;
			saucerObject.name = "Saucer";

			// Spawns Saucer with different colors & scores
			if (saucerType <= 1)
			{
				saucerObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
			}
			if (saucerType == 2)
			{
				saucerObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
			}
			if (saucerType == 3)
			{
				saucerObject.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
			}

			// Time which saucer will appear random range
			saucerTimer = Time.time + Random.Range(5.0f, 10f);
			saucerMade = true;
		}
	}


    void MakeShelter(Vector3 shelterPos)
    {
        shelterObject[shelterNr] = (GameObject)Instantiate(shelterPrefab, shelterPos, Quaternion.identity);
        shelterObject[shelterNr].name = "Shelter";
        shelterObject[shelterNr].tag = "Shelter";
        shelterNr++;
    }

    void MovePlayer()
    {
        playerPos = playerObject.transform.position;
        playerDirection = 0;
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ) // move ship left
        {
            playerDirection = -1;   // Move -1 on the x-axis
        }
		else if (Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow)) // move player right
        {
            playerDirection = 1;
        }

		// Player goes beyond the set bound than the min & max value of the bound are the player position
        playerPos.x += playerSpeed * playerDirection;
		if (playerPos.x < -playerBounds) {
			playerPos.x = -playerBounds;
		} 
		else if (playerPos.x > playerBounds) {
			playerPos.x = playerBounds;
		}

        playerObject.transform.position = playerPos;
    }

	// Destroy Objects

    void DestroyEnemies()
    {
        foreach (GameObject entity in enemyObject)
        {
			if (entity != null) {
				Destroy (entity.gameObject);
			}
        }
    }

    void DestroyBases()
    {
        foreach (GameObject baseObj in shelterObject)
        {
			if (baseObj != null) {
				Destroy (baseObj.gameObject);
			}
        }
    }

    void DestroyGround()
    {
		if (groundObject != null) {
			Destroy (groundObject.gameObject);
		}
    }

	void DestroySaucer()
	{
		saucerObject = GameObject.FindGameObjectWithTag ("Saucer"); // find the object with the tag of name case sensetive =
		Destroy (saucerObject);
	}

	void DestroyBullet()
	{
		bulletObject = GameObject.FindGameObjectWithTag ("Bullet");
		Destroy (bulletObject);
	}

	void DestroyMissile()
	{
		missileObject = GameObject.FindGameObjectsWithTag ("Missile");
		foreach (GameObject missile in missileObject) 
		{
			Destroy(missile);
		}
	}

	void DestroyBomb()
	{
		bombObject = GameObject.FindGameObjectWithTag ("Bomb");
		Destroy (bombObject);
	}

	void DestroyPlayer()
	{
		playerObject = GameObject.FindGameObjectWithTag ("Player");
		Destroy (playerObject);
	}

	void DestroyExplosion()
	{
		explosionObject = GameObject.FindGameObjectsWithTag ("Explosion");
		foreach (GameObject explosion in explosionObject) 
		{
			Destroy(explosion);
		}
	}

   public void ShowLives(int playerLife)
    {
		for (int i = 0; i < maxLives - 1; i++)
		{
			gameLife[i].SetActive(false); // gets the max number of lives
		}

		for (int i = 0; i < playerLives - 1; i++)
		{
			gameLife[i].SetActive(true); // get the players lives and show them
		}
    }
		
	public void CheckLevel()
	{
		currentInvaders--;
		if (currentInvaders <= 0){ // if true create a new level if no more invaders
			levelClear = true;
		}
	}

	// Plays sounds
	void PlayInvaderMarch(int type)
	{
		pitchCount = ++pitchCount % 4;
		GetComponent<AudioSource>().clip = invadermarchSound[type];
		GetComponent<AudioSource>().pitch = 1.0f - (pitchCount * 0.1f);
		GetComponent<AudioSource>().Play();
		marchTimer2 = marchTimer + 0.1f;
		soundPlayed = true;
	}

	void PlayInvaderMarch2(int type)
	{
		if (currentInvaders > 0)
			marchTimer = Time.time + (currentInvaders / 70f);
		else {
			marchTimer = Time.time + 0.01f;
		}
	soundPlayed = false;
	}
}
