using UnityEngine;

public class BulletMove : MonoBehaviour
{
    // 移動速度
    public float speed = 5.0f;
    public bool enemy;
    HPData data;
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);

        // 画面外で削除
        if (Mathf.Abs(transform.position.x) > 10 ||
            Mathf.Abs(transform.position.y) > 10)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Boss"))
        {
            collision.GetComponent<BossController>().TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
