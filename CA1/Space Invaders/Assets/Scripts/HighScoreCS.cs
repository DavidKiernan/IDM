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
using UnityEngine.UI;
using System;
using System.Collections;

public class HighScoreCS : MonoBehaviour {

    private GameManagerCS gm;
    Text txt;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManagerCS>();
    }
    
    // Use this for initialization set the score to 0000
    void Start () {
        txt = gameObject.GetComponent<Text>();
        txt.text = gm.highScore.ToString("0000");
    }
	
	// Update is called once per frame
	void Update () {
        txt = gameObject.GetComponent<Text>();
        txt.text = gm.highScore.ToString("0000");
    }
}
