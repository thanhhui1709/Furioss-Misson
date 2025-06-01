using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/TopDown")]
public class TopDownSpawn : IMovementPattern
{
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float yRange = 5f;
    private float distanceTravelled;
    private float _offset;
    private Rigidbody2D rb;
    public override bool isFinished => distanceTravelled >= yRange;
    public override float offset { get => _offset; set => _offset = value; }

    private Vector3 startPos;

    public override void Initialize(Transform transform)
    {
        spawnPos.x += offset;
        startPos = spawnPos;
        transform.position = spawnPos;
        rb = transform.GetComponent<Rigidbody2D>();
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
       
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody2D attached to enemy!");
            return;
        }

        distanceTravelled = Vector3.Distance(transform.position, startPos);

        if (!isFinished)
        {
            float remaining = Mathf.Clamp01((yRange - distanceTravelled+0.5f) / yRange);
            float currentSpeed = speed * remaining; // Gradually reduce speed
            rb.linearVelocity = Vector2.down * currentSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
