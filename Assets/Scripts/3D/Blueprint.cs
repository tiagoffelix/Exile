using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    private Plane groundPlane;
    private Quaternion initialRotation;
    private bool canPlace;

    [SerializeField] GameObject building;

    [SerializeField] private BuildingPositions buildingPositions;

    [SerializeField] private Material placementMaterial;

    private PlayerMovement player;
    private AudioSource placeBuildingSound;

    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        initialRotation = transform.rotation;

        SetCollidersAsTriggers(transform);
        canPlace = true;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        // Get the audio source from the PlayerMovement component
        placeBuildingSound = player.GetComponent<AudioSource>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distanceToPlane;

        if (groundPlane.Raycast(ray, out distanceToPlane))
        {
            Vector3 hitPoint = ray.GetPoint(distanceToPlane);

            if (!Input.GetMouseButton(1))
            {
                transform.position = hitPoint;
            }

            if (hitPoint != transform.position)
            {
                Quaternion rotation = Quaternion.LookRotation(hitPoint - transform.position, Vector3.up);
                transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, rotation.eulerAngles.y, initialRotation.eulerAngles.z);
            }
        }

        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            placeBuildingSound.Play();

            Instantiate(building, transform.position, transform.rotation);
            if (building.tag == "Hospital")
            {
                buildingPositions.HospitalPosition = transform.position;
                buildingPositions.HospitalRotation = transform.rotation;
                BuyHospital();

            }
            else if (building.tag == "Forge")
            {
                buildingPositions.ForgePosition = transform.position;
                buildingPositions.ForgeRotation = transform.rotation;
                BuyForge();
            }
            else if (building.tag == "Armory")
            {
                buildingPositions.ArmoryPosition = transform.position;
                buildingPositions.ArmoryRotation = transform.rotation;
                BuyArmory();
            }
            Destroy(gameObject);
        }
        if (!CheckPositionInRange(transform.position))
        {
            placementMaterial.color = Color.red;
        }
        else if (canPlace)
        {
            placementMaterial.color = Color.white;
        }
 
    }

    private void SetCollidersAsTriggers(Transform parentTransform)
    {
        Collider[] colliders = parentTransform.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
    }

    private bool CheckPositionInRange(Vector3 position)
    {
        return position.x >= -110f && position.x <= 110f && position.z >= -30f && position.z <= 30f && position.y == 0;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Hospital") || other.CompareTag("Mine") || other.CompareTag("Armory") || other.CompareTag("Forge") || other.CompareTag("Building"))
        {
            canPlace = false;
            placementMaterial.color = Color.red;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") ||other.CompareTag("Hospital") || other.CompareTag("Mine") || other.CompareTag("Armory") || other.CompareTag("Forge") || other.CompareTag("Building"))
        {
            canPlace = true;
            placementMaterial.color = Color.white;
        }
    }
    public void BuyHospital()
    {
        if (!buildingPositions.HospitalBought )
        {
            buildingPositions.HospitalBought = true;
            DeductBuildingCost(buildingPositions.HospitalPrice);
        }
    }

    public void BuyForge()
    {
        if (!buildingPositions.ForgeBought)
        {
            buildingPositions.ForgeBought = true;
            DeductBuildingCost(buildingPositions.ForgePrice);
        }
    }

    public void BuyArmory()
    {
        if (!buildingPositions.ArmoryBought)
        {
            buildingPositions.ArmoryBought = true;
            DeductBuildingCost(buildingPositions.ArmoryPrice);
        }
    }

    private void DeductBuildingCost(ResourceAmount price)
    {
        playerStats.Wood -= price.wood;
        playerStats.Rock -= price.stone;
        playerStats.Metal -= price.iron;
    }
}
