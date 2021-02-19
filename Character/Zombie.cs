using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : CharacterMovement
{
    private bool attacking = false;
    // Start is called before the first frame update
   protected override void Start()
   {
        base.Start();
   }

    // Update is called once per frame
    void Update()
    {
       if (IsAlive)
            DetectTarget();
    }

    protected override void OnInteractionWithTarget()
    {
        /*if (distanceFromtarget > attackRange)
        {
            attacking = false;
        }*/
    }

    protected override void OnFollow()
    {
        //PlayAnimation("Crawl", true);
        if (!attacking)
            PlayAnimation("Walk", true);
    }

    protected override void OnStoppingDistance()
    {
        //PlayAnimation("Walk", false);
    }

    protected override void OnAttack()
    {
        Debug.Log("attacking");
        PlayAnimation("Walk", false);
        //animator.ResetTrigger("AttackTrigger");
        PlayAnimation("Attack");
        /*if (!attacking)
        {
            Debug.Log("attacking");
            PlayAnimation("Walk", false);
            //AttackDone();
            //animator.ResetTrigger("Attack");
            PlayAnimation("Attack", true);
            attacking = true;
            //AttackDone();
        }*/
    }

    public void AttackDone()
    {
        Debug.Log("attack done");
        //StartCoroutine(AttackStatus());
        //attacking = false;
    }

    public IEnumerator AttackStatus()
    {
        yield return new WaitForSeconds(.1f);
        attacking = false;
    }

    protected override void OnExistFromSight()
    {
        PlayAnimation("Walk", false);
    }

    public override void TakeDamage(RaycastHit[] hits, Vector3 hitPosition, AmmoType ammoType)
    {
        health = 0;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        int healthReduction = 0;
        bool enemyCollider = false;
        bool bodyPart = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //Debug.Log(hits[i].point);
            string collider = hits[i].collider.tag;

            if (string.Compare(collider, "HeadCollider") == 0 || string.Compare(collider, "LegCollider") == 0 || string.Compare(collider, "HandCollider") == 0)
                bodyPart = true;
         
            if (string.Compare(collider, "HeadCollider") == 0)
            {
                //Debug.Log(collider);
                healthReduction = 100;
            }
            else if (string.Compare(collider, "LegCollider") == 0)
            {
                //Debug.Log(collider);
                healthReduction = 15;
            }
            else if (string.Compare(collider, "HandCollider") == 0)
            {
                //Debug.Log(collider);
                healthReduction = 15;
            }
            else if (string.Compare(collider, "Enemy") == 0)
            {
                enemyCollider = true;
            }

            if (healthReduction > 0)
                break;
        }

        if (!bodyPart && enemyCollider)
        {
            healthReduction = 40;
        }

        health -= healthReduction;
        if (health <= 0)
            Die();
    }

    protected override void Die()
    {
        PlayAnimation("Walk", false);
        PlayAnimation("Attack", false);
        PlayAnimation("Fall", true);
    }
}
