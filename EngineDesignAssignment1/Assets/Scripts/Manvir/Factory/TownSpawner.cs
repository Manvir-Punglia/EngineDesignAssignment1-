using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSpawner : MonoBehaviour
{
    public GameObject[] buildingPrefabs;  
    public Transform[] spawnPoints;       

    void Awake()
    {     
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            
            int rand = Random.Range(0, buildingPrefabs.Length);

           
            IBuildingFactory fac = new TownFactory(buildingPrefabs[rand]);

            GameObject building = fac.CreateBuilding();

          
            building.transform.position = spawnPoints[i].position;
            building.transform.rotation = spawnPoints[i].rotation;
        }
    }
}

