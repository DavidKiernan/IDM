using UnityEngine;
using System.Collections;
/*
 * * Used gamesplusjames endless runner tutorials as a guide
 * https://www.youtube.com/watch?v=GrQalFLtQT4&list=PLiyfvmtjWC_XmdYfXm2i1AQ3lKrEPgc9-
 * Accessed on 28 February 2017 
 * 
 * Unity Documentation [https://docs.unity3d.com/Manual/index.html] last Accessed 11 March 2017
*/
public class AnimatorCS : MonoBehaviour {
    
    public Sprite[] animFrames;

    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;

    public int animationIdx = 0;
	
    // Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        spriteRenderer.sprite = animFrames[0]; // initial sprite
	}
	
	// Update is called once per frame
	void Update () {
        AnimationSequence();
	}

    void AnimationSequence()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index = index % animFrames.Length;
        spriteRenderer.sprite = animFrames[index];
    }
}
