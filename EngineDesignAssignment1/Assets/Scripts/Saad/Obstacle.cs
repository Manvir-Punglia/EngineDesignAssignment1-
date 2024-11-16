using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float _obstacleImpact;
    public string obstacleName;

    public void ImpactObstacle(Car car, float ObstacleImpact)
    {

        if (car != null)
        {
            _obstacleImpact = ObstacleImpact;
            car.ReduceSpeed(ObstacleImpact);
        }

        Debug.Log(obstacleName);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Car car = other.GetComponent<Car>();
            ImpactObstacle(car, _obstacleImpact);
        }
    }
}
