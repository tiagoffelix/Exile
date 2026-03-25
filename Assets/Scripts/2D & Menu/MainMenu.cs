using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private GameObject continueButton;

    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        audioSource.Play();

        if (playerStats.GameEnding || playerStats.Dead) 
        {
            continueButton.SetActive(false);
        }
        Cursor.visible = true;
    }

    public void PlayGameEasyDifficulty()
    {
        StartGame();
        GameManager.Instance.playerStats.EasyDifficulty = true;
    }

    public void PlayGameMediumDifficulty()
    {
        StartGame();
        GameManager.Instance.playerStats.MediumDifficulty = true;
    }

    public void PlayGameHardDifficulty()
    {
        StartGame();
        GameManager.Instance.playerStats.HardDifficulty = true;
    }

    public void ContinueGame()
    {
        if (!GameManager.Instance.playerStats.Dead && !GameManager.Instance.playerStats.GameEnding)
        {
            if (GameManager.Instance.timeOfDay.IsNight)
            {
                SceneManager.LoadScene("2D");
            }
            else
            {
                SceneManager.LoadScene("3D");
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        GameManager.Instance.timeOfDay.TimeOfDay = 0;
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("MainStory");
    }
}
