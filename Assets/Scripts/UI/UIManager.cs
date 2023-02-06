using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : BaseGameObject
{

    public Text lifeText;

    public Text timerText;

	public Text gameOverText;

    public Text correctEnemies;

    public Text remainingEnemies;



    void OnEnable()
    {
        base.OnEnable();
        GameManager.Tick += OnTick;
        GameManager.GameEnded += OnGameEnded;
        FPSPlayerController.LifeUpdate += OnLifeUpdate;
        PuzzleSolver.CorrectEnemy += OnCorrectEnemy;
        PuzzleSolver.NewStage += OnNewStage;
    }

    void OnDisable()
    {
        base.OnDisable();
        GameManager.Tick -= OnTick;
        GameManager.GameEnded -= OnGameEnded;
        FPSPlayerController.LifeUpdate -= OnLifeUpdate;
        PuzzleSolver.CorrectEnemy -= OnCorrectEnemy;
        PuzzleSolver.NewStage -= OnNewStage;
    }

    void Awake()
    {
        timerText.text = "";
        lifeText.text = "";
		gameOverText.enabled = false;
    }

    private void OnTick(float tickSeconds)
    {
        int minutes = Mathf.FloorToInt(tickSeconds / 60);
        int seconds = Mathf.FloorToInt(tickSeconds % 60);
        timerText.text = string.Format("Time remaining: {0}:{1:00}", minutes, seconds);
    }

    private void OnLifeUpdate(int newLife)
    {
        lifeText.text = string.Format("Life: {0}", newLife);
    }

    protected override void OnGameEnded(bool success)
    {
        base.OnGameEnded(success);
        if(success){
			gameOverText.text = @"SYSTEM OVERRIDEN. CONGRATULATIONS,
THREAD NUMBER 41.

INPUT 'R' TO RESTART.
INPUT 'Q' TO QUIT.";
		}
		else{
			gameOverText.text = @"SYSTEM OVERRIDE COMPROMISED.
YOU'VE FAILED.

INPUT 'R' TO RESTART.
INPUT 'Q' TO QUIT.";
		}
        StartCoroutine(RestartCoroutine());
        gameOverText.enabled = true;
    }

    private void OnCorrectEnemy(int enemyNumber){
        correctEnemies.text = enemyNumber.ToString();
    }

    private void OnNewStage(int numEnemies){
        remainingEnemies.text = numEnemies.ToString();
        correctEnemies.text = "0";
    }

    private IEnumerator RestartCoroutine(){
        YieldInstruction waitFrame = new WaitForEndOfFrame();

        while(true){
            if(Input.GetKey(KeyCode.Q)){
                Application.Quit();
            }
            else if(Input.GetKey(KeyCode.R)){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            yield return waitFrame;
        }
    }
}
