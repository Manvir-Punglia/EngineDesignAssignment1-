using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    public Material roadMaterial;
    public BoxCollider TargetArea;
    [HideInInspector] public int obstacleCount;

    IFactory factory;

    public OurPlugin.ConfigParser _configParser;

    private void Awake()
    {
        SpawnObstacles();

        if (_configParser == null)
        {
            _configParser = FindObjectOfType<OurPlugin.ConfigParser>();
            obstacleCount = _configParser.obstacleAmount;
        }
    }

    void SpawnObstacles()
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 spawnPos = GetPointInArea(TargetArea);

            if (isMaterialHit(spawnPos))
            {
                int rand = Random.Range(0, obstaclePrefab.Length);

                IFactory factory = new ObstacleFactory(obstaclePrefab[rand]);

                GameObject product = factory.CreateProduct();

                product.transform.position = spawnPos;
                product.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);

            }
            else
            {
                i--;
            }

        }
    }

    Vector3 GetPointInArea(BoxCollider boxCollider)
    {
        Bounds boxBounds = boxCollider.bounds;

        float x = Random.Range(boxBounds.min.x, boxBounds.max.x);
        float y = boxBounds.center.y;
        float z = Random.Range(boxBounds.min.z, boxBounds.max.z);

        return new Vector3(x, y, z);
    }

    bool isMaterialHit(Vector3 spawnPosition)
    {
        Ray ray = new Ray(spawnPosition + Vector3.up * 5f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();

            if (renderer != null && renderer.sharedMaterial == roadMaterial)
            {
                return true;
            }
        }

        return false;
    }
}
