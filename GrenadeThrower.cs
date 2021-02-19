using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public GameObject[] grenade;
    public Camera fpsCam;
    public float throwingTime = 1.5f;
    public float throwForce = 10f;
    private int grenadeType = 0;
    private WeaponManger weaponManager;
    private AudioSource throwSound;

    // Start is called before the first frame update
    void Start()
    {
        /*weaponManager = transform.gameObject.GetComponent<WeaponManger>();
        weaponManager.GetPlayerUI().Weapon.SetGrenadeAmount(GrenadeType.Explosive, weaponManager.Ammo.Grenade(GrenadeType.Explosive));
        weaponManager.GetPlayerUI().Weapon.SetGrenadeAmount(GrenadeType.Smoke, weaponManager.Ammo.Grenade(GrenadeType.Smoke));*/
        throwSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Throw()
    {
        gameObject.SetActive(true);
        StartCoroutine(_Throw());
    }

    private IEnumerator _Throw()
    {
        if (throwSound)
            throwSound.Play();
        // wait for throw animation to finish
        yield return new WaitForSeconds(1.5f);
        ThrowGrenade();
        gameObject.SetActive(false);
    }

    private void ThrowGrenade()
    {
        GrenadeType type = grenade[grenadeType].transform.GetComponent<Grenade>().Type;

        GameObject currentGrenade = Instantiate(grenade[grenadeType], transform.position, transform.rotation);
        Rigidbody rb = currentGrenade.GetComponent<Rigidbody>();
        rb.AddForce(fpsCam.transform.up * (throwForce / 3), ForceMode.VelocityChange);
        rb.AddForce(fpsCam.transform.forward * throwForce, ForceMode.VelocityChange);

        /*if (weaponManager.Ammo.Grenade(type) > 0)
        {
            GameObject currentGrenade = Instantiate(grenade[grenadeType], transform.position, transform.rotation);
            Rigidbody rb = currentGrenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * (throwForce / 3), ForceMode.VelocityChange);
            rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
            weaponManager.Ammo.UpdateGrenade(type, -1);
            weaponManager.GetPlayerUI().Weapon.SetGrenadeAmount(type, weaponManager.Ammo.Grenade(type));
        }*/
    }
}