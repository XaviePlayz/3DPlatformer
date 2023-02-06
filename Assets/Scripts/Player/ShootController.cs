using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {
	
	public Transform shotOrigin;

	public float timeBetweenShots;

	public float bulletSpeed;

	public float bulletDamage;

	private float nextShotTime;

	public AudioSource audioSource;

	public AudioClip shotClip;

	public LineRenderer shotLine;


	void Awake(){
		nextShotTime = 0f;
	}
	public void Shoot(){
		if(Time.time >= nextShotTime){
			nextShotTime = Time.time + timeBetweenShots;
			audioSource.PlayOneShot(shotClip);
			StartCoroutine(BulletCoroutine());
		}
	}

	private IEnumerator BulletCoroutine(){
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2f, Screen.height/2f));
		RaycastHit raycastHit;
		YieldInstruction endOfFrame = new WaitForEndOfFrame();

		shotLine.SetPosition(0, shotOrigin.position);
		shotLine.SetPosition(1, shotOrigin.position + shotOrigin.forward * bulletSpeed);		
		shotLine.enabled = true;

		while(!Physics.Raycast(ray.origin, ray.direction, out raycastHit, bulletSpeed, ~LayerMask.GetMask("Player", "Weapon"))){
			yield return endOfFrame;
			shotLine.SetPosition(0, ray.origin);
			ray.origin = ray.origin + ray.direction.normalized * bulletSpeed;			
			shotLine.SetPosition(1, ray.origin);
		}

		shotLine.enabled = false;
		shotLine.SetPosition(0, shotOrigin.position);
		shotLine.SetPosition(1, shotOrigin.position);

		EnemyController enemy = raycastHit.transform.GetComponent<EnemyController>();
		if(enemy != null){
			enemy.Damage(bulletDamage);
		}
	}	
}
