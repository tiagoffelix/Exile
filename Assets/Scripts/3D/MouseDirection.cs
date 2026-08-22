using UnityEngine;

public class MouseDirection : MonoBehaviour
{
    public float sensitivity;
    public PlayerStats playerStats;

    [SerializeField] private Transform player;

    float xRotation;

    private bool canMove;

    private void Start()
    {
        xRotation = 0f;
        canMove = true;
    }

    void Update()
    {
        if (GameInput.FreeLookHoldPressed)
        {
            canMove = false;
        }
        else if (GameInput.FreeLookHoldReleased)
        {
            canMove = true;
        }

        if (canMove)
        {
            sensitivity = playerStats.Sensitivity;

            float mouseX = GameInput.LookX * sensitivity * Time.deltaTime;
            float mouseY = GameInput.LookY * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -70f, 70f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        } 
    }
}