using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleVR;

public class GunScript : MonoBehaviour
{
    public Queue BulletPool = new Queue();

    [SerializeField]
    private float fireRate = 4f;
    [SerializeField]
    private int PoolAmount = 100;
    [SerializeField]
    private float MoveAmount = 1f;
    [SerializeField]
    private float MaxAmounth = 1f;
    [SerializeField]
    private float SmoothAmount = 10f;

    [SerializeField]
    private GvrReticlePointer pointerPos = null;

    [SerializeField]
    private GameObject bulletPrefab = null;
    [SerializeField]
    private Transform gunTransform = null;
    [SerializeField]
    private Transform gunDefaultTransform = null;
    [SerializeField]
    private Transform bulletPoolContainer = null;

    private float timeToFire;
    private int bulletLifeTime = 4;

    private void Awake()
    {
        for (int i = 0; i < PoolAmount; i++)
        {
            GameObject bulletObject = Instantiate(bulletPrefab, bulletPoolContainer);
            bulletObject.SetActive(false);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            BulletPool.Enqueue(bullet);
        }
    }

    private void Start()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            FireGun();
        }

        transform.position = Vector3.Slerp(transform.position, gunDefaultTransform.position, Time.deltaTime * SmoothAmount);
        transform.rotation = Quaternion.Slerp(transform.rotation, gunDefaultTransform.rotation, Time.deltaTime * SmoothAmount);
    }

    private void FireGun()
    {
        try
        {
            Bullet bullet = (Bullet)BulletPool.Dequeue();
            bullet.GunScriptComponent = this;
            bullet.FireBullet(gunTransform);
        }
        catch
        {
            GameObject bulletObject = Instantiate(bulletPrefab, bulletPoolContainer);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.GunScriptComponent = this;
            bullet.FireBullet(gunTransform);
        }
    }
}
