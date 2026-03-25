using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float inputHorizontal;
    private float inputVertical;

    private float walkSpeed;
    private float speedLimiter;

    private float cooldown;
    private float cooldownTimer;

    private bool isBlocking;
    private bool isAttacking;

    private float blockingTime;

    [SerializeField] private Transform attackCenter;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource shieldedSound;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private AudioSource parrySound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        gameObject.tag = "Player";

        walkSpeed = 3f;
        speedLimiter = 1f;
        attackRange = 0.8f;
        cooldown = 0.8f;
        blockingTime = 0f;
        isBlocking = false;
        isAttacking = false;
    }

    private void Update()
    {
        GetInput();

        if(cooldownTimer > 0) { cooldownTimer -= Time.deltaTime; }
        if(cooldownTimer < 0) { cooldownTimer = 0; }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            isAttacking = true;
        }
        else 
        {
            isAttacking = false;
        }

        if (Input.GetMouseButtonDown(0) && cooldownTimer == 0)
        {
            Attack();
        }

        if (Input.GetMouseButton(1) && !isAttacking)
        {
            blockingTime += Time.deltaTime;
            animator.SetBool("Block", true);
            isBlocking = true;
        }
        else
        {
            blockingTime = 0f;
            animator.SetBool("Block", false);
            isBlocking = false;
        }

        if (playerStats.Health <= 0)
        {
            Death();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        Vector2 inputVector = new Vector2(inputHorizontal, inputVertical).normalized;

        float movementSpeed = inputVector.magnitude;

        animator.SetFloat("Speed", movementSpeed);

        if (isBlocking)
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
            walkingSound.enabled = false;
            return;
        }

        inputVector *= (inputVector.magnitude > 0 && inputVector.magnitude < 1) ? inputVector.magnitude : speedLimiter;

        rb.velocity = inputVector * walkSpeed;

        FlipSprite();

        if (movementSpeed > 0)
        {
            walkingSound.enabled = true;
            walkingSound.pitch = 0.8f;
        }
        else
        {
            walkingSound.enabled = false;
        }
    }


    private void FlipSprite()
    {
        if (inputHorizontal < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -0.7f;
            transform.localScale = scale;
        }
        else if (inputHorizontal > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = 0.7f;
            transform.localScale = scale;
        }
    }

    private void Death() {

        animator.SetFloat("Speed", 0);
        walkingSound.Stop();
        rb.velocity = Vector2.zero;

        animator.SetBool("Dead", true);
        deathSound.Play();

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        playerStats.Dead = true;
    }

    private void Attack() {

        attackSound.Play();
        animator.SetTrigger("Attack");
        cooldownTimer = cooldown;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCenter.position,attackRange,enemyLayers);

        foreach (Collider2D enemy in hitEnemies) {

            if (!playerStats.BossFight) { enemy.GetComponent<Enemy>().TakeDamage(playerStats.SwordDamage); }
            else if (!enemy.GetComponent<Boss>().undead && !enemy.GetComponent<Boss>().isBlocking)
            {         
                enemy.GetComponent<Boss>().TakeDamage(playerStats.SwordDamage); 
            }   

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        if (blockingTime < 0.2 && blockingTime > 0)
        {
            parrySound.Play();
            animator.SetTrigger("Blocked");
        }
        else 
        {
            if (isBlocking)
            {
                shieldedSound.Play();
                animator.SetBool("Block", false);

                int damageShielded = damage / 3;

                playerStats.TakeDamage(damageShielded);

            }
            else
            {
                hurtSound.Play();
                animator.SetTrigger("Hurt");
                playerStats.TakeDamage(damage);
            }  
         }
        
        }
}
