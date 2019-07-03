using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GunScript GunScriptComponent { get; set; }
    public Transform BulletTransform;
    public Rigidbody BulletRigidBody;
    public float TravelSpeed = 50f;
    public int Damage = 0;

    [SerializeField]
    private TrailRenderer bulletTrail = null;
    [SerializeField]
    private GameObject bulletHole = null;

    private int bulletLifeTime = 3;

    public void FireBullet(Transform gunTransform)
    {
        gameObject.SetActive(true);
        BulletTransform.position = gunTransform.position;
        BulletTransform.rotation = gunTransform.rotation;
        StartCoroutine(BulletTravell());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().GetHit(Damage);
        }
        StopAllCoroutines();
        BulletRigidBody.velocity = Vector3.zero;
        BulletTransform.position = Vector3.zero;
        gameObject.SetActive(false);
        GunScriptComponent.BulletPool.Enqueue(this);
    }

    private IEnumerator BulletTravell(/*, Vector3 target*/)
    {
        float time = 0;
        bulletTrail.Clear();
        while (time < bulletLifeTime)
        {
            time += Time.deltaTime;
            BulletTransform.position += BulletTransform.forward * (TravelSpeed * Time.fixedDeltaTime);
            //bullet.position = Vector3.Lerp(bullet.position, target, speed * Time.fixedDeltaTime);
            yield return null;
        }
        gameObject.SetActive(false);
        BulletRigidBody.velocity = Vector3.zero;
        BulletTransform.position = Vector3.zero;
        GunScriptComponent.BulletPool.Enqueue(this);
        StopAllCoroutines();
    }
}
