using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayEnding : MonoBehaviour
{
    [SerializeField] private Camera[] cameras;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Camera otherCamera;
    [SerializeField] private TimeOfDayScript timeOfDay;

    private bool moveUp;
    private Vector3 maxCameraPosition;

    private void Start()
    {
        maxCameraPosition = new Vector3(-10f, 170f, 0f);
        DeactivateOtherCameras();
        otherCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        float xMovePerSecond = 70f;    // Movement speed in the x-axis per second
        float zMovePerSecond = 70f;    // Movement speed in the z-axis per second
        float yMovePerSecond = 70f;    // Movement speed in the y-axis per second

        Vector3 currentPosition = otherCamera.transform.position;
        float newX = Mathf.MoveTowards(currentPosition.x, maxCameraPosition.x, xMovePerSecond * Time.deltaTime);
        float newY = Mathf.MoveTowards(currentPosition.y, maxCameraPosition.y, yMovePerSecond * Time.deltaTime);
        float newZ = Mathf.MoveTowards(currentPosition.z, maxCameraPosition.z, zMovePerSecond * Time.deltaTime);

        // Update the camera position
        otherCamera.transform.position = new Vector3(newX, newY, newZ);

        // Check if the camera has reached the maximum position
        if (otherCamera.transform.position == maxCameraPosition)
        {
            SceneManager.LoadScene("2D");
            playerStats.GameEnding = false;
            timeOfDay.TimeOfDay = 0;
            timeOfDay.IsNight = true;
            playerStats.HasMined = false;
        }
        
    }

    private void DeactivateOtherCameras()
    {
        playerStats.GameEnding = true;

        foreach (Camera camera in cameras)
        {
            if (camera.gameObject != otherCamera.gameObject)
            {
                camera.gameObject.SetActive(false);
            }
        }
    }
}
