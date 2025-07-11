using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject testGo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Spawn the test GameObject at the current position with no rotation
            ObjectPoolManager.SpawnObject(testGo, transform.position, Quaternion.identity);
        }
    }
}
