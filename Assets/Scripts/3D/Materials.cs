using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Materials : MonoBehaviour
{
    private int health;
    private int maxHealth;

    private int materialDropQnt;
    private string materialType;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private NumberOfMaterials numberOfMaterials;

    void Start()
    {
        canvas.enabled = false;

        if (gameObject.CompareTag("Small Rock"))
        {
            materialType = "Small Rock";
            materialDropQnt = 25;
            maxHealth = 110;
        }
        else if (gameObject.CompareTag("Rock"))
        {
            materialType = "Rock";
            materialDropQnt = 40;
            maxHealth = 150;
        }
        else if (gameObject.CompareTag("Small Tree"))
        {
            materialType = "Small Tree";
            materialDropQnt = 20;
            maxHealth = 80;
        }
        else if (gameObject.CompareTag("Tree"))
        {
            materialType = "Tree";
            materialDropQnt = 35;
            maxHealth = 100;
        }

        health = maxHealth;

    }

    void Update()
    {
        if (healthText != null)
        {
            healthText.text = health + "/" + maxHealth;
        }

        float healthRatio = (float)health / (float)maxHealth;
        healthBarImage.fillAmount = healthRatio;

        if (health <= 0)
        {
            Death();
        }

    }

    public void TakeDamage(int damage)
    {

        if(canvas.enabled == false) 
        {
            canvas.enabled = true;
        }
        health -= damage;
    }

    private void Death()
    {
        if (materialType == "Small Rock") 
        {
            playerStats.Rock += materialDropQnt;
            numberOfMaterials.SmallRock--;
        }
        else if(materialType == "Rock")
        {
            playerStats.Rock += materialDropQnt;
            numberOfMaterials.Rock--;
        }
        else if(materialType == "Small Tree")
        {
            playerStats.Wood += materialDropQnt;
            numberOfMaterials.SmallTree--;
        }
        else if(materialType == "Tree")
        {
            playerStats.Wood += materialDropQnt;
            numberOfMaterials.Tree--;
        }

        Destroy(gameObject);      
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hospital") || other.CompareTag("Building") || other.CompareTag("Armory") || other.CompareTag("Forge") || other.CompareTag("Mine"))
        {
            TakeDamage(maxHealth);
        }
    }
}
