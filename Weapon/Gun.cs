using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Weapon;

public class Gun : Weapon
{
    [SerializeField] protected float range = 100f;
    [SerializeField] protected float fireRate = 10f;
    [SerializeField] protected int magazineCapacity = 30;
    [SerializeField] protected bool automatic = true;
    [SerializeField] protected AmmoType ammoType;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] GameObject projectile;
    [SerializeField] string fireAnimationName;
    [SerializeField] bool unlimited = false;

    public int magazine = 5;
    protected bool isReloading = false;
    protected float nextShotInRate = 0f;
    protected bool isOnSight = false;
    protected int fullMagazine = 0;
    /* scope */
    protected bool hasCanvasScope = false;
    protected Scope scope;
    protected bool cancelScope = false;

    public delegate void UpdateAmmo(int amount);
    public delegate int AmmoData(AmmoType type);
    public event UpdateAmmo updateWeaponUIEvent;
    public event UpdateAmmo updateAmmoEvent;
    public event AmmoData getAmmoEvent;

    protected bool CanShoot()
    {
        return !(getAmmoEvent(ammoType) <= 0 && magazine == 0 || isReloading || Time.time < nextShotInRate);
    }

    private void Start()
    {
        DetectScope();
    }

    protected void DetectScope()
    {
        scope = gameObject.GetComponent<Scope>();
        if (scope)
            hasCanvasScope = true;
    }

    public virtual void Shoot()
    {
        if (!CanShoot())
            return;
        nextShotInRate = Time.time + 1f / fireRate;
        StartCoroutine(PlayShootEffects());
        //PlayShootEffects();
        // info about hitted target
        RaycastHit target;
        bool isHit = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out target, range);
        RaycastHit[] hits =  Physics.RaycastAll(fpsCam.transform.position, fpsCam.transform.forward, range);
        HandleHits(hits);
        //Debug.Log(target.transform);
        if (isHit)
        {
            Zombie enemy = target.transform.GetComponent<Zombie>();
            if (enemy != null)
            {
                //enemy.TakeDamage(hits, target.point, ammoType);
            }
        }
        /*if (isHit)
        {
            Zombie enemy = target.transform.GetComponent<Zombie>();
            // makes sure it's actualy an enemy type object and not one
            if (enemy != null)
            {
                Debug.Log(target.collider.tag);
                //enemy.TakeDamage(ammoType);
            }
            else
            {
                //SolidHitEffect(target);
            }
        }*/

        if (!unlimited)
            magazine--;

        updateWeaponUIEvent(magazine);

        if (magazine == 0)
        {
            SelfReload();
        }
    }

    protected void HandleHits(RaycastHit[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //Debug.Log(hits[i].transform.name);
            string collider = hits[i].collider.transform.tag;
            //Debug.Log(collider);
           if (collider == "HeadCollider")
                Debug.Log("HeadShot");
            else if (collider == "LegCollider")
                Debug.Log("Leg shot");
            else if (collider == "HandCollider")
                Debug.Log("Hand shot");
            else if (collider == "TorsoCollider")
                Debug.Log("Torso shot");
            //else
            //  Debug.Log("Middle shot");
        }
    }

    protected virtual void SelfReload()
    {
        Reload();
    }

    private IEnumerator PlayShootEffects()
    {
        PlaySound(WeaponAudio.Fire);
        PlayAnimation(WeaponAnimations.Fire, true);
        //animator.Play(WeaponAnimations.Fire, -1, 0f);
        if (muzzleFlash)
            muzzleFlash.Play();
        if (projectile)
            ShootProjectile();
        yield return new WaitForSeconds(0.001f);
        PlayAnimation(WeaponAnimations.Fire, false);
    }

    protected void ShootProjectile()
    {
        GameObject currentprojectile = Instantiate(projectile, transform.position, transform.rotation);
        currentprojectile.transform.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, 200f));
        Destroy(currentprojectile.gameObject, 1f);
    }

    public void FireDone()
    {
        //PlayAnimation(WeaponAnimations.Fire, false);
        
    }

    private void ReloadEffects()
    {
        PlaySound(WeaponAudio.Reload);
        PlayAnimation(WeaponAnimations.Reload, true);
    }
    
    public virtual void Reload()
    {
        int ammo = getAmmoEvent(ammoType);
        if (ammo > 0 && !isReloading && magazine < magazineCapacity)
        {
            if (!hasCanvasScope)
                PlayAnimation(WeaponAnimations.Sight, false);
            isReloading = true;
            PlayAnimation(WeaponAnimations.Reload, true);
            PlaySound(WeaponAudio.Reload);

            fullMagazine = magazineCapacity - magazine;

            // load full magazine
            if (ammo >= fullMagazine)
            {
                magazine += fullMagazine;
                updateAmmoEvent(-fullMagazine);
            }
            // load rest of the ammo
            else
            {
                magazine += ammo;
                updateAmmoEvent(-ammo);
            }
        }
    }

    public virtual void ReloadDone()
    {
        PlayAnimation(WeaponAnimations.Reload, false);
        isReloading = false;

        updateWeaponUIEvent(magazine);
    }

    public virtual void ReloadDoneAnimationEvent()
    {
        if (magazine == magazineCapacity || magazine > 0 && getAmmoEvent(ammoType) == 0)
        {
            ReloadDone();
        }
    }

    /***************** 
     * Sight and scope
     *****************/
    public virtual void SightMode(bool isActive)
    {

        if (hasCanvasScope && !isActive)
            ScopeMode(isActive);
        else
            cancelScope = false;

        PlayAnimation(WeaponAnimations.Sight, isActive);
    }

    protected void DisplayScope(bool state)
    {
        if (hasCanvasScope)
        {
            weaponCam.gameObject.SetActive(!state);
            scope.DisplayScope(state);
            isOnSight = state;
        }
    }

    public void DisplayScopeAnimationEvent()
    {
        // in case scope was canceled before it was set (due to animation duration)
        if (!cancelScope)
        {
            ScopeMode(true);
        }
    }

    protected void ScopeMode(bool isActive)
    {
        if (isActive)
        {
            cancelScope = false;
            DisplayScope(true);
        }
        else if (!isActive)
        {
            cancelScope = true;
            if (IsOnSight)
            {
                DisplayScope(false);
                cancelScope = false;
            }
        }
    }

    public void MeleeAttack()
    {
        isMelee = true;
        PlaySound(WeaponAudio.Melee);
        PlayAnimation(WeaponAnimations.Melee, true);
    }

    public void MeleeDone()
    {
        isMelee = false;
        PlayAnimation(WeaponAnimations.Melee, false);
    }

    /************
     * Properties
     ************/
    public bool IsOnSight
    {
        get { return isOnSight; }
    }

    public bool Reloading
    {
        get { return isReloading; }

    }
    public bool IsOnAction()
    {
        return (isReloading || isMelee || isOnSight);
    }

    public AmmoType GetAmmotype
    {
        get { return ammoType; }
    }

    public int Magazine
    {
        get { return magazine; }
    }

    public Sprite Icon
    {
        get { return icon; }
    }

    public int GetAmmoAmount()
    {
        return getAmmoEvent(ammoType);
    }
}
