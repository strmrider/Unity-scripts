using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Weapon;

public class Shotgun : Gun
{
    private int currentBullets;
    private int fullAmount;

    public override void Shoot()
    {
        base.Shoot();
        Pump();
    }

    private void Pump()
    {
        PlaySound(WeaponAudio.Pump);
        PlayAnimation(WeaponAnimations.Pump, true);
    }

    public void PumpDone()
    {
        PlayAnimation(WeaponAnimations.Pump, false);
    }

    public void StartReloadOnce()
    {
        currentBullets = 1;
        if (magazine > 0 && GetAmmoAmount() == 0)
            fullAmount = magazine;
        else
            fullAmount = fullMagazine;

        PlayAnimation(WeaponAnimations.ReloadOnce, true);
    }

    protected override void SelfReload()
    {
        StartCoroutine(PostponedReload());
    }

    private IEnumerator PostponedReload()
    {
        yield return new WaitForSeconds(1f);
        Reload();
    }

    public void ReloadOnceDone()
    {
        if (currentBullets < fullAmount)
        {
            currentBullets++;
            PlaySound(WeaponAudio.Reload);
            animator.Play("shotgunReloadOnce", -1, 0f);
        }
        else
        {
            PlayAnimation(WeaponAnimations.ReloadOnce, false);
        }
    }

    public override void ReloadDone()
    {
        PlayAnimation(WeaponAnimations.Reload, false);
        base.ReloadDone();
        Pump();
    }
}
