using UnityEngine;
using System;

namespace GameStates {
	public class StrikeState : AbstractGameObjectState {
		private PoolGameController gameController;
		
        // get the game object of the cue & cue ball
		private GameObject cue;
		private GameObject cue_ball;

		private float speed = 30f;
		private float force = 0f;
		
		public StrikeState(MonoBehaviour parent) : base(parent) { 
			gameController = (PoolGameController)parent;
			cue = gameController.cue;

            // setting the cue ball prefab to the varaible
            cue_ball = gameController.cueBall;
			var forceAmplitude = gameController.maxForce - gameController.minForce;
			var relativeDistance = (Vector3.Distance(cue.transform.position, cue_ball.transform.position) - PoolGameController.MIN_DISTANCE) / (PoolGameController.MAX_DISTANCE - PoolGameController.MIN_DISTANCE);
			force = forceAmplitude * relativeDistance + gameController.minForce;
		}

		public override void FixedUpdate () {
			var distance = Vector3.Distance(cue.transform.position, cue_ball.transform.position);
            // less than the min distance 
            if (distance < PoolGameController.MIN_DISTANCE)
            {
                // get the rigidbody & add force  of the strike direction times the force
                cue_ball.GetComponent<Rigidbody>().AddForce(gameController.strikeDirection * force);
                // makes the cue "disappear"
                cue.GetComponent<Renderer>().enabled = false;
                // set the waiting for a turn to a new one
                cue.transform.Translate(Vector3.down * speed * Time.fixedDeltaTime);
				gameController.currentState = new GameStates.WaitingForNextTurnState(gameController);
			}
            else
            {
				cue.transform.Translate(Vector3.down * speed * -1 * Time.fixedDeltaTime);
			}
		}
	}
}