using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleVR;

public class GunScript : MonoBehaviour
{
    public Queue BulletPool = new Queue();
    public Queue BulletCasingPool = new Queue();

    [Header("Weapon Settings")]
    [SerializeField]
    private float fireRate = 4f;
    [SerializeField]
    private int maxAmmo = 10;
    [SerializeField]
    private int BulletPoolAmount = 100;
    [SerializeField]
    private float SmoothMovement = 15f;

    [Header("Muzzleflare Settings")]
    [SerializeField]
    private Light muzzleflashLight;
    [SerializeField]
    private ParticleSystem muzzleParticles;
    [SerializeField]
    private ParticleSystem sparkParticles;
    [SerializeField]
    private int minSparkEmission = 1;
    [SerializeField]
    private int maxSparkEmission = 7;

    [Header("Prefabs and transforms")]
    [SerializeField]
    private GvrReticlePointer pointerPos = null;
    [SerializeField]
    private Animator weaponAnimator = null;
    [SerializeField]
    private GameObject bulletPrefab = null;
    [SerializeField]
    private GameObject bulletCasingPrefab = null;
    [SerializeField]
    private Transform bulletSpawnPoint = null;
    [SerializeField]
    private Transform bulletCasingSpawnPoint = null;
    [SerializeField]
    private Transform gunTransform = null;
    [SerializeField]
    private Transform gunDefaultTransform = null;
    [SerializeField]
    private Transform bulletPoolContainer = null;
    [SerializeField]
    private Text ammoText = null;


    [SerializeField]
    private AudioSource GunAudioSource = null;

    private float timeToFire;
    private int currentAmmo = 0;
    private int bulletLifeTime = 4;
    private bool reloading = false;

    private void Awake()
    {
        for (int i = 0; i < BulletPoolAmount; i++)
        {
            GameObject bulletObject = Instantiate(bulletPrefab, bulletPoolContainer);
            GameObject bulletCasingObject = Instantiate(bulletCasingPrefab, bulletPoolContainer);
            bulletObject.SetActive(false);
            bulletCasingObject.SetActive(false);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            BulletPool.Enqueue(bullet);
            BulletCasingPool.Enqueue(bulletCasingObject);
        }
        currentAmmo = maxAmmo;
        CheckAmmo();
    }

    private void Start()
    {

    }

    void FixedUpdate()
    {
        AnimationCheck();
        if (Input.GetMouseButton(0) && Time.time >= timeToFire && !reloading)
        {
            timeToFire = Time.time + 1 / fireRate;
            weaponAnimator.Play("Fire", 0, 0f);
            FireGun();
            EmitMuzzleParticles();
            currentAmmo -= 1;
            CheckAmmo();
        }

        gunTransform.position = Vector3.Slerp(gunTransform.position, gunDefaultTransform.position, Time.deltaTime * SmoothMovement);
        gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, gunDefaultTransform.rotation, Time.deltaTime * SmoothMovement);
    }

    private void FireGun()
    {
        try
        {
            Bullet bullet = (Bullet)BulletPool.Dequeue();
            GameObject bulletCasing = (GameObject)BulletCasingPool.Dequeue();
            bullet.GunScriptComponent = this;
            bullet.Damage = Player.Instance.Damage;
            bullet.FireBullet(bulletSpawnPoint);
            bulletCasing.SetActive(true);
            Transform casingTransform = bulletCasing.GetComponent<Transform>();
            casingTransform.position = bulletCasingSpawnPoint.position;
            casingTransform.rotation = bulletCasingSpawnPoint.rotation;
            DestroyBulletCasing(bulletCasing);
        }
        catch
        {
            GameObject bulletObject = Instantiate(bulletPrefab, bulletPoolContainer);
            GameObject bulletCasingObject = Instantiate(bulletCasingPrefab, bulletPoolContainer);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.GunScriptComponent = this;
            bullet.Damage = Player.Instance.Damage;
            bullet.FireBullet(bulletSpawnPoint);
            bulletCasingObject.SetActive(true);
            Transform casingTransform = bulletCasingObject.GetComponent<Transform>();
            casingTransform.position = bulletCasingSpawnPoint.position;
            casingTransform.rotation = bulletCasingSpawnPoint.rotation;
            DestroyBulletCasing(bulletCasingObject);
        }
    }

    private void EmitMuzzleParticles()
    {
        sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
        muzzleParticles.Emit(1);
        StartCoroutine(MuzzleFlashLight());
    }

    private void AnimationCheck()
    {
        if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo") ||
            weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left"))
        {
            reloading = true;
        }
        else
        {
            reloading = false;
        }
    }

    private void CheckAmmo()
    {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
        if (currentAmmo == 0)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator DestroyBulletCasing(GameObject bulletCasing)
    {
        yield return new WaitForSeconds(2f);
        bulletCasing.SetActive(false);
        BulletCasingPool.Enqueue(bulletCasing);
    }

    private IEnumerator Reload()
    {
        weaponAnimator.Play("Reload Out Of Ammo", 0, 0f);
        yield return new WaitForSeconds(1f);
        currentAmmo = maxAmmo;
        CheckAmmo();
    }

    private IEnumerator MuzzleFlashLight()
    {
        muzzleflashLight.enabled = true;
        yield return new WaitForSeconds(0.02f);
        muzzleflashLight.enabled = false;
    }

}
