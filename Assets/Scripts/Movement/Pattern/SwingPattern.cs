using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/SwingPattern")]
public class SwingPattern : IMovementPattern
{
    public float SwingRange;
    public float Speed;
    public float _offset;
    public Vector3 StartPos;


    private float distance;
    private float currentSpeed;
    private Rigidbody2D Rb;
    private bool _isFinished;
    public override bool isFinished => _isFinished;

    public override float offset { get => _offset; set => _offset=value; }

    public override void Initialize(Transform transform)
    {
        transform.position = StartPos;
        Rb=transform.GetComponent<Rigidbody2D>();
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        

        distance = Vector3.Distance(transform.position, StartPos);

        if (distance >= (SwingRange - offset))
        {
            Rb.linearVelocity = Vector2.zero;
          
            Quaternion targetRotation = Quaternion.Euler(0, 0, 180);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,5f);
            if (transform.rotation==targetRotation)
            {
                _isFinished = true;
                return;
            }

        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed + 0.1f, Speed, distance / (SwingRange - offset));


            Vector3 direction = StartPos.x > 0 ? Vector2.left : Vector2.right;

            // Rotate towards move direction
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction); // z = forward in 2D
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * deltaTime);
            }

            Rb.linearVelocity = direction * currentSpeed;
        }


      
        
           
        
    }


}
