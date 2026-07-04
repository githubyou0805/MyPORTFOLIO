using UnityEngine;

public enum ItemType
{
    Heal,
    PowerUp,
    FireRateUp,
    SpeedUp
}

[CreateAssetMenu(menuName = "GameData/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public int amount; // 回復量や強化量
}
