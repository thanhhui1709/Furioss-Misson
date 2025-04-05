using UnityEngine;

public class EnemyController : EnemyBase
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private bool isSpawning;

    
    public float verticalRange;
    public float horizontalRange;
    private bool hasSpawnedFirstEgg = false;

    public float horizontalMoveDuration = 2f; 
    public float horizontalSpeed = 2f; 
    private bool movingRight; 
    private float horizontalTimer = 0f; 

    void Start()
    {
        startPos = transform.position;
        Invoke("TrySpawnEgg", 0.5f);
        movingRight=startPos.x>0? false: true;
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(startPos, transform.position);
        float maxRange = Mathf.Sqrt(verticalRange * verticalRange + horizontalRange * horizontalRange);

        if (distance < maxRange)
        {
            MovingVerticalThenHorizontal();
            isSpawning = true;
        }
        else
        {
            isSpawning = false;
            MovingHorizontal();
        }
    }

    private void MovingVerticalThenHorizontal()
    {
        float verticalDistance = Mathf.Abs(transform.position.y - startPos.y);
        if (verticalDistance <= verticalRange)
        {
            transform.Translate((startPos.y <= 0 ? Vector3.up : Vector3.down) * speed * Time.deltaTime);
        }
        else
        {
            // Use movingRight to keep direction consistent with MovingHorizontal
            Vector3 moveDirection = startPos.x<0 ? Vector3.right : Vector3.left;
            transform.Translate(moveDirection * speed * Time.deltaTime);
        }
    }
    private void MovingHorizontal()
    {
        horizontalTimer += Time.deltaTime;

        if (horizontalTimer >= horizontalMoveDuration)
        {
            movingRight = !movingRight;
            horizontalTimer = 0f;
        }
        Vector3 moveDirection = movingRight ? Vector3.right : Vector3.left;
        transform.Translate(moveDirection * horizontalSpeed * Time.deltaTime);
    }

    private void TrySpawnEgg()
    {
        float nextSpawnTime = hasSpawnedFirstEgg ? Random.Range(7f, 12f) : Random.Range(4f, 10f);
        if (!isSpawning)
        {
            if (!hasSpawnedFirstEgg)
            {
                SpawnEgg();
                hasSpawnedFirstEgg = true;    
            }
            else
            {
                SpawnEgg();
            }
        }
        
        
            Invoke("TrySpawnEgg", nextSpawnTime);
        
       
    }

    protected void SpawnEgg()
    {
        Instantiate(chickenEgg, transform.position, Quaternion.identity);
    }
}
