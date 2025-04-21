using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator an;
    private float timeBAttack;
    public float startTimeBAttack;
    

    public Transform attackPos;
    public LayerMask EnemiesInRange;
    public float attackRange;
    public int damage;

    public bool isWaiting = false;



    
    public IEnumerator Wait(int time)
    {
        isWaiting = true;
        yield return new WaitForSeconds(time);
        isWaiting = false;
    }
    
    void Update()
    {
        if(!(an.GetCurrentAnimatorStateInfo(0).IsName("Death"))){
            if(timeBAttack <= 0){
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, EnemiesInRange);
            if(enemiesToDamage.Length  > 0 )
            {
                if(!isWaiting){
                    an.SetBool("IsAttacking", true);
                    enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, EnemiesInRange);
                    for(int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<PlayerMovement>().EnemyDMG(damage);
                    }
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
    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
