
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNumberOfMaterials", menuName = "NumberOfMaterials")]
public class NumberOfMaterials : ScriptableObject
{
    [SerializeField] private int smallTree;
    [SerializeField] private int tree;
    [SerializeField] private int smallRock;
    [SerializeField] private int rock;

    public int SmallTree
    {
        get { return smallTree; }
        set { smallTree = value; }
    }

    public int Tree
    {
        get { return tree; }
        set { tree = value; }
    }

    public int SmallRock
    {
        get { return smallRock; }
        set { smallRock = value; }
    }

    public int Rock
    {
        get { return rock; }
        set { rock = value; }
    }
}
