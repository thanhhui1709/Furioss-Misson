using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/Straight")]
public class StraightPattern : IMovementPattern
{
    public float speed = 7f;
    public float duration = 3f;
    private float time;
  
    public override bool isFinished => time>=duration;

    public override float offset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Initialize(Transform transform)
    {
        time = 0;
      
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        time+= deltaTime;
        transform.Translate(transform.up * speed * Time.deltaTime,Space.World);
        if (time >= duration) return;
    }

}
