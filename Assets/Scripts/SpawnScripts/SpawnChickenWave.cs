using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpawnChickenWave : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float timePerChicken;
    [SerializeField] private float horizontalOffset;
    [SerializeField] private float verticalOffset;
    [SerializeField] private GameObject chicken;
    private EnemyController enemyController;
    [SerializeField] private int numberOfCol;
    [SerializeField] private int numberOfRow;
    void Start()
    {
        enemyController=chicken.GetComponent<EnemyController>();
        StartCoroutine("SpawnChicken");
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnChicken()
    {
        float originalHorizontalRange = enemyController.horizontalRange;
        float originalVerticalRange = enemyController.verticalRange;
        for (int i = 0; i < numberOfRow; i++)
        {
            for (int j = 0; j < numberOfCol; j++)
            {
                yield return SpawnChickenAfterTime();
                enemyController.horizontalRange -= horizontalOffset;
                Debug.Log(enemyController.horizontalRange);
            }
            enemyController.horizontalRange = originalHorizontalRange;
            Debug.Log(enemyController.horizontalRange);
            enemyController.verticalRange -= verticalOffset;
        }
        enemyController.verticalRange = originalVerticalRange;

    }
    IEnumerator SpawnChickenAfterTime()
    {
        yield return new WaitForSeconds(timePerChicken);
        Instantiate(chicken, transform.position, Quaternion.identity);
    }
}
