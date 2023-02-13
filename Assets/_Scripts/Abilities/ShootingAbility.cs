using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Ability/ShootAbility")]
public class ShootingAbility : Ability
{
    [SerializeField] private GameObject _bullet;
    public override void Use(PlayerMovement player)
    {
        Instantiate(_bullet, player.transform.position, player.transform.rotation);
        
    }
}
