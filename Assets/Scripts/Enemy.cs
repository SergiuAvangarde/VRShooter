using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int MaxHealth;
    public int Damage;
    public int MovementSpeed;
    public int KillScore;

    [SerializeField]
    private Transform enemyTransform;
    [SerializeField]
    private Animator animatorComponent;
    [SerializeField]
    private AnimationClip attackAnimation;
    [SerializeField]
    private NavMeshAgent navAgent;

    private Player playerComponent;
    private Transform playerTransform;
    private float distance = 0;
    private int currentHealth;
    private float attackDuration;
    private float attackTimer;

    private bool dying = false;

    private void OnEnable()
    {
        currentHealth = MaxHealth;
        animatorComponent.SetBool("Attack", false);
        attackDuration = attackAnimation.length;
        attackTimer = attackDuration;
        playerComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(MoveTowardsPlayer(playerComponent.PlayerTransform));
    }
    private void OnDisable()
    {
        //StopAllCoroutines();
    }

    private void Update()
    {
        navAgent.speed = MovementSpeed;
        if (playerTransform != null)
        {
            distance = Vector3.Distance(playerTransform.position, enemyTransform.position);
        }

        if (distance < navAgent.stoppingDistance + 1f)
        {
            animatorComponent.SetBool("Attack", true);
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                attackTimer = 0;
                playerComponent.TakeDamage(Damage);
            }
        }
        else
        {
            animatorComponent.SetFloat("Speed", MovementSpeed);
            animatorComponent.SetBool("Attack", false);
        }
    }

    public void GetHit(int damage)
    {
        if (!dying)
        {
            animatorComponent.Play("Damage", 0, 0f);
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                dying = true;
                playerComponent.IncreaseScore(KillScore);
                StopAllCoroutines();
                StartCoroutine(DieAnimation());
            }
        }
    }

    private void MoveToPlayer(Transform player)
    {
        navAgent.SetDestination(player.position);
    }

    private IEnumerator MoveTowardsPlayer(Transform player)
    {
        playerTransform = player;
        yield return new WaitForSeconds(2f);
        MoveToPlayer(player);
    }

    private IEnumerator DieAnimation()
    {
        navAgent.SetDestination(enemyTransform.position);
        animatorComponent.Play("Death", 0, 0f);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
