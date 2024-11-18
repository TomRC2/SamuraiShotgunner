using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public GameObject Weapon;
    public GameObject bulletPrefab;
    public Sprite Shotgun;
    private bool isUsingShotgun = false;
    public Image Crosshair;

    public Sprite Katana;
    public Transform firePoint;
    public float shootCooldown = 0.5f;
    private float lastShootTime;
    public float bulletSpeed;
    public float weaponDistance = 1f;
    public int shotgunBullets = 5;
    public float spreadAngle = 30f;

    private SpriteRenderer weaponRenderer;

    void Start()
    {
        weaponRenderer = Weapon.GetComponent<SpriteRenderer>();
        Crosshair.gameObject.SetActive(false);
        weaponRenderer.sprite = Katana;
    }

    void Update()
    {
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
    void ToggleWeaponMode()
    {
        if (isUsingShotgun)
        {
            weaponRenderer.sprite = Shotgun;
            Crosshair.gameObject.SetActive(true);
        }
        else
        {
            weaponRenderer.sprite = Katana;
            Crosshair.gameObject.SetActive(false);
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
}