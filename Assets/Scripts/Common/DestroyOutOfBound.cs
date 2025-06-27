using UnityEngine;

public class DestroyOutOfBound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float horizontalBound;
    [SerializeField] private float verticalBound;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if(pos.y >= verticalBound || pos.y < -verticalBound || pos.x >= horizontalBound || pos.x < -horizontalBound)
        {
            Destroy(gameObject);
        }
    }
}
