using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSpawner : MonoBehaviour
{
    public GameObject[] buildingPrefabs;  
    public Transform[] spawnPoints;
    IFactory fac;

    void Awake()
    {     
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            
            int rand = Random.Range(0, buildingPrefabs.Length);

           
            IFactory fac = new TownFactory(buildingPrefabs[rand]);

            GameObject building = fac.CreateProduct();

          
            building.transform.position = spawnPoints[i].position;
            building.transform.rotation = spawnPoints[i].rotation;
        }
    }
}

