using UnityEngine;

public class SpawnLifeAfterDeathAction : AfterDeathAction
{
    public GameObject lifePrefab;

    public Vector3 offset;

    public override void Execute(Vector3 spawnPosition)
    {
        if(Random.value > 0.7f)
            Instantiate(lifePrefab, spawnPosition + offset, lifePrefab.transform.rotation);
    }
}