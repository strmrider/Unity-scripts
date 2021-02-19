using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Weapon;

public enum AmmoType { _556mm, _762mm, _9mm, _858mm, _50Cal, Rocket, Fuel};
public enum WeaponType {Melee, HandGun, AssaultRifle, SMG, MachineGun, SniperRifle, Shotgun, RocketLauncher, Flamethrower}
public class Weapon : MonoBehaviour
{
    [SerializeField] protected float damage = 25f;
    [SerializeField] protected Camera weaponCam;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected Camera fpsCam;
    [SerializeField] WeaponType type;

    protected Animator animator;
    protected WeaponAudioManager audioManager;
    protected bool isMelee = false;

    public delegate void SetBool(bool state);
    public event SetBool selectionEvent;

    void Start(){}
    void Update(){}

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
        audioManager = transform.GetComponent<WeaponAudioManager>();
    }

    protected void PlayAnimation(string animation, bool activity)
    {
        if (animator == null)
            return;
        else
            animator.SetBool(animation, activity);
    }

    protected void PlaySound(WeaponAudio sound)
    {
        if (audioManager)
            audioManager.PlaySound(sound);
    }

    public void StopAnimation(string animation)
    {
        PlayAnimation(animation, false);
    }

    public SetBool SelectionEvent
    {
        get { return selectionEvent; }
    }

    public void Selection(bool isSelected)
    {
        if (isSelected)
        {
            gameObject.SetActive(true);
            PlayAnimation(WeaponAnimations.Ready, true);
            PlaySound(WeaponAudio.Ready);
        }
        else
        {
            PlayAnimation(WeaponAnimations.Hide, true);
        }
    }

    public void Hide(bool state)
    {
        PlayAnimation(WeaponAnimations.Hide, state);
    }

    public void ReadyDone()
    {
        PlayAnimation(WeaponAnimations.Hide, false);
        PlayAnimation(WeaponAnimations.Ready, false);
        SelectionEvent(true);
    }

    public void HideDone()
    {
        SelectionEvent(false);
        gameObject.SetActive(false);
    }

    public void SetPosition(string pos, bool state)
    {
        PlayAnimation(pos, state);
    }

    public WeaponType Type
    {
        get { return type; }
    }

    public bool IsMelee
    {
        get { return isMelee; }
    }
}
