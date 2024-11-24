using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private ObstacleSO obstacleData;

    public void ImpactObstacle(Car car)
    {
        
        if (car != null)
        {
            car.ReduceSpeed(obstacleData.ObstacleImpactForce);
        }

        Debug.Log(obstacleData.ObstacleName);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Car car = other.GetComponent<Car>();
            ImpactObstacle(car);
        }
    }
}
