using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : IFactory
{
    private GameObject _obstaclePrefab;

    public ObstacleFactory(GameObject obstaclePrefab)
    {
        _obstaclePrefab = obstaclePrefab;
    }

    public GameObject CreateProduct()
    {
        return GameObject.Instantiate(_obstaclePrefab);
    }
}
