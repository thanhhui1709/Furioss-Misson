using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/LeftRight")]
public class SwingLeftRight : IMovementPattern
{
    [SerializeField] private float speed;
    [SerializeField] private float swingRange;
    [SerializeField] private float swingCooldown;

    [SerializeField] private bool isSwingLeft;
    [SerializeField] private int maxSwingTimes;
    private Vector3 currentPos;
    private Rigidbody2D rb;
    private float currentSpeed;
    private int currentSwingTimes;
    private float swingCooldownTimer;
    private State currentState;

    private enum State
    {
        Moving,
        Waiting,
    }
    public override bool isFinished => currentSwingTimes == maxSwingTimes;

    public override float offset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Initialize(Transform transform)
    {
        currentSwingTimes = 0;
        currentPos = transform.position;
        isSwingLeft = currentPos.x >= 0 ? true : false;
        rb = transform.GetComponent<Rigidbody2D>();
        currentSpeed = speed * 0.5f;
        swingCooldownTimer = 0;
        currentState = State.Moving;
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        float distance;
     

        switch (currentState)
        {
            case State.Moving:
                
                Move(transform, out distance);

                if (distance >= swingRange)
                {
                    currentState = State.Waiting;
                    swingCooldownTimer = swingCooldown;
                    rb.linearVelocity = Vector2.zero; 
                    currentSwingTimes++;
              
                }
                break;

            case State.Waiting:
                swingCooldownTimer -= deltaTime;

                if (swingCooldownTimer <= 0)
                {
                  
                    //reset direction
                    isSwingLeft = !isSwingLeft;
                    currentPos = transform.position;
                    currentSpeed = speed * 0.5f;
               
                    currentState = State.Moving;
                }
                break;
        }
    }

 
    private void Move(Transform transform, out float distance)
    {
        distance = Vector3.Distance(currentPos, transform.position);

        // Optional: adjust speed smoothly based on distance
        float speedRatio = Mathf.Clamp01(distance / swingRange);
        currentSpeed = Mathf.Lerp(speed * 0.5f, speed, speedRatio);

        Vector3 moveDirection = isSwingLeft ? Vector3.left : Vector3.right;
        rb.linearVelocity = moveDirection * currentSpeed;
    }

}
