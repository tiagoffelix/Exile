using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    [SerializeField] private GameObject build;

    public void SpawnBlueprint()
    {
        Instantiate(build); 
    }

    public void BuyMine()
    {
        build.SetActive(false);
    }
}
