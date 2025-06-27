using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    public GameObject heartPrefab;         // Prefab of heart
    public Transform heartContainer;       // UI parent (LivesPanel)
    private int maxLives;

    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        maxLives = GameManager.instance.numberOfLive;
        CreateHearts(maxLives); // Initial lives
    }

    public void CreateHearts(int lives)
    {
        ClearHearts();

        for (int i = 0; i < lives; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart);
        }
    }

    public void RemoveHeart()
    {
        if (hearts.Count > 0)
        {
            GameObject lastHeart = hearts[hearts.Count - 1];
            hearts.Remove(lastHeart);
            Destroy(lastHeart);
        }
    }

    private void ClearHearts()
    {
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();
    }

}
