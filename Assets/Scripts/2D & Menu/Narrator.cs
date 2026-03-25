using UnityEngine;

[CreateAssetMenu(fileName = "Narrator", menuName = "ScriptableObjects/Narrator")]
public class Narrator : ScriptableObject
{
    [SerializeField] private bool firstDay;
    [SerializeField] private bool firstNight;

    public bool FirstDay
    {
        get { return firstDay; }
        set { firstDay = value; }
    }

    public bool FirstNight
    {
        get { return firstNight; }
        set { firstNight = value; }
    }
    public void ResetStats()
    {
        firstDay = true;
        firstNight = true;
    }
}
