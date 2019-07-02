using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Mele,
    Ranged,
}

public enum FireType
{
    Single,
    Burst,
    Auto
}

public enum BulletType
{
    Arrow,
    PistolRound,
    AssaultRifle,
    Sniper
}

public class Weapon : MonoBehaviour
{

    public WeaponType weaponType;
    public FireType fireType;
    public BulletType bulletType;

    public GameObject Target;

    [SerializeField]
    private GameObject muzzleFlash;

    [SerializeField]
    private AudioSource shootSound;
    [SerializeField]
    private AudioSource reloadSound;

    private void TurnOnMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
    }

    private void TurnOffMuzzleFlash()
    {
        muzzleFlash.SetActive(false);
    }

    private void PlayShootSound()
    {
        shootSound.Play();
    }

    private void PlayReloadSound()
    {
        reloadSound.Play();
    }


}
