using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Ability")]
public abstract class Ability : ScriptableObject
{
    public abstract void Use(PlayerMovement player);
}
