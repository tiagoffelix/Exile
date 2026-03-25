using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public PlayerStats playerStats;
    public TimeOfDayScript timeOfDay;
    public BuildingPositions buildingPositions;
    public NumberOfMaterials numberOfMaterials;
    public Narrator narrator;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Screen.fullScreen = true;

        Cursor.lockState = CursorLockMode.Confined;

        LoadGameData(); // Load the saved data when the game starts
    }

    public void ResetGame()
    {
        playerStats.ResetStats();
        buildingPositions.ResetStats();
        narrator.ResetStats();

        SaveGameData(); // Save the updated data after resetting the game
    }

    public void SaveGameData()
    {
        // Save the data to PlayerPrefs or a save file
        PlayerPrefs.SetInt("PlayerHealth", playerStats.Health);
        PlayerPrefs.SetInt("PlayerArmor", playerStats.Armor);
        PlayerPrefs.SetInt("PlayerSwordDamage", playerStats.SwordDamage);
        PlayerPrefs.SetInt("PlayerWood", playerStats.Wood);
        PlayerPrefs.SetInt("PlayerRock", playerStats.Rock);
        PlayerPrefs.SetInt("PlayerMetal", playerStats.Metal);
        PlayerPrefs.SetInt("PlayerEnemies", playerStats.Enemies);
        PlayerPrefs.SetInt("PlayerEnemiesAlive", playerStats.EnemiesAlive);
        PlayerPrefs.SetInt("PlayerNight", playerStats.Night);
        PlayerPrefs.SetInt("PlayerBossFight", playerStats.BossFight ? 1 : 0);
        PlayerPrefs.SetInt("PlayerBossDead", playerStats.BossDead ? 1 : 0);
        PlayerPrefs.SetInt("PlayerDamageTaken", playerStats.DamageTaken);
        PlayerPrefs.SetInt("PlayerHealthRetry", playerStats.HealthRetry);
        PlayerPrefs.SetInt("PlayerArmorRetry", playerStats.ArmorRetry);
        PlayerPrefs.SetInt("PlayerEasyDifficulty", playerStats.EasyDifficulty ? 1 : 0);
        PlayerPrefs.SetInt("PlayerMediumDifficulty", playerStats.MediumDifficulty ? 1 : 0);
        PlayerPrefs.SetInt("PlayerHardDifficulty", playerStats.HardDifficulty ? 1 : 0);
        PlayerPrefs.SetInt("PlayerDead", playerStats.Dead ? 1 : 0);
        PlayerPrefs.SetFloat("PlayerVolume", playerStats.Volume);
        PlayerPrefs.SetFloat("PlayerSensitivity", playerStats.Sensitivity);
        PlayerPrefs.SetInt("PlayerGameEnding", playerStats.GameEnding ? 1 : 0);
        PlayerPrefs.SetInt("PlayerHasMined", playerStats.HasMined ? 1 : 0);

        // Save time of day data
        PlayerPrefs.SetFloat("TimeOfDay", timeOfDay.TimeOfDay);
        PlayerPrefs.SetInt("IsNight", timeOfDay.IsNight ? 1 : 0);

        PlayerPrefs.SetInt("SmallTree", numberOfMaterials.SmallTree);
        PlayerPrefs.SetInt("Tree", numberOfMaterials.Tree);
        PlayerPrefs.SetInt("SmallRock", numberOfMaterials.SmallRock);
        PlayerPrefs.SetInt("Rock", numberOfMaterials.Rock);

        // Save building positions data
        PlayerPrefs.SetInt("HospitalBought", buildingPositions.HospitalBought ? 1 : 0);
        PlayerPrefs.SetInt("MineBought", buildingPositions.MineBought ? 1 : 0);
        PlayerPrefs.SetInt("ForgeBought", buildingPositions.ForgeBought ? 1 : 0);
        PlayerPrefs.SetInt("ArmoryBought", buildingPositions.ArmoryBought ? 1 : 0);
        PlayerPrefs.SetInt("WallsBought", buildingPositions.WallsBought ? 1 : 0);
        PlayerPrefs.SetInt("JeffPlayed", buildingPositions.JeffPlayed ? 1 : 0);
        PlayerPrefs.SetFloat("HospitalPosX", buildingPositions.HospitalPosition.x);
        PlayerPrefs.SetFloat("HospitalPosY", buildingPositions.HospitalPosition.y);
        PlayerPrefs.SetFloat("HospitalPosZ", buildingPositions.HospitalPosition.z);
        PlayerPrefs.SetFloat("HospitalRotX", buildingPositions.HospitalRotation.x);
        PlayerPrefs.SetFloat("HospitalRotY", buildingPositions.HospitalRotation.y);
        PlayerPrefs.SetFloat("HospitalRotZ", buildingPositions.HospitalRotation.z);
        PlayerPrefs.SetFloat("HospitalRotW", buildingPositions.HospitalRotation.w);
        PlayerPrefs.SetFloat("ForgePosX", buildingPositions.ForgePosition.x);
        PlayerPrefs.SetFloat("ForgePosY", buildingPositions.ForgePosition.y);
        PlayerPrefs.SetFloat("ForgePosZ", buildingPositions.ForgePosition.z);
        PlayerPrefs.SetFloat("ForgeRotX", buildingPositions.ForgeRotation.x);
        PlayerPrefs.SetFloat("ForgeRotY", buildingPositions.ForgeRotation.y);
        PlayerPrefs.SetFloat("ForgeRotZ", buildingPositions.ForgeRotation.z);
        PlayerPrefs.SetFloat("ForgeRotW", buildingPositions.ForgeRotation.w);
        PlayerPrefs.SetFloat("ArmoryPosX", buildingPositions.ArmoryPosition.x);
        PlayerPrefs.SetFloat("ArmoryPosY", buildingPositions.ArmoryPosition.y);
        PlayerPrefs.SetFloat("ArmoryPosZ", buildingPositions.ArmoryPosition.z);
        PlayerPrefs.SetFloat("ArmoryRotX", buildingPositions.ArmoryRotation.x);
        PlayerPrefs.SetFloat("ArmoryRotY", buildingPositions.ArmoryRotation.y);
        PlayerPrefs.SetFloat("ArmoryRotZ", buildingPositions.ArmoryRotation.z);
        PlayerPrefs.SetFloat("ArmoryRotW", buildingPositions.ArmoryRotation.w);

        PlayerPrefs.SetInt("FirstDay", narrator.FirstDay ? 1 : 0);
        PlayerPrefs.SetInt("FirstNight", narrator.FirstNight ? 1 : 0);

        PlayerPrefs.Save();
    }

    private void LoadGameData()
    {
        // Load the data from PlayerPrefs or a save file
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            playerStats.Health = PlayerPrefs.GetInt("PlayerHealth");
        }

        if (PlayerPrefs.HasKey("PlayerArmor"))
        {
            playerStats.Armor = PlayerPrefs.GetInt("PlayerArmor");
        }

        if (PlayerPrefs.HasKey("PlayerSwordDamage"))
        {
            playerStats.SwordDamage = PlayerPrefs.GetInt("PlayerSwordDamage");
        }

        if (PlayerPrefs.HasKey("PlayerWood"))
        {
            playerStats.Wood = PlayerPrefs.GetInt("PlayerWood");
        }

        if (PlayerPrefs.HasKey("PlayerRock"))
        {
            playerStats.Rock = PlayerPrefs.GetInt("PlayerRock");
        }

        if (PlayerPrefs.HasKey("PlayerMetal"))
        {
            playerStats.Metal = PlayerPrefs.GetInt("PlayerMetal");
        }

        if (PlayerPrefs.HasKey("PlayerEnemies"))
        {
            playerStats.Enemies = PlayerPrefs.GetInt("PlayerEnemies");
        }

        if (PlayerPrefs.HasKey("PlayerEnemiesAlive"))
        {
            playerStats.EnemiesAlive = PlayerPrefs.GetInt("PlayerEnemiesAlive");
        }

        if (PlayerPrefs.HasKey("PlayerNight"))
        {
            playerStats.Night = PlayerPrefs.GetInt("PlayerNight");
        }

        if (PlayerPrefs.HasKey("PlayerBossFight"))
        {
            playerStats.BossFight = PlayerPrefs.GetInt("PlayerBossFight") == 1;
        }

        if (PlayerPrefs.HasKey("PlayerBossDead"))
        {
            playerStats.BossDead = PlayerPrefs.GetInt("PlayerBossDead") == 1;
        }

        if (PlayerPrefs.HasKey("PlayerDamageTaken"))
        {
            playerStats.DamageTaken = PlayerPrefs.GetInt("PlayerDamageTaken");
        }

        if (PlayerPrefs.HasKey("PlayerHealthRetry"))
        {
            playerStats.HealthRetry = PlayerPrefs.GetInt("PlayerHealthRetry");
        }

        if (PlayerPrefs.HasKey("PlayerArmorRetry"))
        {
            playerStats.ArmorRetry = PlayerPrefs.GetInt("PlayerArmorRetry");
        }

        if (PlayerPrefs.HasKey("PlayerEasyDifficulty"))
        {
            playerStats.EasyDifficulty = PlayerPrefs.GetInt("PlayerEasyDifficulty") == 1;
        }

        if (PlayerPrefs.HasKey("PlayerMediumDifficulty"))
        {
            playerStats.MediumDifficulty = PlayerPrefs.GetInt("PlayerMediumDifficulty") == 1;
        }

        if (PlayerPrefs.HasKey("PlayerHardDifficulty"))
        {
            playerStats.HardDifficulty = PlayerPrefs.GetInt("PlayerHardDifficulty") == 1;
        }

        if (PlayerPrefs.HasKey("PlayerDead"))
        {
            playerStats.Dead = PlayerPrefs.GetInt("PlayerDead") == 1;
        }

        if (PlayerPrefs.HasKey("PlayerVolume"))
        {
            playerStats.Volume = PlayerPrefs.GetFloat("PlayerVolume");
        }

        if (PlayerPrefs.HasKey("PlayerSensitivity"))
        {
            playerStats.Sensitivity = PlayerPrefs.GetFloat("PlayerSensitivity");
        }

        if (PlayerPrefs.HasKey("PlayerGameEnding"))
        {
            playerStats.GameEnding = PlayerPrefs.GetInt("PlayerGameEnding") == 1;
        }

        if (PlayerPrefs.HasKey("PlayerHasMined"))
        {
            playerStats.HasMined = PlayerPrefs.GetInt("PlayerHasMined") == 1;
        }

        // Load time of day data
        if (PlayerPrefs.HasKey("TimeOfDay"))
        {
            timeOfDay.TimeOfDay = PlayerPrefs.GetFloat("TimeOfDay");
        }

        if (PlayerPrefs.HasKey("IsNight"))
        {
            timeOfDay.IsNight = PlayerPrefs.GetInt("IsNight") == 1;
        }

        if (PlayerPrefs.HasKey("SmallTree"))
        {
            numberOfMaterials.SmallTree = PlayerPrefs.GetInt("SmallTree");
        }

        if (PlayerPrefs.HasKey("Tree"))
        {
            numberOfMaterials.Tree = PlayerPrefs.GetInt("Tree");
        }

        if (PlayerPrefs.HasKey("SmallRock"))
        {
            numberOfMaterials.SmallRock = PlayerPrefs.GetInt("SmallRock");
        }

        if (PlayerPrefs.HasKey("Rock"))
        {
            numberOfMaterials.Rock = PlayerPrefs.GetInt("Rock");
        }

        // Load building positions data
        if (PlayerPrefs.HasKey("HospitalBought"))
        {
            buildingPositions.HospitalBought = PlayerPrefs.GetInt("HospitalBought") == 1;
        }

        if (PlayerPrefs.HasKey("MineBought"))
        {
            buildingPositions.MineBought = PlayerPrefs.GetInt("MineBought") == 1;
        }

        if (PlayerPrefs.HasKey("ForgeBought"))
        {
            buildingPositions.ForgeBought = PlayerPrefs.GetInt("ForgeBought") == 1;
        }

        if (PlayerPrefs.HasKey("ArmoryBought"))
        {
            buildingPositions.ArmoryBought = PlayerPrefs.GetInt("ArmoryBought") == 1;
        }

        if (PlayerPrefs.HasKey("WallsBought"))
        {
            buildingPositions.WallsBought = PlayerPrefs.GetInt("WallsBought") == 1;
        }

        if (PlayerPrefs.HasKey("JeffPlayed"))
        {
            buildingPositions.JeffPlayed = PlayerPrefs.GetInt("JeffPlayed") == 1;
        }

        if (PlayerPrefs.HasKey("HospitalPosX") && PlayerPrefs.HasKey("HospitalPosY") && PlayerPrefs.HasKey("HospitalPosZ"))
        {
            float posX = PlayerPrefs.GetFloat("HospitalPosX");
            float posY = PlayerPrefs.GetFloat("HospitalPosY");
            float posZ = PlayerPrefs.GetFloat("HospitalPosZ");
            buildingPositions.HospitalPosition = new Vector3(posX, posY, posZ);
        }

        if (PlayerPrefs.HasKey("HospitalRotX") && PlayerPrefs.HasKey("HospitalRotY") && PlayerPrefs.HasKey("HospitalRotZ") && PlayerPrefs.HasKey("HospitalRotW"))
        {
            float rotX = PlayerPrefs.GetFloat("HospitalRotX");
            float rotY = PlayerPrefs.GetFloat("HospitalRotY");
            float rotZ = PlayerPrefs.GetFloat("HospitalRotZ");
            float rotW = PlayerPrefs.GetFloat("HospitalRotW");
            buildingPositions.HospitalRotation = new Quaternion(rotX, rotY, rotZ, rotW);
        }

        if (PlayerPrefs.HasKey("ForgePosX") && PlayerPrefs.HasKey("ForgePosY") && PlayerPrefs.HasKey("ForgePosZ"))
        {
            float posX = PlayerPrefs.GetFloat("ForgePosX");
            float posY = PlayerPrefs.GetFloat("ForgePosY");
            float posZ = PlayerPrefs.GetFloat("ForgePosZ");
            buildingPositions.ForgePosition = new Vector3(posX, posY, posZ);
        }

        if (PlayerPrefs.HasKey("ForgeRotX") && PlayerPrefs.HasKey("ForgeRotY") && PlayerPrefs.HasKey("ForgeRotZ") && PlayerPrefs.HasKey("ForgeRotW"))
        {
            float rotX = PlayerPrefs.GetFloat("ForgeRotX");
            float rotY = PlayerPrefs.GetFloat("ForgeRotY");
            float rotZ = PlayerPrefs.GetFloat("ForgeRotZ");
            float rotW = PlayerPrefs.GetFloat("ForgeRotW");
            buildingPositions.ForgeRotation = new Quaternion(rotX, rotY, rotZ, rotW);
        }

        if (PlayerPrefs.HasKey("ArmoryPosX") && PlayerPrefs.HasKey("ArmoryPosY") && PlayerPrefs.HasKey("ArmoryPosZ"))
        {
            float posX = PlayerPrefs.GetFloat("ArmoryPosX");
            float posY = PlayerPrefs.GetFloat("ArmoryPosY");
            float posZ = PlayerPrefs.GetFloat("ArmoryPosZ");
            buildingPositions.ArmoryPosition = new Vector3(posX, posY, posZ);
        }

        if (PlayerPrefs.HasKey("ArmoryRotX") && PlayerPrefs.HasKey("ArmoryRotY") && PlayerPrefs.HasKey("ArmoryRotZ") && PlayerPrefs.HasKey("ArmoryRotW"))
        {
            float rotX = PlayerPrefs.GetFloat("ArmoryRotX");
            float rotY = PlayerPrefs.GetFloat("ArmoryRotY");
            float rotZ = PlayerPrefs.GetFloat("ArmoryRotZ");
            float rotW = PlayerPrefs.GetFloat("ArmoryRotW");
            buildingPositions.ArmoryRotation = new Quaternion(rotX, rotY, rotZ, rotW);
        }

        if (PlayerPrefs.HasKey("FirstDay"))
        {
            narrator.FirstDay = PlayerPrefs.GetInt("FirstDay") == 1;
        }

        if (PlayerPrefs.HasKey("FirstNight"))
        {
            narrator.FirstNight = PlayerPrefs.GetInt("FirstNight") == 1;
        }

    }
}
