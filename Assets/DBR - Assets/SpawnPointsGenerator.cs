using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsGenerator : MonoBehaviour
{
    public int numberOfSpawnPoints;
    public GameObject spawnPointPrefab;
    public Transform pointsParent;
    public float xPositionMin;
    public float xPositionMax;
    public float yPositionMin;
    public float yPositionMax;

    public List<GameObject> spawnPointsList;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GenerateSpawnPoints()
    {
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            float randomX = Random.Range(xPositionMin, xPositionMax + 0.001f);
            float randomY = Random.Range(yPositionMin, yPositionMax + 0.001f);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

            GameObject spawnPoint = Instantiate(spawnPointPrefab, spawnPosition, Quaternion.identity);
            spawnPoint.transform.parent = pointsParent;
            spawnPoint.name = "Spawn Point " + i;
            
            spawnPointsList.Add(spawnPoint);
        }
    }

    public void DeleteSpawnPoints()
    {
        spawnPointsList.Clear();

        foreach (Transform child in pointsParent)
        {
            Destroy(child.gameObject);

        }
    }
}
