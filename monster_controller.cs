using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackRange = 1.5f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    private float lastAttackTime;
    private Transform player;

    void Start()
    {
        // Find the player in the scene (assuming there's only one player)
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        MoveTowardsPlayer();
        if (IsPlayerInRange() && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        // Here you would typically call a method on the player to apply damage
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        Debug.Log("Monster attacked the player for " + attackDamage + " damage!");
    }
}