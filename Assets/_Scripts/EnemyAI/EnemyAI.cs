using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Vector3 startPos;
    Quaternion startRotation;

    float smooothRotationTime = 3f;

    [SerializeField] FieldOfView fieldOfView;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] float stoppingDistance = 1f;

    public float hitPoints;
    public float maxHitPoints = 5;
    public HealthBarBehaviour Healthbar;
    public Animator anim;

    private void Start()
    {
        anim.GetComponent<Animator>();
        hitPoints = maxHitPoints;
        Healthbar.SetEnemyHealth(hitPoints, maxHitPoints);

        startPos = transform.position;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetDirection(transform.forward);

        Destination();

        if (agent.remainingDistance <= .1f)
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * smooothRotationTime);
        
    }

    private void Destination()
    {
        var destination = Vector3.zero;

        if (fieldOfView.IsTarget)
        {
            anim.SetBool("Running", true);

            if (hitPoints > 0)
            {
                destination = target.position;
            }
            agent.stoppingDistance = stoppingDistance;

            if (agent.remainingDistance <= stoppingDistance)
            {
                anim.SetBool("Attack", true);
            }
            else
            {
                anim.SetBool("Attack", false);
            }
        }
        else
        {
            destination = startPos;
            agent.stoppingDistance = 0;
        }

        if (transform.position == startPos)
        {
            anim.SetBool("Running", false);
            anim.SetTrigger("Idle");
        }

        agent.SetDestination(destination);
    }
    public void TakeHit(float damage)
    {
        hitPoints -= damage;
        Healthbar.SetEnemyHealth(hitPoints, maxHitPoints);

        if (hitPoints <= 0)
        {
            anim.SetTrigger("Death");
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        StopCoroutine(Death());
    }
}