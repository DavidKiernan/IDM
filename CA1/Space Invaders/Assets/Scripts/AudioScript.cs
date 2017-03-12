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

public class AudioScript : MonoBehaviour {

	private GameManagerCS gm;

	public AudioClip musicSource;
	private bool isPlaying;

	void awake(){
		gm = GameObject.Find ("GameManager").GetComponent<GameManagerCS>();
	}

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().clip = musicSource; // get the clip 
	}
	
	// Update is called once per frame
	void Update () {
		if (gm.gameRunning) 
		{
			GetComponent<AudioSource> ().Stop ();  // stops the audiop
		}
		else
		{
			if (!isPlaying) // not oplaying then play
			{
				PlayAudio();
			}
		}
	}

	void PlayAudio()
	{
		GetComponent<AudioSource>().Play(); // play the sound
		isPlaying = true; 
	}
}
