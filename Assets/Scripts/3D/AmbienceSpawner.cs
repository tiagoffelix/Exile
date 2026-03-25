using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;

    [SerializeField] private GameObject hospital;
    [SerializeField] private GameObject armory;
    [SerializeField] private GameObject forge;
    [SerializeField] private GameObject mineEntrance;
    [SerializeField] private GameObject mineInteractable;
    [SerializeField] private GameObject walls;

    [SerializeField] private BuildingPositions buildingPositions;
    [SerializeField] private TimeOfDayScript timeOfDay;
    [SerializeField] private NumberOfMaterials numberOfMaterials;

    private int startingValueMaterial;

    void Awake()
    {
        startingValueMaterial = 7;

        if(timeOfDay.TimeOfDay == 0)
        {
            numberOfMaterials.Rock = startingValueMaterial;
            numberOfMaterials.SmallRock = startingValueMaterial;
            numberOfMaterials.Tree = startingValueMaterial;
            numberOfMaterials.SmallTree = startingValueMaterial;
        }

        SpawnPrefabs();

        if(buildingPositions.HospitalBought)
        {
            Instantiate(hospital, buildingPositions.HospitalPosition, buildingPositions.HospitalRotation);
        }
        if (buildingPositions.ArmoryBought)
        {
            Instantiate(armory, buildingPositions.ArmoryPosition, buildingPositions.ArmoryRotation);
        }
        if (buildingPositions.ForgeBought)
        {
            Instantiate(forge, buildingPositions.ForgePosition, buildingPositions.ForgeRotation);
        }
        if (buildingPositions.MineBought)
        {
            mineInteractable.SetActive(true);
            mineEntrance.SetActive(false);
        }
        if (buildingPositions.WallsBought)
        {
            walls.SetActive(true);
        }
        else 
        {
            walls.SetActive(false);
        }
    }

    public void SpawnPrefabs()
    {
        float minX = -110f;
        float maxX = 110f;
        float minZ = -30f;
        float maxZ = 30f;

        SpawnMaterial(prefabs[0], numberOfMaterials.Rock, minX, maxX, minZ, maxZ, 0.6f);
        SpawnMaterial(prefabs[1], numberOfMaterials.SmallRock, minX, maxX, minZ, maxZ, 0.2f);
        SpawnMaterial(prefabs[2], numberOfMaterials.Tree, minX, maxX, minZ, maxZ, 0f);
        SpawnMaterial(prefabs[3], numberOfMaterials.SmallTree, minX, maxX, minZ, maxZ, 0f);
    }

    private void SpawnMaterial(GameObject prefab, int count, float minX, float maxX, float minZ, float maxZ, float yPosition)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), yPosition, Random.Range(minZ, maxZ));
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}
