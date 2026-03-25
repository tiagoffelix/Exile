using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mine : MonoBehaviour, IInteractable
{
    private bool canMine = true; 
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI mineText;

    private AudioSource mineAudioSource;

    private void Start()
    {
        canMine = true;
        mineAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!playerStats.HasMined) 
        {
            mineText.text = "press [E] to mine iron";
        }
        else 
        {
            mineText.text = "come back tomorrow to mine more iron";
        }
    }

    public void Interact()
    {
        if (canMine && !playerStats.HasMined)
        {
            MineIron();
            playerStats.HasMined = true;
            mineAudioSource.Play();
        }
    }

    private void MineIron()
    {
        int ironAmount = 30; // Amount of iron to be obtained

        playerStats.Metal += ironAmount;
    }
}