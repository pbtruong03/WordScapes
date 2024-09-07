using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class ChildCategory : ScriptableObject
{
    public string name;
    public Sprite backgroundImage;
    public List<int> listLevelID;

    public int coinReward;
}
