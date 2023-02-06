using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : BaseGameObject
{
    public Transform[] spawnPoints;

    public GameObject enemyPrefab;

    public GameObject lifePickupPrefab;

    public GameObject puzzlePieceSpawnerPrefab;

    public GameObject puzzlePieceParticles;

    private Dictionary<PuzzlePiece, int> _enemiesAlive;

    private Dictionary<PuzzlePiece, int> _enemiesRestriction;

    private Coroutine _spawnCoroutine = null;



    public void Spawn(PuzzlePiece[] levelRestrictions)
    {
        _enemiesRestriction = new Dictionary<PuzzlePiece, int>();
        _enemiesAlive = new Dictionary<PuzzlePiece, int>();

        foreach (PuzzlePiece pPiece in levelRestrictions)
        {
            if (!_enemiesRestriction.ContainsKey(pPiece))
            {
                _enemiesRestriction.Add(pPiece, 1);
                _enemiesAlive.Add(pPiece, 1);
            }
            else
            {
                _enemiesRestriction[pPiece]++;
                _enemiesAlive[pPiece]++;
            }
            SpawnPuzzleEnemies();
            SpawnSimpleEnemies();
        }

        if(_spawnCoroutine == null){
            StartCoroutine(SpawnCoroutine());
        }
    }

    private void SpawnPuzzleEnemies()
    {
        foreach (PuzzlePiece pPiece in _enemiesRestriction.Keys)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[ChooseSpawnPoint()].position, Quaternion.identity);
            GameObject puzzlePc = Instantiate(puzzlePieceSpawnerPrefab, enemy.transform.position, Quaternion.identity);
            GameObject pieceParticles = Instantiate(puzzlePieceParticles, enemy.transform.position, Quaternion.identity);
            ExecutePuzzleStepAfterDeathAction puzzleAfterAction;
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            ParticleSystem.MainModule particlesMainModule = pieceParticles.GetComponent<ParticleSystem>().main;
            particlesMainModule.startColor = pPiece.firewallRule;
            pieceParticles.transform.parent = enemy.transform;
            puzzlePc.transform.parent = enemy.transform;
            puzzlePc.transform.localPosition = Vector3.zero;
            puzzleAfterAction = puzzlePc.GetComponent<ExecutePuzzleStepAfterDeathAction>();
            puzzleAfterAction.piece = pPiece;
            enemyController.afterDeath = puzzleAfterAction;
        }
    }

    private void SpawnSimpleEnemies(){
        int num = 2;
        for(int e=0; e<num; e++){
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[ChooseSpawnPoint()].position, Quaternion.identity);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            SpawnLifeAfterDeathAction healthAfterAction = enemy.AddComponent<SpawnLifeAfterDeathAction>();
            healthAfterAction.lifePrefab = lifePickupPrefab;
            enemyController.afterDeath = healthAfterAction;
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        YieldInstruction waitSeconds = new WaitForSeconds(5f);

        yield return waitSeconds;

        while (true && !gameEnded && !gamePaused)
        {
            yield return waitSeconds;
            SpawnPuzzleEnemies();
        }
    }

    private int ChooseSpawnPoint()
    {
        // int index = 0;
        // float minimumAngle = 60f;

        // for (int sp = 0; sp < spawnPoints.Length; sp++)
        // {
        //     Vector3 playerSpawnPointVector = spawnPoints[sp].position - FPSPlayerController.FPSPlayerInstance.transform.position;
        //     float angle = Vector3.Angle(FPSPlayerController.FPSPlayerInstance.transform.forward, playerSpawnPointVector);

        //     if (angle > minimumAngle)
        //     {
        //         index = sp;
        //     }
        // }
        // return index;

        return Mathf.FloorToInt(Random.value * spawnPoints.Length);
    }
}