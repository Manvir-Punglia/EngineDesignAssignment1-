using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownFactory : IBuildingFactory
{
    private GameObject _buildingPrefab;

    public TownFactory(GameObject buildingPrefab)
    {
        _buildingPrefab = buildingPrefab;
    }

    public GameObject CreateBuilding()
    {
        return GameObject.Instantiate(_buildingPrefab);
    }
}
