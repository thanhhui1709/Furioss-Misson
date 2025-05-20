using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/TopDowmSwingPattern")]
public class TopSwingPattern : MovementPattern
{
    public float duration;
    private float time;
    private float timeScale = 0.01f;
    public float startX;
    public float xRange;

    private float x;
    private float y;
    public override bool isFinished => time >= duration;

    public override void Initialize(Transform transform)
    {
        time = 0;
        x = startX;
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        float t = time / (duration-offset);
        if (time <= duration)
        {
            timeScale = math.lerp(0.06f, duration*2, t);
            time += deltaTime * timeScale;
            x = startX >= 0 ? math.lerp(startX, xRange, t) : math.lerp(startX, -xRange, t);
            y = startX >= 0 ? 1 / x : -1 / x;
            transform.position = new Vector3(x, y, 0);

        }


    }


}
