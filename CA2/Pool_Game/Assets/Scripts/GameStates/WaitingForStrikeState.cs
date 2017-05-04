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
	public class WaitingForStrikeState  : AbstractGameObjectState {
		private GameObject cue;
		private GameObject cue_ball;
		private GameObject main_camera;
		private PoolGameController gameController; // assign the pool controller to game controller variable

        // Inheritance the pool game controller
        public WaitingForStrikeState(MonoBehaviour parent) : base(parent) { 
			gameController = (PoolGameController)parent;
			cue = gameController.cue;
			cue_ball = gameController.cueBall;
			main_camera = gameController.mainCamera;

			cue.GetComponent<Renderer>().enabled = true;
		}

		public override void Update() {
			var x = Input.GetAxis("Horizontal");
			
			if (x != 0) {
				var angle = x * 75 * Time.deltaTime;
                // Quaternion.AngleAxis is a static & Creates a rotation which rotates angle degrees around axis
                gameController.strikeDirection = Quaternion.AngleAxis(angle, Vector3.up) * gameController.strikeDirection;
                // roates around the cue ball
                main_camera.transform.RotateAround(cue_ball.transform.position, Vector3.up, angle);
				cue.transform.RotateAround(cue_ball.transform.position, Vector3.up, angle);
			}
			Debug.DrawLine(cue_ball.transform.position, cue_ball.transform.position + gameController.strikeDirection * 10); // debugging

			if (Input.GetButtonDown("Fire1")) {
				gameController.currentState = new GameStates.StrikingState(gameController);
			}
		}
	}
}