
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/MaphitePattern")]
public class MaphitePattern : IMovementPattern
{
    public float movementSpeed = 5f;
    public float xBoundary = 30f;
    public float yBoundary = 20f;
    public float holdingTime = 2f; // Time to hold position before moving

    public int numberOfMovement = 4;
    private BehaviorState state;

    public enum BehaviorState
    {
        HoldingPosition,
        Moving,
        ReturningToCenter
    }
    private int count;
    private Vector3 originalPosition;
    private float maxDistance;
    private Rigidbody2D rb;
    private GameObject player;
    private float timer;
    private bool _isFinished = false;
    private bool hasMovedToNewPosition;
    public override bool isFinished => _isFinished;

    public override float offset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Initialize(Transform transform)
    {
        maxDistance = 20f;
        hasMovedToNewPosition = false;
        _isFinished = false;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = transform.GetComponent<Rigidbody2D>();
        float x = transform.position.x;
        float y = transform.position.y;
        count = 0;
        state = (x == xBoundary || x == -xBoundary) || (y == yBoundary || y == -yBoundary) ? BehaviorState.HoldingPosition : BehaviorState.Moving;
        timer = 0f;
        if (player != null) RotateTowardPlayer(transform);
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        switch (state)
        {
            case BehaviorState.HoldingPosition:
   
                timer += deltaTime;
                hasMovedToNewPosition = false;
                if (timer >= holdingTime)
                {
                 
                    RotateTowardPlayer(transform);
                    maxDistance=CalculateMaxDistance(transform);
                    state = BehaviorState.Moving;
                    timer = 0f;

                }
                break;
            case BehaviorState.Moving:
                MoveStraight(transform, originalPosition);
                break;
            case BehaviorState.ReturningToCenter:
                ReturnToCenterScreen(transform);
      
                break;
        }
    }

    private void RotateTowardPlayer(Transform transform)
    {
        if (player != null)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            float angle = Vector2.SignedAngle(transform.up, dir);

            //rotate towards the player instatly
            rb.rotation += angle;



        }
    }
    private void MoveStraight(Transform transform, Vector3 originalPos)
    {
    


        float distanceTravelled = Vector3.Distance(originalPos, transform.position);
        float currentSpeed = Mathf.Lerp(movementSpeed / 2, movementSpeed, distanceTravelled / maxDistance);
        rb.linearVelocity = transform.up * currentSpeed;
        Debug.Log("Speed:" + rb.linearVelocity.magnitude);
        float x = transform.position.x;
        float y = transform.position.y;

        if (!hasMovedToNewPosition && (x >= xBoundary || x <= -xBoundary || y >= yBoundary || y <= -yBoundary) && distanceTravelled > 10f)
        {
  
            rb.linearVelocity = Vector2.zero;
            state = BehaviorState.HoldingPosition;
            if (count < numberOfMovement)
            {
                MoveToRandomPosition(transform);
                state = BehaviorState.HoldingPosition;
            }
            else
            {
                state = BehaviorState.ReturningToCenter;
                rb.position = new Vector2(0, 20f);
                rb.rotation = 180f;

            }
            timer = 0f;
        }
    }
    private void MoveToRandomPosition(Transform transform)
    {
        int num = Random.Range(0, 3);
        float x, y;
        if (num == 0)
        {
            x = xBoundary;
            y = Random.Range(-yBoundary, yBoundary);
        }
        else if (num == 1)
        {
            x = -xBoundary;
            y = Random.Range(-yBoundary, yBoundary);
        }
        else if (num == 2)
        {
            y = yBoundary;
            x = Random.Range(-xBoundary, xBoundary);


        }
        else
        {
            y = -yBoundary;
            x = Random.Range(-xBoundary, xBoundary);
        }
      
        Vector2 randomPos = new Vector2(x, y);
        hasMovedToNewPosition = true;
        rb.position = randomPos;
        originalPosition = randomPos;
        count++;
    }
    private void ReturnToCenterScreen(Transform transform)
    {

        rb.linearVelocity = transform.up * movementSpeed;
        if (rb.position.y >= 0)
        {
            _isFinished = true;
            return;
        }

    }
    private float CalculateMaxDistance(Transform transform)
    {
        float angleBetweenRotaionAndXAxis = Mathf.Abs(Vector2.SignedAngle(transform.up, Vector2.right));
       
        if (angleBetweenRotaionAndXAxis > 90f)
        {
            angleBetweenRotaionAndXAxis = 180f - angleBetweenRotaionAndXAxis;
        }
        
        float distanceToMove = xBoundary / Mathf.Sin(Mathf.Deg2Rad*angleBetweenRotaionAndXAxis);
        Debug.Log("distance:"+distanceToMove);
        return distanceToMove;
    }
}
