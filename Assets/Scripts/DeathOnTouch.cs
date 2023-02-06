using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnTouch : MonoBehaviour {

	void OnTriggerEnter(){
		GameManager.EndGame(false);
	}
}
