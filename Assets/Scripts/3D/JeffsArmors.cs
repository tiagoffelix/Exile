using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JeffsArmors : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas shop;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Button armorUpgradeButton;
    [SerializeField] private Image darkImage;
    [SerializeField] private TextMeshProUGUI armorReceived;

    private int armorAmount;

    [SerializeField] private AudioSource welcomeAudioSource;
    [SerializeField] private AudioSource goodbyeAudioSource;

    [SerializeField] private TimeOfDayScript timeOfDay;
    [SerializeField] private AudioSource jeffSound;

    [SerializeField] private BuildingPositions buildingPositions;

    private void Start()
    {
        if (!buildingPositions.ForgeBought && !buildingPositions.JeffPlayed)
        {
            jeffSound.Play();
            buildingPositions.JeffPlayed = true;
        }
        if (playerStats.EasyDifficulty)
        {
            armorAmount = 20;
        }
        else if (playerStats.MediumDifficulty)
        {
            armorAmount = 10;
        }
        else if (playerStats.HardDifficulty)
        {
            armorAmount = 5;
        }

        armorReceived.text = "+"+armorAmount + "armor";

    }

    private void Update()
    {
        if (playerStats.Armor == 100)
        {
            armorUpgradeButton.interactable = false;
            darkImage.gameObject.SetActive(true);
        }
        else 
        {
            armorUpgradeButton.interactable = true;
            darkImage.gameObject.SetActive(false);
        }

        if (shop.isActiveAndEnabled)
        {
            Time.timeScale = 0f;
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
                welcomeAudioSource.Play();
                shop.gameObject.SetActive(true);
            }
        }
    }

    public void ReduceIronAndAddArmor()
    {
        int ironCost = 5;

        if (playerStats.Metal >= ironCost)
        {
            playerStats.Metal -= ironCost;
            playerStats.AddArmor(armorAmount);
        }
    }

}
