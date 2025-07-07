using UnityEngine;

public class Test : MonoBehaviour
{
    public ParticleSystem particleSystem;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
            else
            {
                Debug.LogWarning("Particle System is not assigned.");
            }
        }
    }
}
