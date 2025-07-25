using UnityEngine;

public class Indicator : MonoBehaviour
{
    public float existTime = 2f;
    private float timer;
    void Start()
    {
        timer = 0;
    }
    private void OnEnable()
    {
        timer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > existTime)
        {
           ObjectPoolManager.ReturnObject(gameObject);
        }
    }
}
