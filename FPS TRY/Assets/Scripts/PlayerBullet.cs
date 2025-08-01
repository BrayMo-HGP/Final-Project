using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float lifetime = 2f;
    public int damage = 1;
    public GameObject hitEffect; // Optional VFX prefab

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Damage enemy if it has the component
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }

        // Spawn hit effect at collision contact point
        if (hitEffect != null && collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(hitEffect, contact.point, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
