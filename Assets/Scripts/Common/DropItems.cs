using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour
{

    [System.Serializable]
    public class Item
    {
        public GameObject item;
        [Range(0f, 100f)] public float rate;
    }
    public List<Item> items;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DropItem()
    {
        float number = Random.Range(0, 100);
        float culmulative = 0;
        foreach (Item item in items)
        {
            if (number >= culmulative && number <= culmulative + item.rate)
            {

                Instantiate(item.item, transform.position, Quaternion.identity);
                culmulative += item.rate;
            }


        }
    }
}
