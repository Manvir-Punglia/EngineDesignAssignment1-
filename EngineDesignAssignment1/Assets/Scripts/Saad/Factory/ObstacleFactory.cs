using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : IObstacleFactory
{
    private GameObject _obstaclePrefab;

    public ObstacleFactory(GameObject obstaclePrefab)
    {
        _obstaclePrefab = obstaclePrefab;
    }

    public GameObject CreateObstacle()
    {
        return GameObject.Instantiate(_obstaclePrefab);
    }
}
