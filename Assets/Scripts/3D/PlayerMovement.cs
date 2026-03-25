using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    [SerializeField] private bool isAttacking;

    [SerializeField] Transform attackCenter;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask enemyLayers;

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource easterEggSound;
    [SerializeField] private AudioSource rockSound;
    [SerializeField] private AudioSource woodSound;

    private CameraSwitch cameraSwitch;

    private float nextAttackTimer;

    private float healthIncreaseRate;
    private float healthIncreaseCooldown;

    private void Start()
    {
        cameraSwitch = GetComponent<CameraSwitch>();
        attackRange = 1f;
        nextAttackTimer = 0f;
        healthIncreaseRate = 5f;
        healthIncreaseCooldown = 0f;
    }

    void Update()
    {
        if (healthIncreaseCooldown > 0)
        {
            healthIncreaseCooldown -= Time.deltaTime;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = transform.right * horizontalInput + transform.forward * verticalInput;

        if (!isAttacking)
        {
            controller.Move(movementDirection.normalized * speed * Time.deltaTime);
        }
        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 10f * Time.deltaTime);
        }
        if (movementDirection.magnitude > 0 && Time.timeScale != 0)
        {
            animator.SetFloat("Speed", speed);
            if (!walkingSound.isPlaying )
            {
                walkingSound.Play();
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
            if (walkingSound.isPlaying)
            {
                walkingSound.Stop();
            }
        }
        if (horizontalInput > 0)
        {
            animator.SetFloat("hInput", 1);
            speed = 3f;
        }
        else if (horizontalInput < 0)
        {
            animator.SetFloat("hInput", -1);
            speed = 3f;
        }
        else
        {
            animator.SetFloat("hInput", 0);
        }
        if (verticalInput > 0)
        {
            animator.SetFloat("vInput", 1);
            speed = 5f;
        }
        else if (verticalInput < 0)
        {
            animator.SetFloat("vInput", -1);
            speed = 5f;
        }
        else
        {
            animator.SetFloat("vInput", 0);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            easterEggSound.Play(); // Play the easterEggSound when triggering the animation
            if (cameraSwitch.isFirstPersonActive())
            {
                cameraSwitch.setFirstPersonActive(false);
            }
            animator.SetTrigger("EasterEgg");
        }

        // Check if the "EasterEgg" animation has stopped
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("EasterEgg") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            if (!easterEggSound.isPlaying)
            {
                easterEggSound.Play();
            }
        }
        else
        {
            if (easterEggSound.isPlaying)
            {
                easterEggSound.Stop();
            }
        }


        if (Time.time >= nextAttackTimer) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTimer = Time.time + 1f;
            }
        }
        if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f)
        || (animator.GetCurrentAnimatorStateInfo(0).IsName("PickingUp") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    private void Attack()
    {
        if (Random.value < 0.5f)
        {
            attackSound.Play();
        }

        animator.SetTrigger("Attack");

        Collider[] hitEnemies = Physics.OverlapSphere(attackCenter.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            StartCoroutine(DamageAfterDelay(enemy.GetComponent<Materials>(), playerStats.SwordDamage, 0.5f));
            if (enemy != null && (enemy.CompareTag("Rock") || enemy.CompareTag("Small Rock")))
            {
                rockSound.Play();
            }
            if (enemy != null && (enemy.CompareTag("Tree") || enemy.CompareTag("Small Tree"))) 
            {
                woodSound.Play();
            }
        }
    }

    private IEnumerator DamageAfterDelay(Materials enemyMaterial, int damage, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyMaterial.TakeDamage(damage);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRange);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hospital") && healthIncreaseCooldown <= 0f && playerStats.Health < 100)
        {
            playerStats.Health++;
            healthIncreaseCooldown = 1f / healthIncreaseRate;
        }
    }
}
