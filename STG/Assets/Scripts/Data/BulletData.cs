using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Bullet")]
public class BulletData : ScriptableObject
{
    public float speed = 10f;
    public int damage = 1;
    public int defaultDamage = 1;
}
