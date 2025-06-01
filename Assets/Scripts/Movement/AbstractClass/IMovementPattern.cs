using UnityEngine;


public abstract class IMovementPattern : ScriptableObject
{
    public abstract void Initialize(Transform transform);
    public abstract void UpdateMovement(Transform transform,float deltaTime);
    public abstract bool isFinished { get; }
    public abstract float offset { get; set; }
}
