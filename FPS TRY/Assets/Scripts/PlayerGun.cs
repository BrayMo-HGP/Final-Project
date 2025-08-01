using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 50f;
    public Camera playerCamera;  // Assign this in Inspector

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Ray from center of screen
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Instantiate bullet at firePoint, pointing toward the ray direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(ray.direction));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = ray.direction * bulletSpeed;
        }
    }
}
