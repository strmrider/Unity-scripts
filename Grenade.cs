using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrenadeType {Frag, Smoke}
public class Grenade : MonoBehaviour
{
    public GrenadeType type;
    public float force = 150f;
    public float imapctRadius = 10f;
    public float delay = 3f;
    public GameObject explosionEffect;
    public CameraEffects cameraShaker;

    private float countdown;
    private bool isLevered = false;
    private bool exploded = false;
    private AudioSource explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        explosionSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !exploded)
        {
            Explode();
            exploded = true;
        }
    }

    public void RemoveLever()
    {
        isLevered = true;

    }

    private void Operate()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && exploded)
            Explode();

    }

    private void Explode()
    {
        if(explosionSound)
            explosionSound.Play(0);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        //cameraShaker.GetComponent<CameraEffects>().ShakeCamera(.3f, 4f);
        if (type == GrenadeType.Frag)
            GameObject.Find("Main Camera").transform.GetComponent<CameraEffects>().ShakeCamera(.3f, 4f);
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, imapctRadius);
        Rigidbody rb;
        foreach (Collider nearbyObject in nearbyObjects)
        {
            rb = nearbyObject.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (type == GrenadeType.Frag)
                {
                    rb.AddExplosionForce(force, transform.position, imapctRadius / 2);
                    rb.AddExplosionForce(force / 2, transform.position, imapctRadius);
                }
            }
        }

        Destroy(gameObject, explosionSound.clip.length);
    }

    public GrenadeType Type
    {
        get { return type; }
    }
}
