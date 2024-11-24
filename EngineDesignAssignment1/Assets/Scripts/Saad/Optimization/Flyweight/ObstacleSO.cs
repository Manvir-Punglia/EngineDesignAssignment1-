using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData", order = 2)]

public class ObstacleSO : ScriptableObject
{
    [field: Header("Obstacle Properties")]
    [field: SerializeField] public string ObstacleName { get; private set; }
    [field: SerializeField] public float ObstacleImpactForce { get; private set; }

}
