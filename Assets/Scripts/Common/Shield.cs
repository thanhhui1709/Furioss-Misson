using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float duration;
    private GameObject indicator;
    [SerializeField] private float moveSpeed;
    private GameObject player;
    void Start()
    {

        player = GameObject.FindWithTag("Player");
        Transform indicatorTransform = player.transform.Find("ShieldIndicator");
        indicator = indicatorTransform.gameObject;
       

    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            health.ApplyShieldEffect(indicator,duration);
            Destroy(gameObject);


        }
    }
    private void FindPlayer()
    {
        if (player == null) return;
        float playerRotation = player.transform.rotation.z;
        if (playerRotation == 0)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
        else if (playerRotation > 0 && playerRotation <= 90)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else if (playerRotation < 0 && playerRotation >= -90)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else if (playerRotation == 180)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }

}
