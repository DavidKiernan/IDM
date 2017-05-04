/* 
    David Kiernan     
    With thanks to Paul Fathy for guiding me through this and the other scripts
      used the unity manual as help 
    available  https://docs.unity3d.com/ScriptReference/
    Accessed on : 1st May 2017

    https://msdn.microsoft.com/en-gb/library/618ayhy6.aspx accessd on 3 May 2017
 */
using UnityEngine;
using System.Collections;

namespace GameStates {
	public class StrikingState : AbstractGameObjectState {
		private PoolGameController gameController;

		private GameObject cue;
		private GameObject cue_ball;

		private float cue_direction = -1;   // The direction the cue is moved
        private float speed = 7;            //// How fast the cue will move

        public StrikingState(MonoBehaviour parent) : base(parent) { 
			gameController = (PoolGameController)parent;
			cue = gameController.cue;
			cue_ball = gameController.cueBall;
		}

		public override void Update() {
			if (Input.GetButtonUp("Fire1")) {
				gameController.currentState = new GameStates.StrikeState(gameController);
			}
		}

		public override void FixedUpdate () {
			var distance = Vector3.Distance(cue.transform.position, cue_ball.transform.position);
            // moves the cue 
            if (distance < PoolGameController.MIN_DISTANCE || distance > PoolGameController.MAX_DISTANCE)
            {
                cue_direction *= -1;
            }
            // The interval in seconds at which physics and other fixed frame rate updates	
            cue.transform.Translate(Vector3.down * speed * cue_direction * Time.fixedDeltaTime);
		}
	}
}