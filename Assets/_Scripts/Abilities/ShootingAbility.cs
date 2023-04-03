using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Ability/ShootAbility")]
public class ShootingAbility : Ability
{
    public static ShootingAbility Instance;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioClip magicWandBlastShot;

    public override void Use(PlayerMovement player)
    {
        var fireball = Instantiate(
            projectilePrefab, 
            Player.Instance._bulletSpawn.transform.position, 
            Player.Instance._bulletSpawn.transform.rotation);
        AudioPool.Instance.PlayAudio(magicWandBlastShot);
    }
}
