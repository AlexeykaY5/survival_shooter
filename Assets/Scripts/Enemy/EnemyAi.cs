using Game.Utils;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float loseRadius = 8f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float chaseUpdateInterval = 0.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damage = 50;

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTime;
    private float chaseUpdateTimer;
    private float nextAttackTime;
    private Vector3 roamPosition;
    private Vector3 startingPosition;

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = startingState;
    }

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        if(Player.Instance == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (state == State.Idle || state == State.Roaming) {
            if (distanceToPlayer <= detectionRadius)
            {
                state = State.Chasing;
            }
        } else if (state == State.Chasing)
        {
            if (distanceToPlayer <= attackRange)
            {
                state = State.Attacking;
                navMeshAgent.ResetPath();
            } else if (distanceToPlayer > loseRadius)
            {
                state = State.Roaming;
                roamingTime = 0;
            }
        } else if (state == State.Attacking)
        {
            if (distanceToPlayer > attackRange * 1.3f)
            {
                state = State.Chasing;
                chaseUpdateTimer = 0;
            }
        }

        switch (state)
        {
            default:
            case State.Idle:
                break;
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0)
                {
                    Roaming();
                    roamingTime = roamingTimerMax;
                }
                break;
            case State.Chasing:
                chaseUpdateTimer -= Time.deltaTime;
                if (chaseUpdateTimer < 0)
                {
                    navMeshAgent.SetDestination(Player.Instance.transform.position);
                    chaseUpdateTimer = chaseUpdateInterval;
                }
                break;
            case State.Attacking:
                if (Time.time >= nextAttackTime)
                {
                    AttackPlayer();
                    nextAttackTime = Time.time + attackCooldown;
                }
                break;
        }
    }

    private void AttackPlayer()
    {
        if(Player.Instance.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(damage);
        }
    }

    private void Roaming()
    {
        roamPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }
}