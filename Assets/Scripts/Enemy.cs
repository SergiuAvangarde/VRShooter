using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health;
    public int Damage;
    public int MovementSpeed;

    [SerializeField]
    private Transform enemyTransform;

    private void OnEnable()
    {
        StartCoroutine(MoveTowardsPlayer(Player.Instance.PlayerTransform));
    }
    private void OnDisable()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
    }

    private IEnumerator MoveTowardsPlayer(Transform player)
    {
        yield return new WaitForSeconds(1.5f);
        while (Vector3.Distance(enemyTransform.position, player.position) > 1)
        {
            float step = MovementSpeed * Time.deltaTime;
            enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, player.position, step);
            yield return null;
        }
    }
}
