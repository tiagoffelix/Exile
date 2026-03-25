using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Animator animator;

    private int health;
    private int maxHealth;
    private int damage;

    private bool isAttacking;
    private float cooldown;
    private float cooldownTimer;

    [SerializeField] private Transform attackCenter;
    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private GameObject player;

    [SerializeField] private PlayerStats playerStats;

    private float speed;

    private float chaseDistance , attackRange;

    [SerializeField] private Image healthBarImage;
    [SerializeField] private Canvas canvas;

    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource spawnSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource walkingSound;

    void Start()
    {
        gameObject.tag = "Enemy";
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        speed = 2.3f; 
        chaseDistance = 3.8f;
        attackRange = 1f;
        cooldown = 1.8f;

        spawnSound.Play();

        if (playerStats.EasyDifficulty)
        {
            maxHealth = 100;
            damage = 20;
        }
        else if (playerStats.MediumDifficulty)
        {
            maxHealth = 150;
            damage = 30;
        }
        else if (playerStats.HardDifficulty)
        {
            maxHealth = 200;
            damage = 50;
        }
        health = maxHealth;
        isAttacking = false;
    }

    private void Update()
    {
        if (cooldownTimer > 0) { cooldownTimer -= Time.deltaTime; }
        if (cooldownTimer < 0) { cooldownTimer = 0; }

        animator.SetFloat("Speed", 0);

        float distance = Vector2.Distance(player.transform.position, transform.position);
   
        if (distance <= chaseDistance)
        {
            if (distance <= attackRange)
            {
                animator.SetFloat("Speed", 0);
                walkingSound.enabled = false;

                if (cooldownTimer == 0)
                {
                    StartCoroutine(Attack());
                    cooldownTimer = cooldown;
                }
            }
            else
            {

                walkingSound.enabled = true;
                Vector2 direction = player.transform.position - transform.position;
                direction.Normalize();

                if (!isAttacking)
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

    public void TakeDamage(int damage) {
    
        health -= damage;
        animator.SetTrigger("Hit");
        deathSound.Play();

        float healthRatio = (float)health / (float)maxHealth;
        healthBarImage.fillAmount = healthRatio;

        if (health <= 0)
        {
            canvas.enabled = false;
            Die();
        }

    }

    public void Die() {

        walkingSound.enabled = false;
        animator.SetBool("Dead",true);  

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        if (playerStats.EasyDifficulty || playerStats.MediumDifficulty)
        {
            playerStats.RestoreHealthAndArmor();
        }
        else if (playerStats.HardDifficulty)
        {
            playerStats.RestoreHealth();
        }

        StartCoroutine(WaitForSeconds());
  
    }

    private IEnumerator Attack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackCenter.position, attackRange, enemyLayers);

        foreach (Collider2D player in hitPlayer)
        {
            Player Player = player.GetComponent<Player>();

            if (Player != null)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
                attackSound.Play();
                animator.SetFloat("Speed", 0);
                Player.TakeDamage(damage);
                yield return new WaitForSeconds(1f);
                isAttacking = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRange);
    }
    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(2f);

        playerStats.EnemiesAlive--;

    }

}
