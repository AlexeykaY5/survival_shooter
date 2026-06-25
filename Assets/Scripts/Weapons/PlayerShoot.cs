using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 0.2f;
    //[SerializeField] private int bulletsQuantity = 10;  
    [SerializeField] private int manaCostPerShoot = 5;

    private Mana mana;
    private float nextFireTime;

    private void Awake()
    {
        mana = GetComponent<Mana>();
    }

    private void Update()
    {
        bool isHolding = GameInput.Instance.IsAttacking();

        if (isHolding && Time.time >= nextFireTime)
        {
            if(mana == null || mana.TryUse(manaCostPerShoot))
            {
                FireBullet();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void FireBullet()
    {
        Vector2 direction = GameInput.Instance.GetAimDirection(transform.position);
        if (direction.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;
    }
}
