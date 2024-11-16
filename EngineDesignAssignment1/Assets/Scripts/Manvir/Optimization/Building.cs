using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingSO buildingData;

    private int currentHealth;

    void Start()
    {
      
        currentHealth = buildingData.Health;
        ApplyMaterial();
    }

    void ApplyMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && buildingData.Material != null)
        {
            renderer.material = buildingData.Material;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DestroyBuilding();
        }
    }

    void DestroyBuilding()
    {
        Debug.Log(buildingData.BuildingName);
        Debug.Log("Destroyed");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }
            
    }
}