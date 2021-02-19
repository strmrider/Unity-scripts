using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float sightRadius = 25f;
    [SerializeField] float sightRange;
    [SerializeField] float sightAngle = 45f;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float health = 100f;

    protected Transform target;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected bool noticedTarget = false;
    protected float distanceFromtarget = 0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        target = PlayerRef.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //DetectTarget();
    }

    protected void PlayAnimation(string animation, bool state)
    {
        if (animator)
        {
            animator.SetBool(animation, state);
        }
    }

    protected void PlayAnimation(string animation)
    {
        if (animator)
            animator.SetTrigger(animation);
    }

    protected void DetectTarget()
    {
        distanceFromtarget = GetDistanceFromtarget();

        if (distanceFromtarget <= sightRadius)
        {
            OnInteractionWithTarget();
            if (TargetInSight() && !BlockedByObsticle())
            {
                noticedTarget = true;
                OnTargetDetection();
            }

            if (noticedTarget)
                FollowTarget();

            if (noticedTarget && distanceFromtarget <= attackRange)
            {
                AttackTarget();
            }

        }
        else
        {
            noticedTarget = false;
            OnExistFromSight();
        }
    }

    protected virtual void FollowTarget()
    {
        float distance = GetDistanceFromtarget();
        agent.SetDestination(target.position);
        OnFollow();
        if (distance <= agent.stoppingDistance)
        {
            OnStoppingDistance();
            FaceTarget();
        }
    }

    protected void PlayPositionAnimation(string anim, bool state)
    {
        animator.SetBool(anim, state);
    }

    protected float GetDistanceFromtarget()
    {
        return Vector3.Distance(target.position, transform.position);
    }

    protected Vector3 GetDirectionTotarget()
    {
        return (target.position - transform.position).normalized;
    }

    protected bool TargetInSight()
    {
        Vector3 vectorTotarget = target.position - transform.position;
        float angle = Vector3.Angle(transform.forward, vectorTotarget);
        if (angle <= sightAngle)
            return true;

        return false;
    }

    protected void FaceTarget()
    {
        Vector3 direction = GetDirectionTotarget();
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    protected bool BlockedByObsticle()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(transform.position, GetDirectionTotarget(), out hit, sightRadius);
        if (isHit)
        {
            // open line to target
            if (hit.transform == target)
            {
                return false;
            }
        }

        return true;
    }

    protected void AttackTarget()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(transform.position, GetDirectionTotarget(), out hit, sightRadius);
        if (isHit)
        {
            // open line to target
            if (hit.transform == target)
            { 
                OnAttack();
            }
        }
    }

    protected bool IsAlive
    {
        get { return health > 0; }
    }

    public virtual void TakeDamage(RaycastHit[] hits, Vector3 hitPosition, AmmoType ammotype) { }
    protected virtual void Die() { }
    protected virtual void OnExistFromSight() { }
    protected virtual void OnInteractionWithTarget() { }
    protected virtual void OnStoppingDistance() { }
    protected virtual void OnTargetDetection() { }
    protected virtual void OnFollow() { }
    protected virtual void OnAttack() { }
}
