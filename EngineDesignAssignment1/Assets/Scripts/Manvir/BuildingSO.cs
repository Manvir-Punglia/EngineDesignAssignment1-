using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/BuildingData", order = 1)]
public class BuildingSO : ScriptableObject
{
    [field:SerializeField] public string BuildingName { get; private set; }
    [field:SerializeField] public int Health { get; private set; }
    [field:SerializeField] public int Cost { get; private set; }
    [field:SerializeField] public Material Material { get; private set; }
}