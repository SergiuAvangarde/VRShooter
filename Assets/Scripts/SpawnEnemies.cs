using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    [SerializeField]
    private float enemiesRadius = 5;
    [SerializeField]
    private float searchRadius = 50;
    [SerializeField]
    private float spawnRadius = 20;
    [SerializeField]
    private int spawnTimer = 10;
    [SerializeField]
    private Transform environment = null;
    [SerializeField]
    private GameObject enemy = null;
    [SerializeField]
    private LayerMask enemyMask;
    [SerializeField]
    private Camera mainCamera;

    private Collider[] colliders;
    private int enemiesToSpawn = 20;

    private void Start()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            InstantiateEnemies(enemy);
        }
        StartCoroutine(EnemySpawner());
    }

    public void InstantiateEnemies(GameObject prefab)
    {
        bool canInstantiate = false;
        Vector3 spawnPos = new Vector3(0, prefab.transform.position.y, 0);

        while (!canInstantiate)
        {
            float instantiateX = Random.Range(spawnRadius, -spawnRadius);
            float instantiateZ = Random.Range(spawnRadius, -spawnRadius);

            spawnPos = new Vector3(instantiateX, spawnPos.y, instantiateZ);
            canInstantiate = PreventOverlap(spawnPos);

            if (canInstantiate)
            {
                break;
            }
        }
        var obj = Instantiate(prefab, spawnPos, Quaternion.identity, environment);
        float dim = Random.Range(-0.2f, 0.2f);
        obj.transform.localScale += new Vector3(dim, dim, dim);
    }

    public bool PreventOverlap(Vector3 spawnPos)
    {
        if (CameraCanSeePoint(spawnPos))
        {
            return false;
        }

        colliders = Physics.OverlapSphere(spawnPos, searchRadius, enemyMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 centerPoint = colliders[i].bounds.center;
            float widthX = colliders[i].bounds.extents.x;
            float widthZ = colliders[i].bounds.extents.z;

            float leftExtent = centerPoint.x - widthX * enemiesRadius;
            float rightExtent = centerPoint.x + widthX * enemiesRadius;
            float frontExtent = centerPoint.z - widthZ * enemiesRadius;
            float backExtent = centerPoint.z + widthZ * enemiesRadius;


            if (spawnPos.x >= leftExtent && spawnPos.x <= rightExtent)
            {
                if (spawnPos.z >= frontExtent && spawnPos.z <= backExtent)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool CameraCanSeePoint(Vector3 point)
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(point);
        return viewportPoint.z > 0 && new Rect(0, 0, 1, 1).Contains(viewportPoint);
    }

    private IEnumerator EnemySpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimer);
            InstantiateEnemies(enemy);
        }
    }
}
