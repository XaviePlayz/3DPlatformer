using UnityEngine;

public class SpawnAmmoAfterDeathAction : AfterDeathAction
{
    public GameObject ammoPrefab;

    public Vector3 offset;

    public override void Execute(Vector3 spawnPosition)
    {
        Instantiate(ammoPrefab, spawnPosition + offset, Quaternion.identity);
    }
}