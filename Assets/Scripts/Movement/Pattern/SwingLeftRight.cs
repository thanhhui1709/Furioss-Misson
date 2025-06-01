using System;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/LeftRight")]
public class SwingLeftRight : IMovementPattern
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float swingRange;
    private Vector3 currentPos;
    private bool isSwingLeft;
    private Rigidbody2D rb;
    private float currentSpeed;
    public override bool isFinished => false;

    public override float offset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Initialize(Transform transform)
    {
        currentPos = transform.position;
        isSwingLeft = currentPos.x >= 0 ? true : false;
        rb = transform.GetComponent<Rigidbody2D>();
        currentSpeed = speed * 0.5f;
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {

        float distance = Vector3.Distance(currentPos, transform.position);
        if (distance <= swingRange)
        {
            float speedRatio = Mathf.Clamp01(distance / swingRange);
            currentSpeed = speedRatio >= 0&&speedRatio>=0.5f ? speedRatio * speed : currentSpeed;
            Vector3 moveDirection = isSwingLeft ? Vector3.left  : Vector3.right;
            rb.linearVelocity = moveDirection  * currentSpeed;

        }
        else
        {
            isSwingLeft = !isSwingLeft;
            currentPos = transform.position;
            currentSpeed = speed * 0.5f;
            rb.linearVelocity = Vector2.zero;

        }

    }
}
