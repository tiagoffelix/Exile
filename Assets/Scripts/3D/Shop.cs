using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Image hospitalImage;
    [SerializeField] private Button hospitalButton;
    [SerializeField] private Image forgeImage;
    [SerializeField] private Button forgeButton;
    [SerializeField] private Image mineImage;
    [SerializeField] private Button mineButton;
    [SerializeField] private Image armoryImage;
    [SerializeField] private Button armoryButton;

    [SerializeField] private GameObject hospitalPriceObject;
    [SerializeField] private GameObject forgePriceObject;
    [SerializeField] private GameObject minePriceObject;
    [SerializeField] private GameObject armoryPriceObject;


    [SerializeField] private Button wallsButton;

    [SerializeField] private TextMeshProUGUI hospitalPriceStoneText;
    [SerializeField] private TextMeshProUGUI hospitalPriceWoodText;
    [SerializeField] private TextMeshProUGUI forgePriceStoneText;
    [SerializeField] private TextMeshProUGUI forgePriceWoodText;
    [SerializeField] private TextMeshProUGUI forgePriceIronText;
    [SerializeField] private TextMeshProUGUI minePriceStoneText;
    [SerializeField] private TextMeshProUGUI minePriceWoodText;
    [SerializeField] private TextMeshProUGUI armoryPriceStoneText;
    [SerializeField] private TextMeshProUGUI armoryPriceWoodText;
    [SerializeField] private TextMeshProUGUI armoryPriceIronText;

    [SerializeField] private TextMeshProUGUI wallsPriceStoneText;
    [SerializeField] private TextMeshProUGUI wallsPriceWoodText;
    [SerializeField] private TextMeshProUGUI wallsPriceIronText;

    [SerializeField] private BuildingPositions buildingPositions;
    [SerializeField] private PlayerStats playerStats;

    private void Update()
    {
        // Hospital
        bool canAffordHospital = CanAffordBuilding(buildingPositions.HospitalPrice);
        hospitalButton.interactable = !buildingPositions.HospitalBought && canAffordHospital;
        hospitalImage.enabled = buildingPositions.HospitalBought;
        hospitalPriceStoneText.text = buildingPositions.HospitalPrice.stone.ToString();
        hospitalPriceWoodText.text = buildingPositions.HospitalPrice.wood.ToString();

        // Forge
        bool canAffordForge = CanAffordBuilding(buildingPositions.ForgePrice);
        forgeButton.interactable = !buildingPositions.ForgeBought && canAffordForge;
        forgeImage.enabled = buildingPositions.ForgeBought;
        forgePriceStoneText.text = buildingPositions.ForgePrice.stone.ToString();
        forgePriceWoodText.text = buildingPositions.ForgePrice.wood.ToString();
        forgePriceIronText.text = buildingPositions.ForgePrice.iron.ToString();

        // Mine
        bool canAffordMine = CanAffordBuilding(buildingPositions.MinePrice);
        mineButton.interactable = !buildingPositions.MineBought && canAffordMine;
        mineImage.enabled = buildingPositions.MineBought;
        minePriceStoneText.text = buildingPositions.MinePrice.stone.ToString();
        minePriceWoodText.text = buildingPositions.MinePrice.wood.ToString();

        // Armory
        bool canAffordArmory = CanAffordBuilding(buildingPositions.ArmoryPrice);
        armoryButton.interactable = !buildingPositions.ArmoryBought && canAffordArmory;
        armoryImage.enabled = buildingPositions.ArmoryBought;
        armoryPriceStoneText.text = buildingPositions.ArmoryPrice.stone.ToString();
        armoryPriceWoodText.text = buildingPositions.ArmoryPrice.wood.ToString();
        armoryPriceIronText.text = buildingPositions.ArmoryPrice.iron.ToString();

        // Walls
        bool allBuildingsPurchased = buildingPositions.HospitalBought && buildingPositions.ForgeBought &&
            buildingPositions.MineBought && buildingPositions.ArmoryBought;
        hospitalButton.gameObject.SetActive(!allBuildingsPurchased);
        forgeButton.gameObject.SetActive(!allBuildingsPurchased);
        mineButton.gameObject.SetActive(!allBuildingsPurchased);
        armoryButton.gameObject.SetActive(!allBuildingsPurchased);
        wallsButton.gameObject.SetActive(allBuildingsPurchased);

        bool canAffordWalls = CanAffordBuilding(buildingPositions.WallsPrice);
        wallsButton.interactable = canAffordWalls;
        wallsPriceStoneText.text = buildingPositions.WallsPrice.stone.ToString();
        wallsPriceWoodText.text = buildingPositions.WallsPrice.wood.ToString();
        wallsPriceIronText.text = buildingPositions.WallsPrice.iron.ToString();

        hospitalPriceObject.SetActive(!buildingPositions.HospitalBought);
        forgePriceObject.SetActive(!buildingPositions.ForgeBought);
        minePriceObject.SetActive(!buildingPositions.MineBought);
        armoryPriceObject.SetActive(!buildingPositions.ArmoryBought);
    }
    public void BuyMine()
    {
        if (!buildingPositions.MineBought)
        {
            buildingPositions.MineBought = true;
            DeductBuildingCost(buildingPositions.MinePrice);
        }
    }

    private bool CanAffordBuilding(ResourceAmount price)
    {
        return playerStats.Wood >= price.wood && playerStats.Rock >= price.stone && playerStats.Metal >= price.iron;
    }

    private void DeductBuildingCost(ResourceAmount price)
    {
        playerStats.Wood -= price.wood;
        playerStats.Rock -= price.stone;
        playerStats.Metal -= price.iron;
    }
}
