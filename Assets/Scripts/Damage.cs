using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public PlayerMovement playerMovement;
    public int doDamage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerMovement.EnemyDMG(doDamage);

        }
    }
}
