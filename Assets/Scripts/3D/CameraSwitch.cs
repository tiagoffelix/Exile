using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private Toggle cameraToggle;

    [SerializeField] private PlayerStats playerStats;

    private bool firstPersonActive;

    private void Awake()
    {
        cameraToggle.onValueChanged.AddListener(OnCameraToggleValueChanged);
        if (!playerStats.GameEnding)
        {
            ToggleCameras();
        }
    }

    private void Update()
    {
        if (!playerStats.GameEnding) 
        {
            ToggleCameras();
        }
    }

    public void ToggleCameras()
    {
        if (firstPersonActive)
        {
            firstPersonCamera.gameObject.SetActive(true);
            thirdPersonCamera.gameObject.SetActive(false);
            cameraToggle.isOn = false;
        }
        else
        {
            firstPersonCamera.gameObject.SetActive(false);
            thirdPersonCamera.gameObject.SetActive(true);
            cameraToggle.isOn = true;
        }
    }

    public void OnCameraToggleValueChanged(bool isOn)
    {
        firstPersonActive = !isOn;
    }

    public bool isFirstPersonActive()
    {
        return firstPersonActive;
    }

    public void setFirstPersonActive(bool value)
    {
        firstPersonActive = value;
    }
}
