using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownFactory : IFactory
{
    private GameObject _buildingPrefab;

    public TownFactory(GameObject buildingPrefab)
    {
        _buildingPrefab = buildingPrefab;
    }

    public GameObject CreateProduct()
    {
        return GameObject.Instantiate(_buildingPrefab);
    }
}
