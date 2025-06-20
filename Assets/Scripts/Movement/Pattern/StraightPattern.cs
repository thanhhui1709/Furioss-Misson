using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/Straight")]
public class StraightPattern : IMovementPattern
{
    public float speed = 7f;
    public float duration = 3f;
    private float time;
    private Rigidbody2D rb;
  
    public override bool isFinished => time>=duration;

    public override float offset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Initialize(Transform transform)
    {
        time = 0;
        rb=transform.GetComponent<Rigidbody2D>();
    }

    public override void UpdateMovement(Transform transform, float deltaTime)

    {
        time+= deltaTime;

        // Move in the "up" direction (can be changed to right/left/etc)
        rb.MovePosition(rb.position + Vector2.down * speed * deltaTime);
        if (time >= duration) return;
    }

}
