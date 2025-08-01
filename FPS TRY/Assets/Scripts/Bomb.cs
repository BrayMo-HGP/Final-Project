using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;
    public GameObject explosionEffectPrefab;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Safety delete
    }

    void OnCollisionEnter(Collision collision)
    {
        // Instantiate explosion effect
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Destroy visual after short time
        }

        // Damage player
        if (collision.gameObject.CompareTag("Player"))
        {
            var health = collision.gameObject.GetComponent<PlayerHeath>(); // Your class name
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
