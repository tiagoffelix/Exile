using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu2D : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;

    [SerializeField] private GameObject materialCanvas;
    [SerializeField] private GameObject controlsCanvas;

    private bool isPaused;

    [SerializeField] private GameObject nightEndCanvas;
    [SerializeField] private TextMeshProUGUI nightText;
    [SerializeField] private GameObject nightEndCamera;

    [SerializeField] private GameObject bossEndCanvas;
    [SerializeField] private TextMeshProUGUI bossText;

    [SerializeField] private Button pauseButton;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject bossFailedCanvas;

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TimeOfDayScript timeOfDay;

    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource lossSound;

    [SerializeField] private Player player;

    [SerializeField] private Narrator narrator;

    [SerializeField] private AudioSource firstNightSound;

    [SerializeField] private AudioSource BossRetrySound;
    [SerializeField] private AudioSource SecondBossRetrySound;

    private void Start()
    {
        isPaused = false;

        if (narrator.FirstNight)
        {
            firstNightSound.Play();
            narrator.FirstNight = false;
        }

        playerStats.HealthRetry = playerStats.Health;
        playerStats.ArmorRetry = playerStats.Armor;
    }
    void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            Cursor.visible = false;
        }

        if (materialCanvas.activeSelf && !isPaused) {

            controlsCanvas.SetActive(isPaused);

        }

        if (Input.GetKeyDown(KeyCode.Escape) && playerStats.EnemiesAlive > 0)
        {
            TogglePauseMenu();
        }

        if(playerStats.EnemiesAlive == 0)
        {
            pauseButton.gameObject.SetActive(false);
            materialCanvas.SetActive(false);
            pauseMenuCanvas.SetActive(false);
            nightEndCanvas.SetActive(true);
            nightText.text = "Night " + playerStats.Night + " completed";
            nightEndCamera.SetActive(true);
            timeOfDay.IsNight = false;
        }

        if (playerStats.Health == 0 && !playerStats.BossFight) 
        {
            pauseButton.gameObject.SetActive(false);
            StartCoroutine(WaitForSecondsDead());
            Cursor.visible = true;
        }

        if (playerStats.Health == 0 && playerStats.BossFight)
        {
            pauseButton.gameObject.SetActive(false);
            StartCoroutine(WaitForSecondsBoss());
            Cursor.visible = true;
        }

        if (playerStats.BossDead)
        {
            pauseButton.gameObject.SetActive(false);
            materialCanvas.SetActive(false);
            pauseMenuCanvas.SetActive(false);
            bossEndCanvas.SetActive(true);
            bossText.text = "Boss defeated in " + playerStats.Night + " nights!";
            nightEndCamera.SetActive(true);
            timeOfDay.IsNight = false;
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        pauseMenuCanvas.SetActive(isPaused);
        materialCanvas.SetActive(!isPaused);  
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RetryButton()
    {
        if (!playerStats.BossDead)
        { 
            playerStats.EnemiesAlive = playerStats.Enemies;
        }
        playerStats.Health = playerStats.HealthRetry;
        playerStats.Armor = playerStats.ArmorRetry;
        playerStats.DamageTaken = 0;
        SceneManager.LoadScene("2D");
    }

    public void RetryBoss()
    {
        playerStats.Health = playerStats.HealthRetry;
        playerStats.Armor = playerStats.ArmorRetry;
        playerStats.DamageTaken = 0;
        SceneManager.LoadScene("2D");
    }

    IEnumerator WaitForSecondsDead()
     {
        yield return new WaitForSeconds(2f);

        Time.timeScale = 0f;
        materialCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
     }

    IEnumerator WaitForSecondsBoss()
    {
        yield return new WaitForSeconds(2f);

        if (Random.value < 0.5f)
        {
            BossRetrySound.Play();
        }
        else 
        {
            SecondBossRetrySound.Play();
        }

        Time.timeScale = 0f;
        materialCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        bossFailedCanvas.SetActive(true);
    }
}
