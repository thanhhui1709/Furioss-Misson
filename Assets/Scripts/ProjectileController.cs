using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     transform.Translate(Vector2.up*speed*Time.deltaTime);    
    }
}
