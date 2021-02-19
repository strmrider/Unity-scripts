using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAudio { Fire, Reload, Melee, Ready, Bolt, Pump};
public class WeaponAudioManager : MonoBehaviour
{
    [SerializeField] float volume = 0.1f;
    [SerializeField] AudioClip fire;
    [SerializeField] AudioClip reload;
    [SerializeField] AudioClip melee;
    [SerializeField] AudioClip ready;
    [SerializeField] AudioClip bolt;
    [SerializeField] AudioClip pump;


    private AudioSource fireSource;
    private AudioSource reloadSource;
    private AudioSource meleeSource;
    private AudioSource readySource;
    private AudioSource boltSource;
    private AudioSource pumpSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        fireSource = AddAudio(fire);
        reloadSource = AddAudio(reload);
        meleeSource = AddAudio(melee);
        readySource = AddAudio(ready);
        boltSource = AddAudio(bolt);
        pumpSource = AddAudio(pump);
    }

    private AudioSource AddAudio(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = false;
        source.playOnAwake = false;
        source.volume = volume;

        return source;
    }

    public void PlaySound(WeaponAudio sound)
    {
        switch (sound)
        {
            case WeaponAudio.Fire:
                fireSource.Play();
                break;
            case WeaponAudio.Reload:
                reloadSource.Play();
                break;
            case WeaponAudio.Melee:
                meleeSource.Play();
                break;
            case WeaponAudio.Ready:
                readySource.Play();
                break;
            case WeaponAudio.Bolt:
                boltSource.Play();
                break;
            case WeaponAudio.Pump:
                pumpSource.Play();
                break;
        }
    }
}
