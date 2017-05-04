/* 
    David Kiernan     
    With thanks to Paul Fathy for guiding me through this and the other scripts
    used the unity manual as help 
    available  https://docs.unity3d.com/ScriptReference/
    Accessed on : 1st May 2017

 */
using UnityEngine;
using System.Collections;

public class SnookerBallController : MonoBehaviour {
	void Start() {
        // force it to sleep at this speed
        GetComponent<Rigidbody>().sleepThreshold = 0.15f;  
	}

	void FixedUpdate () {
		var rigidbody = GetComponent<Rigidbody>();
		if (rigidbody.velocity.y > 0) {
			var velocity = rigidbody.velocity;
			velocity.y *= 0.3f;    // gets the y-axis times 0.32
            rigidbody.velocity = velocity;
		}
	}
}
