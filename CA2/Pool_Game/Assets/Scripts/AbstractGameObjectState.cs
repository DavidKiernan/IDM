/* 
    David Kiernan     
    With thanks to Paul Fathy for guiding me through this and the other scripts
     The virtual keyword is used to modify a method, property, indexer, or event declaration and allow for it to be overridden in a derived class
     used the unity manual as help 
    available  https://docs.unity3d.com/ScriptReference/
    Accessed on : 1st May 2017

    https://msdn.microsoft.com/en-gb/library/618ayhy6.aspx accessd on 3 May 2017
    */
using UnityEngine;

public abstract class AbstractGameObjectState : IGameObjectState {
	protected MonoBehaviour parent;
	public AbstractGameObjectState(MonoBehaviour parent) {
		this.parent = parent;
	}

	public virtual void Update() { }
	public virtual void FixedUpdate() { }
	public virtual void LateUpdate() { }
}