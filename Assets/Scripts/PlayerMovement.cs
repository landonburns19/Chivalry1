using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 5f;
    public SpriteRenderer SpriteRenderer;
    public Animator an;
    Rigidbody2D rb;

    


    [Header("Tile Maps")]
    public Tilemap items;
    public Tilemap damage;
    public Tilemap hole;
    public Tilemap trap;
    


    [Header ("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;   

    private Vector2 movement;
    private float pHW;
    private float xPos;


    [Header ("Game Over")]
    public GameOver gameOver;
    public Timer timer;

    
    private bool freezeMovement = false;
    private bool isWaiting = false;


    private bool jump = false;
    private bool fall = true;
    // Start is called before the first frame update
    void Start()
    {
        pHW = SpriteRenderer.bounds.extents.x;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }




    // Update is called once per frame
    void Update()
    {
        if(timer.remainingTime < 0)
        {
            win();
        }
        doMovement();
        Flip();
    }


    // Died
    public void lost()
    {
        freezeMovement = true;
        an.SetBool("IsDead", true);
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (taggedObjects != null && taggedObjects.Length > 0){
            foreach (GameObject taggedObject in taggedObjects)
            {
                taggedObject.GetComponent<EnemyChase>().speed = 0;
                taggedObject.GetComponent<EnemyAttack>().isWaiting = true;
                taggedObject.GetComponent<EnemyAttack>().an.SetBool("IsAttacking", false);
            }
        }
        gameOver.Setup();
    }

    public void win()
    {
        freezeMovement = true;
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (taggedObjects != null && taggedObjects.Length > 0){
            foreach (GameObject taggedObject in taggedObjects)
            {
                taggedObject.GetComponent<EnemyChase>().speed = 0;
                taggedObject.GetComponent<EnemyAttack>().isWaiting = true;
                taggedObject.GetComponent<EnemyAttack>().an.SetBool("IsAttacking", false);
            }
        }
        gameOver.Setup();
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

    public IEnumerator waitToFreeze()
    {
        yield return new WaitForSeconds(0.1f);
        freezeMovement = true;
        yield return new WaitForSeconds(1f);
        freezeMovement = false;
    }
    public IEnumerator TakeDamage(int amount, int time)
    {
        isWaiting = true;
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
        yield return new WaitForSeconds(time);
        isWaiting = false;
        if(currentHealth <= 0)
        {
            lost();
        }
    }

    public void EnemyDMG(int amount)
    {
        // not working completely 
        an.SetBool("IsDamaged", true);
        if(!isWaiting){
            StartCoroutine(TakeDamage(amount, 1));
        }
        if(currentHealth <= 0)
        {
            lost();
        }
    }

    public IEnumerator fallInHole()
    {
        StartCoroutine(waitToFreeze());
        float duration = 0.5f;
        float startScale = 1f;
        float targetScale = 0.01f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(new Vector3(startScale, startScale, 1f), new Vector3(targetScale, targetScale, 1f), t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(targetScale, targetScale, 1f); // Ensure we reach the target scale
        an.SetBool("IsDamaged", true);
        if(!isWaiting){
            StartCoroutine(TakeDamage(20, 2));
            transform.position = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);
            freezeMovement = false;
        }
        
    }


    private void doMovement(){
        transform.localScale = new Vector3(1f, 1f, 1f);
        float inputx = Input.GetAxis("Horizontal");
        float inputy = Input.GetAxis("Vertical");
        if(freezeMovement)
        {
            inputx = 0;
            inputy = 0;
        }
        movement.x = inputx * speed * Time.deltaTime;
        movement.y = inputy * speed * Time.deltaTime;
        transform.Translate(movement);
        if(inputx != 0 || inputy != 0)
        {
            an.SetBool("IsRunning", true);
            
        }
        else{
            an.SetBool("IsRunning", false);
        }

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            an.SetBool("IsJumping", true);
        }
        else
        {
            jump = false;
            an.SetBool("IsJumping", false);
        }



        // if the player is jumping (necessary due to delay time)
        if(!(an.GetCurrentAnimatorStateInfo(0).IsName("Jump")) && hole.GetTile(hole.WorldToCell(transform.position)))
        {
            StartCoroutine(fallInHole());
        }
        


        // if the player walks over damage tile
        if(damage.GetTile(damage.WorldToCell(transform.position)) && !(an.GetCurrentAnimatorStateInfo(0).IsName("Jump")))
        {
            an.SetBool("IsDamaged", true);

            if(!isWaiting){
                StartCoroutine(TakeDamage(10, 1));
            }
        }
        else
        {
            an.SetBool("IsDamaged", false);
        }

    }
}


/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = 5f;
    public SpriteRenderer SpriteRenderer;
    public Animator an;
    RigidBody2D rb;
     private bool isFacingRight = true;

    private Vector movement;
    private Vector2 bounds;
    private float pHW;
    private float xPos;

    // Start is called before the first frame update
    void Start()
    {
        pHW = SpriteRenderer.bounds.extents.x;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        doMovement();
        Flip();
    }

     private void Flip()
     {
        if (transform.position.x > xPos){
            SpriteRenderer.flipX = false;
        }
        else if (transform.position.x < xPos){
            SpriteRenderer.flipX = true;
        }

        xPos = tansform.position.x;
     }

     private void doMovement(){
        float input = Input.GetAxis("Horizontal");
        movement.x = input * speed * Time.deltaTime;
        transform.Translate(movement);
        if(input != 0 )
        {
            an.SetBool("isRunning", true);
        }
        else{
             an.SetBool("isRunning", false);
        }
     }
}
*/