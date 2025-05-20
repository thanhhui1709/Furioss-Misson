using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/Curved")]
public class CurvedPattern : MovementPattern
{
    public float duration = 3f;
    public float amplitude = 2f;
    private float time;
    private float startY;
    private float x;
    private float y;
    public float endY;
    public override bool isFinished => time >= duration;

    public override void Initialize(Transform transform)
    {
        time = 0f;
        y=startY;

    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        time += deltaTime;
        float t = time / duration;
        
        y=math.lerp(y, endY, t);
        x = y * y + 1;
        transform.position = new Vector3(x, y, 0);
        if (time >= duration)
        {
            return;
        }


    }
}
