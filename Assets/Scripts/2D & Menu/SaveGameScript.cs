using UnityEngine;

public class SaveGameScript : MonoBehaviour
{
    public void SaveGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGameData();
        }
        else
        {
            Debug.LogWarning("GameManager.Instance is null. Make sure the GameManager is properly set up.");
        }
    }
}
