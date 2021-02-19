using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Text ammoText;
    [SerializeField] Image icon;
    [SerializeField] Text explosiveGrenade;
    [SerializeField] Text smokeGrenade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAmmoAmount(int magazine, int totalAmmo)
    {
        ammoText.text = magazine + "/" + totalAmmo;
    }

    public void SetWeaponIcon(Sprite icon)
    {
        this.icon.sprite = icon;
    }

    public void SetGrenadeAmount(GrenadeType type, int amount)
    {
        if (type == GrenadeType.Frag)
            explosiveGrenade.text = amount + "";
        else if (type == GrenadeType.Smoke)
            smokeGrenade.text = amount + "";
    }
}
