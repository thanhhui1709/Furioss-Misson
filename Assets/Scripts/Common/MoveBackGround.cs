using UnityEngine;

public class MoveBackGround : MonoBehaviour
{
    public float scrollSpeed = 2f;
    public float backgroundHeight = 20f;
    private Vector3 initialPos;

    

    void Start()
    {
      initialPos = transform.position;
                
    }

    void FixedUpdate()
    {
       float distance=Vector3.Distance(initialPos, transform.position);
       transform.Translate(Vector3.down * scrollSpeed*Time.fixedDeltaTime,Space.World); 
        if (distance >= backgroundHeight)
        {
         
            transform.position = initialPos;
           

        }
    }

   
}
