using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameObject : MonoBehaviour
{
    [HideInInspector]
    public bool gamePaused = false;

    [HideInInspector]
    public bool gameEnded = false;

    [HideInInspector]
    public bool gameResumed = false;

    protected void OnEnable()
    {
        GameManager.GamePaused += OnGamePaused;
        GameManager.GameEnded += OnGameEnded;
        GameManager.GameResumed += OnGameResumed;
    }

    protected void OnDisable()
    {
        GameManager.GamePaused -= OnGamePaused;
        GameManager.GameEnded -= OnGameEnded;
        GameManager.GameResumed -= OnGameResumed;
    }

    protected virtual void OnGamePaused()
    {
        gamePaused = true;
    }

    protected virtual void OnGameEnded(bool success)
    {
        gameEnded = true;
    }

    protected virtual void OnGameResumed()
    {
        gamePaused = false;
    }
}
