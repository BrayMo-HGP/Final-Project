using UnityEngine;
using UnityEngine.AI;
using MyGame.Navigation;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;
    public NavMeshAgent Agent => agent;
    public GameObject Player { get => player; }
    public Path path;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;

    [Range(0.1f, 10f)]
    public float fireRate;

    [SerializeField]
    private string currentState;

    [Header("Health and Spawning")]
    public int maxHealth = 5;
    private int currentHealth;

    public GameObject enemyPrefab;
    private static int spawnMultiplier = 1;

    [Header("Hit Flash")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;

    private Renderer enemyRenderer;
    private Color originalColor;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;

        // Setup hit flash
        enemyRenderer = GetComponentInChildren<Renderer>(); // Works for child mesh
        if (enemyRenderer != null)
        {
            // Clone material instance so flash doesn't affect all enemies
            enemyRenderer.material = new Material(enemyRenderer.material);
            originalColor = enemyRenderer.material.color;
        }
    }

    private void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.ActiveState?.ToString() ?? "NoState";
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection.normalized);
                    RaycastHit hitInfo;

                    Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red, 0.1f);

                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    // 🔫 Call this from Bullet script
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (enemyRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            MultiplyAndRespawn();
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashRed()
    {
        enemyRenderer.material.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.material.color = originalColor;
    }

    private void MultiplyAndRespawn()
    {
        int newCount = spawnMultiplier * 2;
        spawnMultiplier = newCount;

        for (int i = 0; i < newCount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(enemyPrefab, transform.position + randomOffset, Quaternion.identity);
        }
    }
}
