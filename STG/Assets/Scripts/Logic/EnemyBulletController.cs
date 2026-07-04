using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public EnemyBulletData data;

    void Update()
    {
        transform.Translate(Vector3.up * data.speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && name == "EnemyBullet(Clone)")
        {
            col.GetComponent<PlayerController>().hp -= data.damage;
            var ui = Object.FindFirstObjectByType<UIController>();
            ui.UpdateHP(col.GetComponent<PlayerController>().hp);
            Destroy(gameObject);
        }
    }
}
