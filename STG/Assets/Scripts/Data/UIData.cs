using UnityEngine;

[CreateAssetMenu(menuName = "GameData/UI")]
public class UIData : ScriptableObject
{
    public string scorePrefix = "Score: ";
    public string hpPrefix = "HP: ";
    public string gameOverText = "GAME OVER";
}
