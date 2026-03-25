using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildingPositions", menuName = "BuildingPositions")]
public class BuildingPositions : ScriptableObject
{
    [SerializeField] private bool hospitalBought;
    [SerializeField] private bool mineBought;
    [SerializeField] private bool forgeBought;
    [SerializeField] private bool armoryBought;
    [SerializeField] private bool wallsBought;

    [SerializeField] private bool jeffPlayed;

    [SerializeField] private Vector3 hospitalPosition;
    [SerializeField] private Quaternion hospitalRotation;

    [SerializeField] private Vector3 forgePosition;
    [SerializeField] private Quaternion forgeRotation;

    [SerializeField] private Vector3 armoryPosition;
    [SerializeField] private Quaternion armoryRotation;

    [Header("Building Prices")]
    [SerializeField] private ResourceAmount hospitalPrice;
    [SerializeField] private ResourceAmount minePrice;
    [SerializeField] private ResourceAmount forgePrice;
    [SerializeField] private ResourceAmount armoryPrice;
    [SerializeField] private ResourceAmount wallsPrice;

    public bool HospitalBought
    {
        get { return hospitalBought; }
        set { hospitalBought = value; }
    }

    public bool MineBought
    {
        get { return mineBought; }
        set { mineBought = value; }
    }

    public bool ForgeBought
    {
        get { return forgeBought; }
        set { forgeBought = value; }
    }

    public bool ArmoryBought
    {
        get { return armoryBought; }
        set { armoryBought = value; }
    }

    public bool WallsBought
    {
        get { return wallsBought; }
        set { wallsBought = value; }
    }

    public bool JeffPlayed
    {
        get { return jeffPlayed; }
        set { jeffPlayed = value; }
    }
    public Vector3 HospitalPosition
    {
        get { return hospitalPosition; }
        set { hospitalPosition = value; }
    }

    public Quaternion HospitalRotation
    {
        get { return hospitalRotation; }
        set { hospitalRotation = value; }
    }

    public Vector3 ForgePosition
    {
        get { return forgePosition; }
        set { forgePosition = value; }
    }

    public Quaternion ForgeRotation
    {
        get { return forgeRotation; }
        set { forgeRotation = value; }
    }

    public Vector3 ArmoryPosition
    {
        get { return armoryPosition; }
        set { armoryPosition = value; }
    }

    public Quaternion ArmoryRotation
    {
        get { return armoryRotation; }
        set { armoryRotation = value; }
    }

    public ResourceAmount HospitalPrice
    {
        get { return hospitalPrice; }
    }

    public ResourceAmount MinePrice
    {
        get { return minePrice; }
    }

    public ResourceAmount ForgePrice
    {
        get { return forgePrice; }
    }

    public ResourceAmount ArmoryPrice
    {
        get { return armoryPrice; }
    }

    public ResourceAmount WallsPrice
    {
        get { return wallsPrice; }
    }

    public void ResetStats()
    {
        hospitalBought = false;
        forgeBought = false;
        mineBought = false;
        armoryBought = false;
        wallsBought = false;
        hospitalPosition = Vector3.zero;
        hospitalRotation = Quaternion.identity;
        forgePosition = Vector3.zero;
        forgeRotation = Quaternion.identity;
        armoryPosition = Vector3.zero;
        armoryRotation = Quaternion.identity;
    }
}

[System.Serializable]
public class ResourceAmount
{
    public int stone;
    public int wood;
    public int iron;
}
