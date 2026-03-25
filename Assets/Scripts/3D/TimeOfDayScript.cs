using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTimeOfDay", menuName = "TimeOfDay")]
public class TimeOfDayScript : ScriptableObject
{
    [SerializeField] private float timeOfDay;
    [SerializeField] private bool isNight;

    public float TimeOfDay
    {
        get { return timeOfDay; }
        set { timeOfDay = value; }
    }

    public bool IsNight
    {
        get { return isNight; }
        set { isNight = value; }
    }
}
