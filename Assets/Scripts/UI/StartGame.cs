using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	public Text instructions;

	public Button startButton;

	public Text title;

	public void Breach(){
		startButton.gameObject.SetActive(false);
		title.enabled = false;
		instructions.enabled = true;
		StartCoroutine(WaitInput());
	}

	private IEnumerator WaitInput(){
		while(true){
			yield return new WaitForEndOfFrame();
			if(Input.anyKey){
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}
	}
}
