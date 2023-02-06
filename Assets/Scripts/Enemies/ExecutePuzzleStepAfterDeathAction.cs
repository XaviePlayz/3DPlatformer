using UnityEngine;

public class ExecutePuzzleStepAfterDeathAction : AfterDeathAction
{
    public PuzzlePiece piece;    


    public override void Execute(Vector3 spawnPosition)
    {
        PuzzleSolver.PuzzleSolverInstance.PutPiece(piece);
    }
}