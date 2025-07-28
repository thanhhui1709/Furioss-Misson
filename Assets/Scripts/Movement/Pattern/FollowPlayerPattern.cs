using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/FollowPlayer")]
public class FollowPlayerPattern : IMovementPattern
{
    public float duration = 7f;
    public float speed = 15f;
    public float rotationSpeed = 180f;
    public float xBounds = 24f;
    public float yBounds = 10f;
    public int damage = 50;
    public bool hasDealDamage = false;
    public float damageCooldown = 1f;

    private float currentSpeed;
    private float elapsedTime;
    private Transform player;
    private Rigidbody2D rb;
    private bool _isFinished;
    private float damageCooldownTimer;
    public override bool isFinished => _isFinished;

    public override float offset { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Initialize(Transform transform)
    {
        damageCooldownTimer = 0f;
        hasDealDamage = false;
        _isFinished = false;
        rb = transform.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        elapsedTime = 0f;
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        if (elapsedTime < duration)
        {
            elapsedTime += deltaTime;
            RotateToPlayer(transform, player);
            MoveTowardPlayer(transform, elapsedTime);

        }
        else
        {
            _isFinished = true;
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (hasDealDamage)
        {
            damageCooldownTimer += deltaTime;
            if (damageCooldownTimer >= damageCooldown)
            {
                hasDealDamage = false;
                damageCooldownTimer = 0f;
            }
        }
    }
    private void RotateToPlayer(Transform transform, Transform playerTransform)
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // trừ 90 nếu dùng up làm forward
        float newAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.deltaTime);
        rb.rotation = newAngle;
    }
    private void MoveTowardPlayer(Transform transform, float elapsedTime)
    {
        currentSpeed = Mathf.Lerp(speed / 2, speed, elapsedTime / duration);
        rb.linearVelocity = transform.up * currentSpeed;
        if (Mathf.Abs(transform.position.x) >= xBounds || Mathf.Abs(transform.position.y) >= yBounds)
        {
            rb.position= new Vector2(Mathf.Clamp(transform.position.x, -xBounds, xBounds), Mathf.Clamp(transform.position.y, -yBounds, yBounds));
        }

    }
 
}
