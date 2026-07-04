using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Player")]
public class PlayerData : ScriptableObject
{
    public float moveSpeed = 5f;
    public float fireRate = 0.2f;
    public GameObject bulletPrefab;
    public float defaultMoveSpeed = 5f;
    public float defaultFireRate = 0.2f;
}
