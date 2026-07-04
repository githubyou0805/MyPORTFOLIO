using UnityEngine;

public class BulletController : MonoBehaviour
{
    public BulletData data;

    void Update()
    {
        transform.Translate(Vector3.up * data.speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && name == "Bullet(Clone)")
        {
            col.GetComponent<EnemyController>().Damage(data.damage);
            Destroy(gameObject);
        }
    }
}
