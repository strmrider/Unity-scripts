using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgRocket : MonoBehaviour
{
    [SerializeField] float impactForce = 150f;
    [SerializeField] float imapctRadius = 10f;
    [SerializeField] GameObject rocketTrailEfefct;
    [SerializeField] GameObject explosionEffect;

    private AudioSource explosionSound;
    private bool exploded = false;
    private bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        explosionSound = gameObject.GetComponent<AudioSource>();
        explosionSound.volume = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch(float force)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * force, ForceMode.VelocityChange);
        rb.velocity = transform.TransformDirection(new Vector3(0, -10f, 200f));
        if (rocketTrailEfefct)
            Instantiate(rocketTrailEfefct, transform.position, transform.rotation);
    }

    private void Explode()
    {
        explosionSound.Play();
        if (explosionEffect)
           Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, imapctRadius);
        Rigidbody rb;
        foreach (Collider nearbyObject in nearbyObjects)
        {
            rb = nearbyObject.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(impactForce, transform.position, imapctRadius / 2);
                rb.AddExplosionForce(impactForce / 2, transform.position, imapctRadius);
            }
        }
        
        Destroy(gameObject, explosionSound.clip.length);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided && collision.transform.name != "First Person Player")
        {
            hasCollided = true;
            Explode();
        }
    }
}
