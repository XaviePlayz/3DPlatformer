using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{
	public float startSeconds = 90f;

	public float timePenalty = 2f;

	private static bool paused = false;

	private static bool gameOver = false;

	public static event Action<float> Tick;

	public static event Action GamePaused;

	public static event Action<bool> GameEnded;

	public static event Action GameResumed;

	private static GameManager _instance;

	public static void Pause ()
	{
		if (GamePaused != null)
			GamePaused ();
	}

	public static void Resume ()
	{
		if (GameResumed != null)
			GameResumed ();
	}

	public static void EndGame (bool success)
	{
		if (GameEnded != null)
			GameEnded (success);
		gameOver = true;
	}

	void Awake(){
		gameOver = false;
		paused = false;
		_instance = this;
	}

	void Start(){
		if(Tick != null)
			Tick(startSeconds);
		StartCoroutine(TickCoroutine());
	}

	// void Update ()
	// {
	// 	if (!gameOver) {
	// 		if (Input.GetKeyDown (KeyCode.Escape)) {
	// 			if (!paused) {
	// 				Pause ();
	// 			} else {
	// 				Resume ();
	// 			}
	// 			paused = !paused;
	// 		}
	// 	}
	// }

	public static void DiminishTimer(){
		_instance.startSeconds = Mathf.Clamp(_instance.startSeconds -_instance.timePenalty, 0f, _instance.startSeconds);
		if(_instance.startSeconds <= 0){
			EndGame(false);
		}
	}

	private IEnumerator TickCoroutine()
	{
		YieldInstruction sleepASecond = new WaitForSeconds(1f);
		while(startSeconds > 0f && !gameOver){
			if(Tick != null)
				Tick(startSeconds);
			startSeconds = Mathf.Clamp(startSeconds - 1f, 0f, startSeconds);
			yield return sleepASecond;
			if(_instance.startSeconds <= 0){
				EndGame(false);
			}
		}
	} 
}
