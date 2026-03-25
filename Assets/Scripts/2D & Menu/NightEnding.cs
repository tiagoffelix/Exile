using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NightEnding : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera otherCamera;
    [SerializeField] private PlayerStats playerStats;

    private bool moveUp;
    private Vector3 maxCameraPosition;

    private void Start()
    {
        maxCameraPosition = new Vector3(0f, 0f, -16f);
		mainCamera.gameObject.SetActive(false);
        otherCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        float xMovePerSecond = 4f;    // Movement speed in the x-axis per second
        float zMovePerSecond = 4f;    // Movement speed in the z-axis per second
        float yMovePerSecond = 4f;    // Movement speed in the y-axis per second

        Vector3 currentPosition = otherCamera.transform.position;
        float newX = Mathf.MoveTowards(currentPosition.x, maxCameraPosition.x, xMovePerSecond * Time.deltaTime);
        float newY = Mathf.MoveTowards(currentPosition.y, maxCameraPosition.y, yMovePerSecond * Time.deltaTime);
        float newZ = Mathf.MoveTowards(currentPosition.z, maxCameraPosition.z, zMovePerSecond * Time.deltaTime);

        // Update the camera position
        otherCamera.transform.position = new Vector3(newX, newY, newZ);

        // Check if the camera has reached the maximum position
        if (otherCamera.transform.position == maxCameraPosition)
        {
            if (!playerStats.BossDead)
            {
                playerStats.Enemies++;
                playerStats.EnemiesAlive = playerStats.Enemies;
                playerStats.Night++;
            }
            SceneManager.LoadScene("3D");
        }
    }
}
