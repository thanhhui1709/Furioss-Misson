using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/Curved")]
public class CurvedPattern : IMovementPattern 
{
    public float startY; 
    public float endY; 
    public float speed = 2f;
    public bool isPositiveSide; 

    private float _offset;
    private float _currentY;
    private Rigidbody2D _rb;
    private bool _isFinished;

   
    public override bool isFinished => _isFinished;
    public override float offset { get => _offset; set => _offset = value; }

    public override void Initialize(Transform transform)
    {
        _currentY = startY;
        _isFinished = false;

     
        _rb = transform.GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogWarning("Rigidbody2D not found on enemy.", transform.gameObject);
            return;
        }
        float x = isPositiveSide ? (_currentY * _currentY) / 2 + 1f : -(_currentY * _currentY) / 2 + 1f;

        // Adjust y based on isPositiveSide
        float y = _currentY;
        transform.position = new Vector2(x, y);

        UpdatePosition(transform);
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        if (_rb == null || _isFinished)
        {
            if (_rb != null) _rb.linearVelocity = Vector2.zero;
            return;
        }

        // Determine direction of movement (toward endY)
        float direction = startY < endY ? 1f : -1f;

        // Update y position based on speed
        _currentY += direction * speed * Time.fixedDeltaTime;

        // Check if movement is finished
        if ((direction > 0 && _currentY >= endY-offset) || (direction < 0 && _currentY <= endY+offset))
        {
             // Clamp to endpoint
            _isFinished = true;
        }

        // Update position based on current y
        UpdatePosition(transform);
    }

    private void UpdatePosition(Transform transform)
    {
      

        // Calculate x based on y using x = y^2 + 1 + offset
        float x = isPositiveSide ? (_currentY * _currentY)/2 + 1f: -(_currentY * _currentY) / 2 + 1f;

        // Adjust y based on isPositiveSide
        float y =  _currentY ;

        // Set position using Rigidbody2D
        Vector3 targetPosition = new Vector3(x, y, transform.position.z);
        _rb.MovePosition(targetPosition);
        Debug.Log(transform.position);
    }
}