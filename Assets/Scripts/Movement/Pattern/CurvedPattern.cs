using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/Curved")]
public class CurvedPattern : MovementPattern
{
    public float duration = 3f;
    public float amplitude = 2f;
    public float startY;
    public float endY;

    private float time;
    private float x;
    private float y;

    public override bool isFinished => time >= duration;

    public override void Initialize(Transform transform)
    {
        y = startY;
        x = y * y + 1;
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        time += deltaTime;
        float t = time / (duration-offset);

        if (isFinished) {

            y = math.lerp(startY, endY, t);
            x = y * y + 1;

            transform.position = new Vector3(x, y, 0);
        }
    }
}
