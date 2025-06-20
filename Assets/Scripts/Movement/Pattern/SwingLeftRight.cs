using System;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/LeftRight")]
public class SwingLeftRight : IMovementPattern
{
    [SerializeField] private float speed;
    [SerializeField] private float swingRange;

    [SerializeField] private bool isSwingLeft;
    [SerializeField] private int maxSwingTimes;
    private Vector3 currentPos;
    private Rigidbody2D rb;
    private float currentSpeed;
    private int currentSwingTimes;
    public override bool isFinished => currentSwingTimes==maxSwingTimes;

    public override float offset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Initialize(Transform transform)
    {
        currentSwingTimes=0;    
        currentPos = transform.position;
        isSwingLeft = currentPos.x >= 0 ? true : false;
        rb = transform.GetComponent<Rigidbody2D>();
        currentSpeed = speed * 0.5f;
        
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {

        float distance = Vector3.Distance(currentPos, transform.position);
        //swing
        if (distance <= swingRange)
        {
            float speedRatio = Mathf.Clamp01(distance / swingRange);
            currentSpeed = speedRatio >= 0 && speedRatio >= 0.5f ? speedRatio * speed : currentSpeed;
            Vector3 moveDirection = isSwingLeft ? Vector3.left : Vector3.right;
            rb.linearVelocity = moveDirection * currentSpeed;

        }
        //reset direction
        else
        {   
            currentSwingTimes++;
            isSwingLeft = !isSwingLeft;
            currentPos = transform.position;
            currentSpeed = speed * 0.5f;
            rb.linearVelocity = Vector2.zero;

        }

    }
}
