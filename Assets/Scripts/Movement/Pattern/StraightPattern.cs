using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/Straight")]
public class StraightPattern : MovementPattern
{
    public float speed = 7f;
    public float duration = 3f;
    private float time;
   
    public override bool isFinished => time>=duration;

    public override void Initialize(Transform transform)
    {
        time = 0;
     
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        time+= deltaTime;
        transform.Translate(transform.up * speed * Time.deltaTime,Space.World);
        if (time >= duration) return;
    }

}
