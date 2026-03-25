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
        if (Input.GetMouseButtonDown(1))
        {
            canMove = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            canMove = true;
        }

        if (canMove)
        {
            sensitivity = playerStats.Sensitivity;

            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -70f, 70f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        } 
    }
}