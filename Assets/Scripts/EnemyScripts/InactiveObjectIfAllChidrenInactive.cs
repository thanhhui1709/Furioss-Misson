using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InactiveObjectIfAllChidrenInactive : MonoBehaviour
{
    private List<Transform> children = new List<Transform>();

    void Start()
    {
        //get all children
        foreach (Transform child in transform)
        {
            children.Add(child);
        }



    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!CheckChildrenActive())
        {
           
            ObjectPoolManager.ReturnObject(gameObject);
        }
    }
    private bool CheckChildrenActive()
    {
        int count = 0;
        foreach (Transform child in children)
        {
            if (child.gameObject.activeSelf)
            {
                count++;
            }
        }
        return count > 0;
    }
}
