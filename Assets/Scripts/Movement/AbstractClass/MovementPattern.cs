using UnityEngine;


public abstract class MovementPattern : ScriptableObject
{
    public abstract void Initialize(Transform transform);
    public abstract void UpdateMovement(Transform transform,float deltaTime);
    public abstract bool isFinished { get; }
}
