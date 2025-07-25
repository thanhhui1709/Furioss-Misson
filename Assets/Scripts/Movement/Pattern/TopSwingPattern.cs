using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/TopDownSwingPattern")]
public class TopSwingPattern : IMovementPattern
{
    [SerializeField] private float initialSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;

    [Header("X Curve Movement")]
    public float startX = -5f;
    public float xRange = 10f;
    [SerializeField] private float _offset;

    //run time variable
    private Rigidbody2D rb;
    private Vector2 previousPos;
    private float currentSpeed;
    private float currentX;
    private float targetX;
    private bool goingRight;

    public override float offset { get => _offset; set => _offset = value; }
    public override bool isFinished  => checkFinish();

    public override void Initialize(Transform transform)
    {
        rb = transform.GetComponent<Rigidbody2D>();
        transform.rotation =Quaternion.Euler(0,0 ,180);
        goingRight = startX >= 0;

        currentX = startX;
        targetX = goingRight ? xRange + offset : -xRange - offset;

        float y = CurveFunction(currentX);
        Vector2 startPos = new Vector2(currentX, y);
        transform.position = startPos;
        previousPos = startPos;

        currentSpeed = initialSpeed;
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        if (isFinished || rb == null)
            return;

        // Move X towards target
        currentX = Mathf.MoveTowards(currentX, targetX, currentSpeed * deltaTime);
        float y = CurveFunction(currentX);
        Vector2 newPos = new Vector2(currentX, y);

        Vector2 direction = (newPos - previousPos).normalized;
        Vector2 nextPos = rb.position + direction * currentSpeed * deltaTime;

        rb.MovePosition(nextPos);
        previousPos = newPos;

        // Gradually increase speed
        currentSpeed = math.min(currentSpeed + deltaTime, maxSpeed);


        rb.linearVelocity = Vector2.zero;
        
    }

    private float CurveFunction(float x)
    {
        if (math.abs(x) < 0.01f)
            x = 0.01f;

        return goingRight ? 1f / x : -1f / x;
    }
    private bool checkFinish()
    {
        return goingRight && currentX >= targetX || !goingRight && currentX <= targetX;
    }
}
