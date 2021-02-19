using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] WeaponUI weaponUI;
    [SerializeField] PlayerBar healthBar;
    [SerializeField] PlayerBar energyBar;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EffectEnergy(int value)
    {
        energyBar.ChangeValue(value);
    }

    public void UpdateAmmo(int magazine, int totalAmmo)
    {
        weaponUI.SetAmmoAmount(magazine, totalAmmo);
    }

    public WeaponUI Weapon
    {
        get { return weaponUI; }
    }
}
