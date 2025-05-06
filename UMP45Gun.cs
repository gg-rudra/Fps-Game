using UnityEngine;

public class UMP45Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public float damage = 20f;                // Damage per shot
    public float range = 100f;                 // Maximum range of the gun
    public float fireRate = 10f;               // Shots per second
    public Camera fpsCamera;                   // Reference to the player's camera
    public ParticleSystem muzzleFlash;         // Optional muzzle flash particle effect
    public AudioSource shootSound;             // Optional shooting sound

    [Header("Recoil Settings")]
    public float recoilForce = 2f;             // How much recoil affects camera rotation
    public float recoilSmoothness = 4f;        // How smoothly recoil is applied

    private float nextTimeToFire = 0f;

    private Vector3 currentRecoil = Vector3.zero;
    private Vector3 recoilVelocity = Vector3.zero;

    void Update()
    {
        // Handle shooting input
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        // Smoothly recover from recoil
        currentRecoil = Vector3.SmoothDamp(currentRecoil, Vector3.zero, ref recoilVelocity, 1f / recoilSmoothness);
        fpsCamera.transform.localRotation = Quaternion.Euler(currentRecoil);
    }

    void Shoot()
    {
        // Play muzzle flash if assigned
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Play shooting sound if assigned
        if (shootSound != null)
        {
            shootSound.Play();
        }

        // Recoil effect - small upwards rotation
        currentRecoil += new Vector3(-recoilForce, Random.Range(-recoilForce * 0.3f, recoilForce * 0.3f), 0);

        // Raycast from camera center forward
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            // Debug hit location
            Debug.Log("Hit: " + hit.transform.name);

            // Apply damage if target has a health script
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}

// Example target script to handle damage
// Attach this to any enemy or target object you want to damage
public class Target : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage.");

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject);
    }
}
