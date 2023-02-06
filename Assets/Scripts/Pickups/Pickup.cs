using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class Pickup : MonoBehaviour {	

	private SphereCollider _collider;

	private Rigidbody _rigidbody;



	void OnTriggerEnter(Collider other){
		FPSPlayerController playerController = other.GetComponent<FPSPlayerController>();
		if(playerController != null){
			OnContactDo();			
		}
	}

	void Update(){
		transform.position += Time.deltaTime * new Vector3(0f, 0f, 0.5f*Mathf.Sin(Time.time));
		transform.Rotate(0f, 0f, Time.deltaTime * 15f);
	}

	protected virtual void OnContactDo(){}
}
