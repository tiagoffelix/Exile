using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Animator animator;

    private int health;
    private int maxHealth;
    private int damage;

    private bool isAttacking;

    public bool isBlocking;
    public bool undead;

    private float cooldownFirstStage;
    private float cooldownTimerFirstStage;

    private float cooldownSecondStage;
    private float cooldownTimerSecondStage;

    private bool firstStage;
    private bool secondStage;

    [SerializeField] private Transform attackCenter;
    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private GameObject player;

    [SerializeField] private PlayerStats playerStats;

    private float speed;

    [SerializeField] private float chaseDistance, attackRange;

    [SerializeField] private Image healthBarImage;
    [SerializeField] private Canvas canvas;

    private float blockCooldown;
    private float blockCooldownTimer;

    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource fakeDeathSound;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource rebornSound;
    [SerializeField] private AudioSource walkingSound;

    void Start()
    {  
        gameObject.tag = "Enemy";
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");

        if (playerStats.EasyDifficulty)
        {
            maxHealth = 900;
            damage = 30;
        }
        else if (playerStats.MediumDifficulty)
        {
            maxHealth = 1100;
            damage = 40;
        }
        else if (playerStats.HardDifficulty)
        {
            maxHealth = 1500;
            damage = 50;
        }

        health = maxHealth;
        cooldownFirstStage = 1.5f;
        cooldownSecondStage = 3.8f;
        speed = 2f;
        chaseDistance = 8f;
        attackRange = 2f;
        blockCooldown = 2f;
        isAttacking = false;
        isBlocking = false;
        undead = false;
        firstStage = true;
        secondStage = false;
}

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Undead") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            undead = false;
            rebornSound.Play();
        }

        if(firstStage) 
        {
            if (cooldownTimerFirstStage > 0) { cooldownTimerFirstStage -= Time.deltaTime; }
            if (cooldownTimerFirstStage < 0) { cooldownTimerFirstStage = 0; }
        }
        if (secondStage) 
        {
            if (cooldownTimerSecondStage > 0) { cooldownTimerSecondStage -= Time.deltaTime; }
            if (cooldownTimerSecondStage < 0) { cooldownTimerSecondStage = 0; }
        }

        if (blockCooldownTimer > 0) { blockCooldownTimer -= Time.deltaTime; }
        if (blockCooldownTimer < 0) { blockCooldownTimer = 0; }

        animator.SetFloat("Speed", 0);

        if (!undead)
        {
            float healthRatio = (float)health / (float)maxHealth;
            healthBarImage.fillAmount = healthRatio;

            float distance = Vector2.Distance(player.transform.position, transform.position);

            if (blockCooldownTimer == 0 && secondStage)
            {
                animator.SetBool("Block",false);
                isBlocking = false;

            }

            if (distance <= chaseDistance)
            {
                if (distance <= attackRange)
                {
                    animator.SetFloat("Speed", 0);
                    walkingSound.enabled = false;

                    if (firstStage && cooldownTimerFirstStage == 0)
                    {

                        StartCoroutine(Attack());
                        cooldownTimerFirstStage = cooldownFirstStage;

                    }
                    if (secondStage && cooldownTimerSecondStage == 0)
                    {

                        blockCooldownTimer = blockCooldown;
                        Combo();
                        cooldownTimerSecondStage = cooldownSecondStage;

                    }
                }
                else
                {
                    walkingSound.enabled = true;
                    Vector2 direction = player.transform.position - transform.position;
                    direction.Normalize();

                    if (!isAttacking && !isBlocking)
                    {
                        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                        animator.SetFloat("Speed", speed);
                    }

                    if (direction.x < 0)
                    {
                        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                    else if (direction.x > 0)
                    {
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                }
            }
            else 
            {
                walkingSound.enabled = false;
                animator.SetFloat("Speed", 0); 
            }

        }

    }

    public void TakeDamage(int damage)
    {

        health -= damage;
        animator.SetTrigger("Hit");   
        
        if(!isBlocking) {
            hitSound.Play();
        }

        if (health <= 0 && firstStage) {

            if(playerStats.EasyDifficulty || playerStats.MediumDifficulty)
            {
                playerStats.RestoreHealthAndArmor();
            }
            else if(playerStats.HardDifficulty)
            {
                playerStats.RestoreHealth();
            }

            undead = true;
            walkingSound.enabled = false;
            fakeDeathSound.Play();
            animator.SetTrigger("Undead");

            float healthRatio = (float)health / (float)maxHealth;
            healthBarImage.fillAmount = healthRatio;

            health = maxHealth;
            firstStage = false;
            secondStage = true;
            
        }

        if (health <= 0 && secondStage)
        {
            canvas.enabled = false;
            Die();
        }

    }

    public void Die()
    {
        walkingSound.enabled = false;
        animator.SetBool("Dead", true);
        deathSound.Play();

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        playerStats.RestoreHealth();

        StartCoroutine(WaitForSecondsBoss());

    }

    private IEnumerator Attack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackCenter.position, attackRange, enemyLayers);

        foreach (Collider2D player in hitPlayer)
        {
            Player Player = player.GetComponent<Player>();

            if (Player != null)
            {
                attackSound.Play();
                isAttacking = true;
                animator.SetTrigger("Attack0");
                animator.SetFloat("Speed", 0);
                Player.TakeDamage(damage);
                yield return new WaitForSeconds(1f);
                isAttacking = false;
            }
        }
    }
    private void Combo()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackCenter.position, attackRange, enemyLayers);

        foreach (Collider2D player in hitPlayer)
        {
            Player Player = player.GetComponent<Player>();

            if (Player != null)
            {
                attackSound.Play();
                isAttacking = true;
                animator.SetTrigger("Attack");
                animator.SetFloat("Speed", 0);
                Player.TakeDamage(damage); 
                
                isAttacking = false;
                animator.SetBool("Block", true);
                attackSound.Play();
                Player.TakeDamage(damage);
                isBlocking = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRange);
    }

    IEnumerator WaitForSecondsBoss()
    {
        yield return new WaitForSeconds(2f);

        playerStats.BossDead = true;

    }

}
