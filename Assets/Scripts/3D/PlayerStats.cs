using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private int health;
    [SerializeField] private int armor;
    [SerializeField] private int swordDamage;

    [SerializeField] private int wood;
    [SerializeField] private int rock;
    [SerializeField] private int metal;

    [SerializeField] private int enemies; 
    [SerializeField] private int enemiesAlive;
    [SerializeField] private int night;

    [SerializeField] private bool bossFight;
    [SerializeField] private bool bossDead;

    [SerializeField] private bool hasMined;

    [SerializeField] private int damageTaken;

    [SerializeField] private bool easyDifficulty;
    [SerializeField] private bool mediumDifficulty;
    [SerializeField] private bool hardDifficulty;

    [SerializeField] private bool dead;

    [SerializeField] private float volume;
    [SerializeField] private float sensitivity;

    [SerializeField] private bool gameEnding;

    [SerializeField] private int healthRetry;
    [SerializeField] private int armorRetry;

    public int Health 
    {
        get { return this.health; }
        set {  this.health = value; }
    }

    public int Armor
    {
        get { return this.armor; }
        set { this.armor = value; }
    }

    public int HealthRetry
    {
        get { return this.healthRetry; }
        set { this.healthRetry = value; }
    }

    public int ArmorRetry
    {
        get { return this.armorRetry; }
        set { this.armorRetry = value; }
    }

    public int SwordDamage
    {
        get { return this.swordDamage; }
        set { this.swordDamage = value; }
    }
    public int Wood
    {
        get { return this.wood; }
        set { this.wood = value; }
    }

    public int Rock
    {
        get { return this.rock; }
        set { this.rock = value; }
    }

    public int Metal
    {
        get { return this.metal; }
        set { this.metal = value; }
    }

    public int Enemies
    {
        get { return this.enemies; }
        set { this.enemies = value; }
    }
    public int EnemiesAlive
    {
        get { return this.enemiesAlive; }
        set { this.enemiesAlive = value; }
    }

    public int Night
    {
        get { return this.night; }
        set { this.night = value; }
    }

    public bool BossFight
    {
        get { return this.bossFight; }
        set { this.bossFight = value; }
    }

    public bool BossDead
    {
        get { return this.bossDead; }
        set { this.bossDead = value; }
    }

    public int DamageTaken
    {
        get { return this.damageTaken; }
        set { this.damageTaken = value; }
    }

    public bool EasyDifficulty
    {
        get { return this.easyDifficulty; }
        set { this.easyDifficulty = value; }
    }

    public bool MediumDifficulty
    {
        get { return this.mediumDifficulty; }
        set { this.mediumDifficulty = value; }
    }

    public bool HardDifficulty
    {
        get { return this.hardDifficulty; }
        set { this.hardDifficulty = value; }
    }

    public bool Dead
    {
        get { return this.dead; }
        set { this.dead = value; }
    }

    public float Volume
    {
        get { return this.volume; }
        set { this.volume = value; }
    }

    public float Sensitivity
    {
        get { return this.sensitivity; }
        set { this.sensitivity = value; }
    }

    public bool GameEnding
    {
        get { return this.gameEnding; }
        set { this.gameEnding = value; }
    }

    public bool HasMined
    {
        get { return this.hasMined; }
        set { this.hasMined = value; }
    }

    public void ResetStats()
    {
        health = 100;
        armor = 0;
        healthRetry = 0;
        armorRetry = 0;
        swordDamage = 20;
        wood = 0;
        rock = 0;
        metal = 0;
        enemies = 3;
        enemiesAlive = enemies;
        night = 1;
        bossFight = false;
        bossDead = false;
        damageTaken = 0;
        easyDifficulty = false;
        mediumDifficulty = false;
        hardDifficulty = false;
        dead = false;
        gameEnding = false;
        hasMined = false;
    }


    public void IncrementNightAndEnemies()
    {
        night++;
        enemies++;
    }

    public void RestoreHealth() {

        health += (damageTaken == 0 ? 20 : (damageTaken / 2));
        damageTaken = 0;

        if (health > 100)
        { 
            health = 100;
        }   

    }

    public void RestoreHealthAndArmor()
    {
        health += (damageTaken == 0 ? 10 : (damageTaken / 2));
        damageTaken = 0;

        if (health > 100)
        {
            armor += health - 100;
            health = 100;
        }
        if (armor > 100) {

            armor = 100;

        }
    }

    public void AddArmor(int armorReceived)
    {
        armor += armorReceived;

        if (armor > 100)
        {

            armor = 100;

        }
    }

    public void TakeDamage(int damage) {

        if (armor > 0)
        {
            armor -= damage;
            damageTaken += damage;
            if (armor < 0)
            {
                health += armor;
                armor = 0;
            }
        }
        else
        {
            health -= damage;
            damageTaken += damage;
            if (health < 0)
            {
                health = 0;
            }

        }

    }
}
