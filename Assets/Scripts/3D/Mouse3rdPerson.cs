using UnityEngine;

public class Mouse3rdPerson : MonoBehaviour
{
    public float sensitivity;
    public PlayerStats playerStats;

    [SerializeField] private Transform player;

    private bool canMove;

    private void Start()
    {
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

            player.Rotate(Vector3.up * mouseX);
        }
    }
}