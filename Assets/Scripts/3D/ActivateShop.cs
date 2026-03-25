using UnityEngine;

public class ActivateShop : MonoBehaviour
{
    [SerializeField] private GameObject shopUI;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private AudioSource audioSource;

    private bool isShopActive;

    private void Start()
    {
        isShopActive = false;
        shopUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !playerStats.BossDead)
        {
            ToggleShop();
        }
    }

    public void ToggleShop()
    {
        audioSource.Play();
        isShopActive = !isShopActive;

        if (isShopActive)
        {
            Time.timeScale = 0;

            // Activate the shop UI
            shopUI.SetActive(true);
        }
        else
        {
            // Deactivate the shop UI
            shopUI.SetActive(false);
        }
    }

}
