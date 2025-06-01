using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/MovementPattern/TopDowmSwingPattern")]
public class TopSwingPattern : IMovementPattern
{
    public float duration;
    private float time;
    private float timeScale = 0.01f;
    public float startX;
    public float xRange;
    public float _offset;
    private float x;
    private float y;
    public override bool isFinished => time >= duration;

    public override float offset { get => _offset; set => _offset=value; }

    public override void Initialize(Transform transform)
    {
      
        x = startX;
        y = 1 / x ;
        transform.position=new Vector3(x, y, 0);
    }

    public override void UpdateMovement(Transform transform, float deltaTime)
    {
        float t = time / (duration);
        if (!isFinished)
        {
            timeScale = math.lerp(0.09f, duration * 2, t);
            time += deltaTime*timeScale ;
            x = startX >= 0 ? math.lerp(startX, xRange-offset, t) : math.lerp(startX, -xRange+offset, t);
            y = startX >= 0 ? 1 / x:(-1/x);
            transform.position = new Vector3(x, y, 0);
       

        }


    }


}
