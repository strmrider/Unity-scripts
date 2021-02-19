using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Weapon;

public class SniperRifle : Gun
{
    private bool pendingReload = false;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        audioManager = gameObject.GetComponent<WeaponAudioManager>();
        DetectScope();
        magazine = magazineCapacity;
    }

    public override void Reload()
    {
        // reload will be performed only after bolt action
        pendingReload = true;
    }

    public override void Shoot()
    {
        base.Shoot();
        if (scope.ScopeOn)
            StartCoroutine(Recoil());
        PlayAnimation(WeaponAnimations.SniperBolt, true);
        PlaySound(WeaponAudio.Bolt);
    }

    public void SniperFireEffect()
    {
        if (scope.ScopeOn)
            StartCoroutine(Recoil());
        PlaySound(WeaponAudio.Bolt);
    }

    public void FireDoneAnimationEvent()
    {
        PlayAnimation(WeaponAnimations.SniperBolt, false);
    }

    private IEnumerator Recoil()
    {
        System.Random random = new System.Random();
        float xRecoil = (float)random.NextDouble() * (2f - (-2f)) + -2f;
        fpsCam.GetComponent<CameraEffects>().EnableAnimation(false);
        Vector3 v = new Vector3(fpsCam.transform.eulerAngles.x - xRecoil,
            fpsCam.transform.eulerAngles.y, fpsCam.transform.eulerAngles.z);
        fpsCam.transform.eulerAngles = v;
        //fpsCam.transform.Rotate(Vector3.left, 15.0f * Time.deltaTime);
        fpsCam.transform.Translate(0, 0, -0.01f * Time.deltaTime);
        yield return new WaitForSeconds(0.18f);
        //fpsCam.transform.Rotate(Vector3.left, -15.0f * Time.deltaTime);
        v = new Vector3(fpsCam.transform.eulerAngles.x + xRecoil,
            fpsCam.transform.eulerAngles.y, fpsCam.transform.eulerAngles.z);
        fpsCam.transform.eulerAngles = v;
        fpsCam.GetComponent<CameraEffects>().EnableAnimation(true);
    }

    public void BoltActionDone()
    {
        PlayAnimation(WeaponAnimations.SniperBolt, false);
        if (pendingReload)
        {
            if (IsOnSight)
            {
                SightMode(false);
            }
            base.Reload();
            pendingReload = false;
        }
    }
}
