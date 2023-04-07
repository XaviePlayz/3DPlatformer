using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackField : MonoBehaviour
{
    public Player player;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.Health -= 10;
        }
    }
}
