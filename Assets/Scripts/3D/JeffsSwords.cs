using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JeffsSwords : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas shop;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Button swordUpgradeButton;
    [SerializeField] private Image checkImage;
    [SerializeField] private Image darkImage;
    [SerializeField] private TextMeshProUGUI swordDamageReceived;

    private int swordDamageAmount;
    private int ironCost;

    [SerializeField] private AudioSource welcomeAudioSource;
    [SerializeField] private AudioSource goodbyeAudioSource;

    [SerializeField] private TimeOfDayScript timeOfDay;

    [SerializeField] private AudioSource jeffSound;

    [SerializeField] private BuildingPositions buildingPositions;

    private void Start()
    {
        if (!buildingPositions.ArmoryBought && !buildingPositions.JeffPlayed) 
        {
            jeffSound.Play(); 
            buildingPositions.JeffPlayed = true;
        }

        if (playerStats.EasyDifficulty)
        {
            swordDamageAmount = 15;
        }
        else if (playerStats.MediumDifficulty)
        {
            swordDamageAmount = 10;
        }
        else if (playerStats.HardDifficulty)
        {
            swordDamageAmount = 5;
        }

        swordDamageReceived.text = "+" + swordDamageAmount + " sword damage";
        ironCost = 10;
    }

    private void Update()
    {
        if (shop.isActiveAndEnabled)
        {
            Time.timeScale = 0f;
        }

        if (playerStats.Metal < ironCost)
        {
            swordUpgradeButton.interactable = false;
            darkImage.gameObject.SetActive(true);
        }
        else 
        {
            swordUpgradeButton.interactable = true;
            darkImage.gameObject.SetActive(false);
        }
        // Disable button if sword damage is at the maximum limit
        if (playerStats.SwordDamage == 60)
        {
            swordUpgradeButton.interactable = false;
            checkImage.gameObject.SetActive(true);
            darkImage.gameObject.SetActive(true);
        }
    }

    public void Interact()
    {
        if (timeOfDay.TimeOfDay < 90f) 
        {
            if (shop.isActiveAndEnabled)
            {
                shop.gameObject.SetActive(false);
                goodbyeAudioSource.Play();
            }
            else
            {
                shop.gameObject.SetActive(true);
                welcomeAudioSource.Play();
            }
        }
    }

    public void ReduceIronAndAddSwordDamage()
    {

        if (playerStats.Metal >= ironCost)
        {
            playerStats.Metal -= ironCost;
            playerStats.SwordDamage += swordDamageAmount;
        }

        if (playerStats.SwordDamage > 60)
        {
            playerStats.SwordDamage = 60;
        }
        
    }
}
