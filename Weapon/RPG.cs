using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Weapon;

public class RPG : Gun
{
    [SerializeField] float launchForce = 2f;
    [SerializeField] GameObject rocket;
    public override void Shoot()
    {
        if (CanShoot())
            StartCoroutine(LaunchRocket());
        base.Shoot();
        PlayAnimation(WeaponAnimations.Reload, true);
    }

    private IEnumerator LaunchRocket()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject currentRocket = Instantiate(rocket, transform.position, transform.rotation);
        currentRocket.GetComponent<RpgRocket>().Launch(launchForce);
        //Rigidbody rb = currentRocket.GetComponent<Rigidbody>();
        //rb.AddForce(fpsCam.transform.forward * launchForce, ForceMode.VelocityChange);
    }
}
