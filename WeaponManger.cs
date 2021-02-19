using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Weapon;

public class WeaponManger : MonoBehaviour
{
    public Gun[] weapons;
    [SerializeField] int defaultWeapon = 8;
    public GrenadeThrower grenadeThrower;
    public CharacterController character;
    private int currentWeapon = 0;
    private PlayerMovement charcterConteroller;
    public Camera fpsCamera;
    public GameObject crosshair;
    public PlayerUI playerUI;

    private bool grenadeThrowing = false;
    private bool isSwitching = false;
    private bool weaponHidden = false;

    private GearVest gearVest;

    // Start is called before the first frame update
    void Start()
    {
        charcterConteroller = character.GetComponent<PlayerMovement>();
        gearVest = new GearVest();
        InitWeaponsList();
        charcterConteroller.WeaponPositionEvent += new PlayerMovement.WeaponPosition(WeaponPosition);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            HideWeapon();
        }

        if (weaponHidden)
            return;

        if (Input.GetButton("Fire1"))
        {
            if (charcterConteroller.CanFire)
            {
                Weapon.Shoot();
            }
        }

        if (Input.GetMouseButtonDown(1) && charcterConteroller.IsWalking && charcterConteroller.OnGround)
        {
            crosshair.SetActive(false);
            Weapon.SightMode(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            crosshair.SetActive(true);
            Weapon.SightMode(false);
        }

        WeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Weapon.Reload();
        }
        
        if (Input.GetKeyDown(KeyCode.F) && CanAnimate())
        {
            Weapon.MeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }
    }

    private void ThrowGrenade()
    {
        Weapon.Selection(false);
        grenadeThrowing = true;
        isSwitching = true;
    }

    private void HideWeapon()
    {
        weaponHidden = !weaponHidden;
        Weapon.Selection(!weaponHidden);
    }
    private void WeaponSwitch()
    {
        if (Weapon.Type == WeaponType.SniperRifle && Weapon.IsOnSight || !charcterConteroller.IsWalking || Weapon.IsOnAction() || isSwitching)
            return;

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel != 0)
        {
            weapons[currentWeapon].Selection(false);
            isSwitching = true;

            if (mouseWheel > 0f)
            {
                if (currentWeapon == weapons.Length - 1)
                    currentWeapon = 0;
                else
                    currentWeapon++;
            }
            if (mouseWheel < 0f)
            {
                if (currentWeapon == 0)
                    currentWeapon = weapons.Length - 1;
                else
                    currentWeapon--;
            }
        }
    }

    public void UpdateAmmoUI(int ammo)
    {
        playerUI.UpdateAmmo(ammo, gearVest.Ammo.Amount(Weapon.GetAmmotype));
    }

    public Gun Weapon
    {
        get
        {
            return weapons[currentWeapon];
        }
    }

    private void InitWeaponsList()
    {
        currentWeapon = defaultWeapon;
        for(int i=0; i<weapons.Length; i++)
        {
            weapons[i].updateWeaponUIEvent += new Gun.UpdateAmmo(UpdateAmmoUI);
            weapons[i].updateAmmoEvent += new Gun.UpdateAmmo(UpdateAmmo);
            weapons[i].getAmmoEvent += new Gun.AmmoData(GetAmmo);
            weapons[i].selectionEvent += new Weapon.SetBool(WeaponSelection);
            weapons[i].gameObject.SetActive((i == defaultWeapon));
        }
        UpdateAmmoUI(Weapon.Magazine);
        playerUI.Weapon.SetWeaponIcon(Weapon.Icon);
    }
    
    public void UpdateAmmo(int amount)
    {
        gearVest.Ammo.SetAmount(Weapon.GetAmmotype, amount);
    }

    public int GetAmmo(AmmoType type)
    {
        return gearVest.Ammo.Amount(type);
    }

    public PlayerUI GetPlayerUI()
    {
        return playerUI;
    }

    public void WeaponPosition(string pos, bool state)
    {
        Weapon.SetPosition(pos, state);
    }

    public bool IsOnSight
    {
        get { return Weapon.IsOnSight; }
    }

    private bool CanAnimate()
    {
        return charcterConteroller.IsWalking && !Weapon.Reloading && !Weapon.IsOnSight;
    }

    private void WeaponSelection(bool state)
    {
       if(state)
            isSwitching = false;
       else if (!weaponHidden)
       {
            if (grenadeThrowing)
            {
                grenadeThrower.Throw();
                grenadeThrowing = false;
                StartCoroutine(SelectWeapon(grenadeThrower.throwingTime + 0.2f));
            }
            else
                StartCoroutine(SelectWeapon());
       }
    }

    private IEnumerator SelectWeapon(float waitingTime = 0.6f)
    {
        // delay weapon selection
        yield return new WaitForSeconds(waitingTime);
        weapons[currentWeapon].Selection(true);
        playerUI.UpdateAmmo(Weapon.Magazine, gearVest.Ammo.Amount(Weapon.GetAmmotype));
        playerUI.Weapon.SetWeaponIcon(Weapon.Icon);
    }
}
