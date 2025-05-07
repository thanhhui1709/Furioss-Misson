using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/Curved")]
public class CurvedPattern : MovementPattern
{
    public float duration = 3f;
    public float amplitude = 2f;
    private float time;
    private float startX;
    public float xRange;

    public override bool isFinished => time >= duration;

    public override void Initialize(Transform transform)
    {
        time = 0f;
        startX=transform.position.x;

    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        time += deltaTime;
        float t = time / duration;
        float x = startX>=0? math.lerp(startX,startX-xRange, t): math.lerp(startX,startX+xRange,t);
        float y = -3 / x + 1;
        transform.position = new Vector3(x, y, 0);
        if (time >= duration)
        {
            return;
        }


    }
}
