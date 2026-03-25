using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BedEnding : MonoBehaviour, IInteractable
{
    [SerializeField] private Camera[] cameras;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Camera otherCamera;
    private bool move;
    private Vector3 maxCameraPosition;

    private void Start()
    {
        move = false;
        maxCameraPosition = new Vector3(-10f, 170f, 0f);
    }

    private void Update()
    {
        if (move)
        {

            float xMovePerSecond = 10f;    // Movement speed in the x-axis per second
            float zMovePerSecond = 10f;    // Movement speed in the z-axis per second
            float yMovePerSecond = 10f;    // Movement speed in the y-axis per second

            Vector3 currentPosition = otherCamera.transform.position;
            float newX = Mathf.MoveTowards(currentPosition.x, maxCameraPosition.x, xMovePerSecond * Time.deltaTime);
            float newY = Mathf.MoveTowards(currentPosition.y, maxCameraPosition.y, yMovePerSecond * Time.deltaTime);
            float newZ = Mathf.MoveTowards(currentPosition.z, maxCameraPosition.z, zMovePerSecond * Time.deltaTime);

            // Update the camera position
            otherCamera.transform.position = new Vector3(newX, newY, newZ);

            // Check if the camera has reached the maximum position
            if (otherCamera.transform.position == maxCameraPosition)
            {
                SceneManager.LoadScene("Credits");
            }
        }
    }

    public void Interact()
    {
        playerStats.GameEnding = true;

        // Disable other active cameras
        DeactivateOtherCameras();

        otherCamera.gameObject.SetActive(true);

        move = true;
    }

    private void DeactivateOtherCameras()
    {
        foreach (Camera camera in cameras)
        {
            if (camera.gameObject != otherCamera.gameObject)
            {
                camera.gameObject.SetActive(false);
            }
        }
    }

}
