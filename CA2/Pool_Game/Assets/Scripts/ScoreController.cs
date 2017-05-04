/* 
    David Kiernan     
    With thanks to Paul Fathy for guiding me through this and the other scripts
    used the unity manual as help 
    available  https://docs.unity3d.com/ScriptReference/
    Accessed on : 1st May 2017
 */
using UnityEngine;
using System;
using System.Collections;

public class ScoreController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var text = GetComponent<UnityEngine.UI.Text>();
		var currentPlayer = PoolGameController.GameInstance.Player_one;
		var otherPlayer = PoolGameController.GameInstance.Player_two;
        // format in the way {0} = current player name (player1), {1} is points same for the other player
        text.text = String.Format("* {0} - {1}\n{2} - {3}", currentPlayer.Name, currentPlayer.Points, otherPlayer.Name, otherPlayer.Points);
	}
}
