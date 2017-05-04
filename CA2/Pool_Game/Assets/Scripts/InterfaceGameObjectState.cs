/* 
    David Kiernan     
    With thanks to Paul Fathy for guiding me through this and the other scripts
    An interface contains only the signatures of methods, properties, events or indexers.
    implements the interface must implement the members of the interface that are specified in the interface definition.
    used the unity manual as help 
    available  https://docs.unity3d.com/ScriptReference/
    Accessed on : 1st May 2017
    https://msdn.microsoft.com/en-gb/library/618ayhy6.aspx accessd on 3 May 2017
 */

public interface IGameObjectState {
    // update is called on every frame
    void Update();

    // called every fixed framerate frame. FxedUpdate should be used instead of Update when dealing with Rigidbody
    void FixedUpdate();

    // called every frame, if the Behaviour is enabled.
    void LateUpdate();
}