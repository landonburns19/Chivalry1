using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator an;
    private float timeBAttack;
    public float startTimeBAttack;
    

    public Transform attackPos;
    public LayerMask EnemiesInRange;
    public float attackRange;
    public int damage;
    void Update()
    {
        if(timeBAttack <= 0){
        if(Input.GetKey(KeyCode.Comma))
        {
            an.SetBool("IsAttacking", true);
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, EnemiesInRange);
            for(int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<EnemyChase>().TakeDamage(damage);
            }
            
        }    
        timeBAttack = startTimeBAttack;
        }
        else
        {
            an.SetBool("IsAttacking", false);
            timeBAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
