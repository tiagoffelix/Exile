using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarEnemy : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Transform healthBarTransform;

    private void LateUpdate()
    {
        healthBarTransform.position = enemy.transform.position ;
    }

}
   
