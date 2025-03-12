using GLTFast.Schema;
using UnityEngine;

public class EnemyController : EnemyBase
{
    [SerializeField] private Vector3 startPos;
    public float verticalRange;
    public float horizontalRange;

    void Start()
    {
        
        InvokeRepeating(nameof(SpawnEgg), 3f, 3f);
        startPos = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(startPos, transform.position) < Mathf.Sqrt(verticalRange * verticalRange + horizontalRange * horizontalRange))
        {
            MovingVerticalThenHorizontal();
        }
    }

    private void MovingVerticalThenHorizontal()
    {
        if (Vector3.Distance(startPos, transform.position) < verticalRange)
        {
            if (startPos.y <= 0)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        }
        else
        {
            if (startPos.x <= 0)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
    }
}
