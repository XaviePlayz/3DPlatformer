using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Stats")]
public class Stats : ScriptableObject
{
    public static Stats Instance;
    public float HP;
    public float moveSpeed;
    public float jumpHeight;
}
