using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterMovement
{
    [SerializeField] protected float health;

    bool isActive = true;
    protected Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
            DetectTarget();
    }

    protected void Die()
    {
        isActive = false;
    }
}
