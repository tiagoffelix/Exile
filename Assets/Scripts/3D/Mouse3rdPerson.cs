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

            player.Rotate(Vector3.up * mouseX);
        }
    }
}