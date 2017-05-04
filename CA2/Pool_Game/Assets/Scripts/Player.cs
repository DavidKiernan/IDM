/* 
    David Kiernan     
    With thanks to Paul Fathy for guiding me through this and the other scripts
      used the unity manual as help 
    available  https://docs.unity3d.com/ScriptReference/
    Accessed on : 1st May 2017

    https://msdn.microsoft.com/en-gb/library/618ayhy6.aspx accessd on 3 May 2017
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player {
	private IList<Int32> ballsCollected = new List<Int32>();

	public Player(string name) {
		Name = name;
	}

	public string Name {
		get;
		private set;
	}

	public int Points {
		get { return ballsCollected.Count; }
	}

	public void Collect(int ballNumber) {
		Debug.Log(Name + " collected ball " + ballNumber);  // debugging
		ballsCollected.Add(ballNumber);
	}
}
