using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public int spawnTimer = 10;

    [SerializeField]
    private int enemiesToSpawn = 20;
    [SerializeField]
    private float enemiesRadius = 5;
    [SerializeField]
    private float searchRadius = 50;
    [SerializeField]
    private float spawnRadius = 20;
    [SerializeField]
    private Transform environment = null;
    [SerializeField]
    private GameObject[] enemies = null;
    [SerializeField]
    private LayerMask enemyAndPlayerMask;
    [SerializeField]
    private LayerMask obstacleMask;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private Camera mainCamera;

    private Collider[] enemyColliders;
    private Collider[] obstacleColliders;

    private void Start()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemy = Random.Range(0, enemies.Length);
            InstantiateEnemies(enemies[randomEnemy]);
        }
        StartCoroutine(EnemySpawner());
    }

    public void InstantiateEnemies(GameObject prefab)
    {
        bool canInstantiate = false;
        Vector3 spawnPos = new Vector3(0, 2, 0);

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
        RaycastHit hit;
        if (Physics.Raycast(spawnPos, Vector3.down, out hit, Mathf.Infinity, groundMask))
        {
            var obj = Instantiate(prefab, hit.point + prefab.transform.position, Quaternion.identity, environment);
            float dim = Random.Range(-0.2f, 0.2f);
            obj.transform.localScale += new Vector3(dim, dim, dim);
        }
    }

    public bool PreventOverlap(Vector3 spawnPos)
    {
        if (CameraCanSeePoint(spawnPos))
        {
            return false;
        }

        enemyColliders = Physics.OverlapSphere(spawnPos, searchRadius, enemyAndPlayerMask);
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            Vector3 centerPoint = enemyColliders[i].bounds.center;
            float widthX = enemyColliders[i].bounds.extents.x;
            float widthZ = enemyColliders[i].bounds.extents.z;

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

        obstacleColliders = Physics.OverlapSphere(spawnPos, searchRadius, obstacleMask);
        for (int i = 0; i < obstacleColliders.Length; i++)
        {
            Vector3 centerPoint = obstacleColliders[i].bounds.center;
            float widthX = obstacleColliders[i].bounds.extents.x;
            float widthZ = obstacleColliders[i].bounds.extents.z;

            float leftExtent = centerPoint.x - widthX;
            float rightExtent = centerPoint.x + widthX;
            float frontExtent = centerPoint.z - widthZ;
            float backExtent = centerPoint.z + widthZ;


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
            int randomEnemy = Random.Range(0, enemies.Length);
            InstantiateEnemies(enemies[randomEnemy]);
        }
    }
}
