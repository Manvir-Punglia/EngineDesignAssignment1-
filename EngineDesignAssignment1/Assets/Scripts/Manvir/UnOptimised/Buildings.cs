using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour
{

    public string BuildingName;
    public int Health;
    public int Cost;
    public Material Material;

    private int currentHealth;

    void Start()
    {
      
        currentHealth = Health;
        ApplyMaterial();
    }

    void ApplyMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && Material != null)
        {
            renderer.material = Material;
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
        Debug.Log(BuildingName);
        Debug.Log("Destroyed");
        Destroy(transform.parent.gameObject);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }
            
    }
}
