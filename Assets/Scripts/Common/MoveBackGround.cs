using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Mathematics;

public class MoveBackGround : MonoBehaviour
{
    public float scrollSpeed = 2f;
    public float backgroundHeight = 20f;
    private Vector3 initialPos;

    public float increaseSpeedTime;
    public AudioClip flySound;
    

    void Awake()
    {
        GameEvent.instance.onStageStart.AddListener(() =>
        {
            StartCoroutine(IncreaseBackgroundSpeed(increaseSpeedTime));
        });
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
    IEnumerator IncreaseBackgroundSpeed(float increaseTime)
    {
        ObjectPoolManager.PlayAudio(flySound, 0.8f);
        float elapsedTime = 0f;
        float originalSpeed = scrollSpeed;
        float boostedSpeed = originalSpeed * 20f;

       
        scrollSpeed = boostedSpeed;

        while (elapsedTime < increaseTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / increaseTime;
            scrollSpeed = Mathf.Lerp(boostedSpeed, originalSpeed, t);
            yield return null;
        }

        scrollSpeed = originalSpeed;
    }


}
