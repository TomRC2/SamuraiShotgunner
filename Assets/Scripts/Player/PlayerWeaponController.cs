using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public GameObject Weapon;
    public GameObject bulletPrefab;
    public List<Sprite> WeaponSprites;
    public Sprite shotgun;
    public List<Vector2> ColliderSizes;
    public Image Crosshair;

    public Transform firePoint;
    public float shootCooldown = 3f;
    private float lastShootTime;
    public float bulletSpeed;
    public float weaponDistance = 1f;
    public int shotgunBullets = 5;
    public float spreadAngle = 30f;

    private SpriteRenderer weaponRenderer;
    private Collider2D swordCollider;
    private bool isUsingShotgun = false;
    private int currentWeaponIndex = 0;

    private Player player;

    void Start()
    {
        swordCollider = Weapon.GetComponent<Collider2D>();
        weaponRenderer = Weapon.GetComponent<SpriteRenderer>();
        Crosshair.gameObject.SetActive(false);

        if (WeaponSprites != null && WeaponSprites.Count > 0)
        {
            weaponRenderer.sprite = WeaponSprites[0]; // Configura el arma inicial
        }

        UpdateSwordCollider(); // Ajusta el collider al arma inicial
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (player != null)
        {
            shootCooldown = Mathf.Max(0.1f, 1.5f - (player.level - 1) * 0.05f);

            // Cambia el arma basada en el nivel del jugador
            if (player.level == 5 && currentWeaponIndex < 1)
            {
                ChangeWeapon(1);
            }
            else if (player.level == 10 && currentWeaponIndex < 2)
            {
                ChangeWeapon(2);
            }
            else if (player.level == 15 && currentWeaponIndex < 3)
            {
                ChangeWeapon(3);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            isUsingShotgun = !isUsingShotgun;
            ToggleWeaponMode();
        }

        if (isUsingShotgun && Input.GetMouseButton(0) && Time.time >= lastShootTime + shootCooldown)
        {
            lastShootTime = Time.time;
            Shoot();
        }

        RotateWeapon();
    }

    public void ChangeWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < WeaponSprites.Count)
        {
            currentWeaponIndex = weaponIndex;
            weaponRenderer.sprite = WeaponSprites[weaponIndex];
            UpdateSwordCollider();
            Debug.Log($"Arma cambiada a: {weaponIndex}");
        }
    }

    void ToggleWeaponMode()
    {
        if (isUsingShotgun)
        {
            weaponRenderer.sprite = shotgun;
            Crosshair.gameObject.SetActive(true);
            swordCollider.enabled = false;
        }
        else
        {
            weaponRenderer.sprite = WeaponSprites[currentWeaponIndex];
            Crosshair.gameObject.SetActive(false);
            swordCollider.enabled = true;
        }
    }


    void Shoot()
    {
        for (int i = 0; i < shotgunBullets; i++)
        {
            float angle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bullet.transform.right * bulletSpeed;

            Destroy(bullet, 2f);
        }
    }

    void RotateWeapon()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - transform.position).normalized;

        Weapon.transform.position = transform.position + direction * weaponDistance;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Vector3 localScale = Weapon.transform.localScale;
        localScale.y = (angle > 90 || angle < -90) ? -1 : 1;
        Weapon.transform.localScale = localScale;
    }

    public void UpdateSwordCollider()
    {
        if (ColliderSizes != null && currentWeaponIndex < ColliderSizes.Count)
        {
            if (swordCollider is BoxCollider2D boxCollider)
            {
                boxCollider.size = ColliderSizes[currentWeaponIndex];
            }
            else
            {
                Debug.LogWarning("El collider no es un BoxCollider2D. Verifica el tipo de collider.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un tamaño de collider válido para este arma.");
        }
    }

}
