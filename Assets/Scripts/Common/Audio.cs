using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource src;
    public AudioClip clip;
    void Start()
    {
        src.clip = clip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { 
        
        src.Play();
        }
    }
}
