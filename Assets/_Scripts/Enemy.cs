using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Stats
{
    [SerializeField] private Stats _stats;

    private void Update()
    {
        Debug.Log(_stats.HP+" "+_stats.moveSpeed);
    }
}
