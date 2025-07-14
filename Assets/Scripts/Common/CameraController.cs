using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    public Camera Camera;
    public float increaseTime;
    public int initSize;
    public int maxSize;
    public Transform player;
    [Header("Shake Settings")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.3f;

    void Awake()
    {
        GameEvent.instance.onStageStart.AddListener(() =>
        {
            StartCoroutine(IncreaseCameraSize(maxSize, increaseTime));
        });
        Camera.orthographicSize = initSize; // Initialize camera size



    }
    private void Update()
    {
      
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
    public void Shake(float duration, float magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    public void Shake() // overload mặc định
    {
        Shake(shakeDuration, shakeMagnitude);
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 initialPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}
