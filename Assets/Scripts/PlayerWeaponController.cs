using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public Transform player;

    public GameObject Weapon;
    public GameObject bulletPrefab;
    public Sprite Shotgun;
    private bool isUsingShotgun = false;
    public Image Crosshair;

    public Sprite Katana;
    public Transform firePoint;
    public float bulletSpeed;
    public float weaponDistance = 1f;


    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer weaponRenderer;
    void Start()
    {
        weaponRenderer = Weapon.GetComponent<SpriteRenderer>();
        Crosshair.gameObject.SetActive(false);
        weaponRenderer.sprite = Katana;
    }
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        RotateWeapon();

        if (Input.GetMouseButtonDown(1))
        {
            isUsingShotgun = !isUsingShotgun;
            ToggleWeaponMode();
        }
        if (isUsingShotgun && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }


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
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletSpeed;
    }
    void RotateWeapon()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - transform.position).normalized;

        Weapon.transform.position = transform.position + direction * weaponDistance;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}

