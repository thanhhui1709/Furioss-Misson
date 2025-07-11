using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public Camera Camera;
    public float increaseTime;
    public int initSize;
    public int maxSize;
    public Transform player;

    void Awake()
    {
        GameEvent.instance.onStageStart.AddListener(() =>
        {
            StartCoroutine(IncreaseCameraSize(maxSize, increaseTime));
        });
        Camera.orthographicSize = initSize; // Initialize camera size



    }

 
    IEnumerator IncreaseCameraSize(float size, float increaseTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < increaseTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / increaseTime;
            Camera.orthographicSize = Mathf.Lerp(initSize, maxSize, t);
          
            yield return null;
        }

    }
}
