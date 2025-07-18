using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/Straight")]
public class StraightPattern : IMovementPattern
{
    public float speed = 7f;
    public float duration = 3f;
    public bool moveDown=true;
    public bool hasStartPos=false;
    private float _offset;
    public Vector3 startPos;
    private float time;
    private Rigidbody2D rb;
    
  
    public override bool isFinished => time>=duration;

    public override float offset { get => _offset; set => _offset=value; }

    public override void Initialize(Transform transform)
    {
        time = 0;
        rb =transform.GetComponent<Rigidbody2D>();
        if(hasStartPos) transform.position = startPos+new Vector3(0,offset,0);
    }

    public override void UpdateMovement(Transform transform, float deltaTime)

    {
        time+= deltaTime;

        Vector2 moveDir = moveDown ? -transform.up : transform.up;
        rb.MovePosition(rb.position + deltaTime * speed * moveDir);
        if (time >= duration) return;
    }

}
