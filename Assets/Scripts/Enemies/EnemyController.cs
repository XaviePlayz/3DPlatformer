using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : BaseGameObject
{
    public event System.Action<EnemyController> EnemyKilled;

    public AfterDeathAction afterDeath;

    public NavMeshAgent navMeshAgent;

    public float shootingDistance;

    public float shootingAngle;

    public float timeBetweenShots;

    public float shotProbability;

    public int shotDamage;

    public float life;

    public AudioSource audioSource;

    public AudioClip shotClip;

    public Transform shotOrigin;

    public LineRenderer shotLine;

    public Light shotMuzzle;

    private EnemyState movementState;

    private EnemyState shootState;

    private float _nextShotTime;


    void Start()
    {
        movementState = new EnemyStateFollowing(this);
        shootState = new EnemyStateShooting(this);
        _nextShotTime = 0f;
        StartCoroutine(UpdateStates());
    }

    IEnumerator UpdateStates()
    {
        YieldInstruction endOfFrame = new WaitForEndOfFrame();
        while (true)
        {
            if (!gamePaused && !gameEnded)
            {
                movementState.Update();
                shootState.Update();
            }
            else{
                movementState.Stop();                
            }
            yield return endOfFrame;
        }
    }

    public void Shoot(Vector3 targetPosition)
    {
        if (Time.time >= _nextShotTime)
        {
            _nextShotTime = Time.time + timeBetweenShots;
            audioSource.PlayOneShot(shotClip);
            StartCoroutine(ShootCoroutine(targetPosition));            
        }
    }

    private IEnumerator ShootCoroutine(Vector3 targetPosition){
        Ray ray = new Ray(shotOrigin.position, (targetPosition - shotOrigin.position).normalized);
		RaycastHit raycastHit;
        float bulletSpeed = 5f;
		YieldInstruction endOfFrame = new WaitForEndOfFrame();

		shotLine.SetPosition(0, shotOrigin.position);
		shotLine.SetPosition(1, shotOrigin.position + shotOrigin.forward * bulletSpeed);		
		shotLine.enabled = true;

        shotMuzzle.enabled = true;

		while(!Physics.Raycast(ray.origin, ray.direction, out raycastHit, bulletSpeed, ~LayerMask.GetMask("Enemies"))){
			yield return endOfFrame;
			shotLine.SetPosition(0, ray.origin);
			ray.origin = ray.origin + ray.direction.normalized * bulletSpeed;			
			shotLine.SetPosition(1, ray.origin);
		}

        shotMuzzle.enabled = false;
		shotLine.enabled = false;
		shotLine.SetPosition(0, shotOrigin.position);
		shotLine.SetPosition(1, shotOrigin.position);

		FPSPlayerController player = raycastHit.transform.GetComponent<FPSPlayerController>();
		if(player != null){
			player.Damage(shotDamage);
		}
    }

    public void Damage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            afterDeath.Execute(transform.position);

            if (EnemyKilled != null)
                EnemyKilled(this);
            
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}
