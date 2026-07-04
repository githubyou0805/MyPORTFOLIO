using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Enemy")]
public class EnemyData : ScriptableObject
{
    public float moveSpeed = 2f;
    public int hp = 3;
    public GameObject bulletPrefab;
    public float fireRate = 1.5f;
}
