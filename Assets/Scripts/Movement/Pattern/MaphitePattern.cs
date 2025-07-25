using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/MaphitePattern")]
public class MaphitePattern : IMovementPattern
{
    // === Configuration ===
    public int damage = 80;
    public float movementSpeed = 35f;
    public float xBoundary = 30f;
    public float yBoundary = 20f;
    public float holdingTime = 2.5f;
    public int numberOfMovement = 4;
    public GameObject skillIndicator;
    public bool hasDealDamage = false;
    public AudioClip clip;

    // === Runtime State ===
    private BehaviorState state;
    private bool hasMovedToNewPosition = false;
    private bool hasRotated = false;
    private bool _isFinished = false;
    private int count = 0;
    private float timer = 0f;
    private float currentSpeed;
    private float maxDistance = 20f;
    private Vector3 originalPosition;
    private GameObject indicatorInstance;

    // === Components ===
    private Rigidbody2D rb;
    private GameObject player;

    public enum BehaviorState
    {
        HoldingPosition,
        Moving,
        ReturningToCenter
    }

    public override bool isFinished => _isFinished;
    public override float offset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // === Initialize Pattern ===
    public override void Initialize(Transform transform)
    {
        rb = transform.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        state = IsOutOfBounds(transform.position) ? BehaviorState.HoldingPosition : BehaviorState.Moving;
        currentSpeed = movementSpeed / 2;
        count = 0;
        timer = 0f;
        _isFinished = false;
        hasDealDamage = false;
        hasRotated = false;
        hasMovedToNewPosition = false;

        if (player != null)
        {
            RotateTowardPlayer(transform);
        }
    }

    // === Update Loop ===
    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        switch (state)
        {
            case BehaviorState.HoldingPosition:
                HandleHolding(transform, deltaTime);
                break;

            case BehaviorState.Moving:
                HandleMoving(transform);
                break;

            case BehaviorState.ReturningToCenter:
                HandleReturn(transform, deltaTime);
                break;
        }
    }

    // === Movement Phases ===

    private void HandleHolding(Transform transform, float deltaTime)
    {
        timer += deltaTime;
        hasMovedToNewPosition = false;

        if (timer > holdingTime)
        {
            state = BehaviorState.Moving;
            ObjectPoolManager.PlayAudio(clip, 1f);
            timer = 0f;
            currentSpeed = movementSpeed / 2;
        }
        else if (timer > 0.5f && !hasRotated)
        {
            RotateTowardPlayer(transform);
            Debug.Log("Rotating towards player " + count);
            hasRotated = true;

            indicatorInstance = ObjectPoolManager.SpawnObject(
                skillIndicator,
                transform.position + transform.up * 5,
                transform.rotation
            );
            indicatorInstance.transform.SetParent(transform);
            indicatorInstance.transform.rotation = transform.rotation;
            indicatorInstance.SetActive(true);
        }
    }

    private void HandleMoving(Transform transform)
    {
        float distanceTravelled = Vector3.Distance(originalPosition, transform.position);
        currentSpeed = Mathf.MoveTowards(currentSpeed, movementSpeed, 10 * Time.fixedDeltaTime);
        rb.linearVelocity = transform.up * currentSpeed;

        Vector2 pos = transform.position;

        if (!hasMovedToNewPosition && IsOutOfBounds(pos) && distanceTravelled > 10f)
        {
            rb.linearVelocity = Vector2.zero;

            if (count < numberOfMovement)
            {
                MoveToRandomPosition(transform);
                state = BehaviorState.HoldingPosition;
                timer = 0f;
                hasDealDamage = false;
                hasRotated = false;
            }
            else
            {
                rb.position = new Vector2(0f, 20f);
                rb.rotation = 180f;
                state = BehaviorState.ReturningToCenter;
                timer = 0f;
            }
        }
    }

    private void HandleReturn(Transform transform, float deltaTime)
    {
        timer += deltaTime;
        rb.linearVelocity = transform.up * movementSpeed;

        if (rb.position.y <= 5f)
        {
            rb.linearVelocity = Vector2.zero;
            _isFinished = true;
        }
    }

    // === Helper Methods ===

    private void RotateTowardPlayer(Transform transform)
    {
        if (player == null) return;

        Vector2 dir = (player.transform.position - transform.position).normalized;
        float angle = Vector2.SignedAngle(transform.up, dir);
        rb.rotation += angle;
    }

    private void MoveToRandomPosition(Transform transform)
    {
        float x, y;
        int num = Random.Range(0, 4);

        switch (num)
        {
            case 0:
                x = xBoundary;
                y = Random.Range(-yBoundary, yBoundary);
                break;
            case 1:
                x = -xBoundary;
                y = Random.Range(-yBoundary, yBoundary);
                break;
            case 2:
                y = yBoundary;
                x = Random.Range(-xBoundary, xBoundary);
                break;
            default:
                y = -yBoundary;
                x = Random.Range(-xBoundary, xBoundary);
                break;
        }

        Vector2 randomPos = new Vector2(x, y);
        rb.position = randomPos;
        originalPosition = randomPos;
        hasMovedToNewPosition = true;
        count++;
    }
    private bool IsOutOfBounds(Vector2 pos)
    {
        return pos.x >= xBoundary || pos.x <= -xBoundary || pos.y >= yBoundary || pos.y <= -yBoundary;
    }
}
