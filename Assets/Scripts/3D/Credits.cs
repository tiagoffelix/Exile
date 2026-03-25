using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private float positionTolerance;
    [SerializeField] private GameObject clickToContinue;

    private float endingYPosition;

    private void Start()
    {
        positionTolerance = 0.01f;
        endingYPosition = 251.1374f;
    }

    private void Update()
    {
        float currentPositionY = text.transform.position.y;

        if (Mathf.Abs(currentPositionY - endingYPosition) <= positionTolerance)
        {
            clickToContinue.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
