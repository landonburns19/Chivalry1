using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyChase : MonoBehaviour
{
    // Speed at which the enemy chases the player
    public float speed = 3.0f;
    private float currSpeed;
    public Animator an;
    public SpriteRenderer SpriteRenderer;
    public float AwarenessDistance = 2.0f;

    private float DazedTime;
    public float startDazedTime;
    
    // Reference to the player's Transform
    private Transform player;
    private float xPos;
    private bool freezeAll = false;

    
    [Header ("Health")]
    public int maxHealth = 20;
    public int currentHealth;
    public HealthBar healthBar;

    void Start()
    {
        currSpeed = speed;
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        // Automatically find the player GameObject by tag.
        // Make sure the player GameObject has the "Player" tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }
    }

    void Update()
    {
        if(!(an.GetCurrentAnimatorStateInfo(0).IsName("Death"))){

            if(DazedTime <= 0)
            {
                currSpeed = speed;
            }
            else
            {
                currSpeed = 0;
                DazedTime -= Time.deltaTime;
            }


            an.SetBool("IsHit", false);
            // If the player exists, calculate the direction and move the enemy
            if (player != null)
            {
                // Calculate the normalized direction vector from enemy to player
                Vector3 direction = (player.position - transform.position).normalized;
                Vector2 enemyToPlayerVector = player.position - transform.position;
                if(enemyToPlayerVector.magnitude >= AwarenessDistance)
                {
                    currSpeed = 0;
                }
                else
                {
                    currSpeed = speed;
                }
                an.SetFloat("Speed", (Mathf.Abs(direction.x) + Mathf.Abs(direction.y)) * currSpeed);
                // Move the enemy toward the player
                transform.position += direction * currSpeed * Time.deltaTime;
                
            }
            Flip();
        }
    }



    // Died
    public void DeadEnemy()
    {
        if(!(an.GetCurrentAnimatorStateInfo(0).IsName("Death"))){
            healthBar.SetHealth(0);
            an.SetBool("IsHit", false);
            an.SetBool("IsAttacking", false);
            an.SetBool("IsDead", true);
        }
    }

    public void TakeDamage(int damage)
    {
        DazedTime = startDazedTime;
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Hit the Enemy");
        an.SetBool("IsHit", true);
        if (currentHealth <= 0)
        {
          DeadEnemy();
        }
    }

    private void Flip()
    {
        if (transform.position.x > xPos){
            SpriteRenderer.flipX = false;
        }
        else if (transform.position.x < xPos){
            SpriteRenderer.flipX = true;
        }

        xPos = transform.position.x;
    }

}
