/* 
    David Kiernan     
    With thanks to Paul Fathy for guiding me through this and the other scripts
    the controller for the game
    used the unity manual as help 
    available  https://docs.unity3d.com/ScriptReference/
    Accessed on : 1st May 2017

    https://msdn.microsoft.com/en-gb/library/618ayhy6.aspx accessd on 3 May 2017
 */
using UnityEngine;
using System.Collections;

public class PoolGameController : MonoBehaviour {
	public GameObject cue;
	public GameObject cueBall;
	public GameObject redBalls;
	public GameObject mainCamera;
	public GameObject scoreBar;
	public GameObject winnerMessage;

	public float maxForce;
	public float minForce;
	public Vector3 strikeDirection;

	public const float MIN_DISTANCE = 27.5f;
	public const float MAX_DISTANCE = 32f;
	
	public IGameObjectState currentState;

    // the players
    public Player Player_one;
	public Player Player_two;

    // decides if the current player contiue to play
    private bool currentPlayerContinuesToPlay = false;

	// This is kinda hacky but works
	static public PoolGameController GameInstance {
		get;
		private set;
	}

	void Start()
    {
		strikeDirection = Vector3.forward;
        // sets the player name, if time let user set it
        Player_one = new Player("P1");   
		Player_two = new Player("P2");
		GameInstance = this;
		winnerMessage.GetComponent<Canvas>().enabled = false;
        currentState = new GameStates.WaitingForStrikeState(this);
    }
	
	void Update() {
		currentState.Update();
	}

	void FixedUpdate() {
		currentState.FixedUpdate();
	}

	void LateUpdate() {
		currentState.LateUpdate();
    }

    //ball if pocketed then player continues
    public void BallPocketed(int ballNumber) {
		currentPlayerContinuesToPlay = true;
        Player_one.Collect(ballNumber);
	}

    // player doesn't pocket a ball, allow for fouls. need to fix for black ball or create new method 
    public void NextPlayer() {
		if (currentPlayerContinuesToPlay) {
			currentPlayerContinuesToPlay = false;
			Debug.Log(Player_one.Name + " continues to play");   // debugging
			return;
		}

		Debug.Log(Player_two.Name + " will play");  // debugging
		var aux = Player_one;
        Player_one = Player_two;
		Player_two = aux;
	}

    // end the match give the message of who won 
    public void EndMatch() {
		Player winner = null;
		if (Player_one.Points > Player_two.Points)
			winner = Player_one; 
		else if (Player_one.Points < Player_two.Points)
			winner = Player_two;

		var message = "Game Over\n";

        if (winner != null)
        {
            message += string.Format("The winner is '{0}'", winner.Name);
        }
        // same number of ball potted as no black ball ( ran out of time)
        else
            message += "It was a draw!";

		var text = winnerMessage.GetComponentInChildren<UnityEngine.UI.Text>();
		text.text = message;
		winnerMessage.GetComponent<Canvas>().enabled = true;
	}
}
