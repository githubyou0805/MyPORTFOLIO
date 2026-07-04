using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData data;
    private float fireTimer = 0f;
    public int hp;
    public int maxHP = 3;
    void Start()
    {
        hp = maxHP;

        var ui = Object.FindFirstObjectByType<UIController>();
        ui.InitHP(maxHP);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyBullet") || col.CompareTag("Enemy"))
        {
            hp -= 10;

            var ui = Object.FindFirstObjectByType<UIController>();
            ui.UpdateHP(hp);
            Destroy(col.gameObject);
            if (hp <= 0)
            {
                ui.ShowGameOver();
                Destroy(gameObject);
            }
        }
    }
    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");

        Vector3 dir = new Vector3(h, 0, 0);
        transform.Translate(dir * data.moveSpeed * Time.deltaTime);
        ClampPosition();
    }
    void ClampPosition()
    {
        Camera cam = Camera.main;

        // カメラの画面端をワールド座標で取得
        float left = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float right = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float bottom = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float top = cam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // プレイヤーの半径（Spriteの大きさ）
        float radius = 0.5f; // 必要なら調整 or SpriteRenderer.bounds.extents.x を使う

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, left + radius, right - radius);
        pos.y = Mathf.Clamp(pos.y, bottom + radius, top - radius);

        transform.position = pos;
    }

    void Shoot()
    {
        fireTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && fireTimer >= data.fireRate)
        {
            Instantiate(data.bulletPrefab, new Vector3(transform.position.x,
                                                        transform.position.y + 1,
                                                        transform.position.z), Quaternion.identity);
            fireTimer = 0f;
        }
    }
    public void ResetStatus()
    {
        hp = maxHP;

        // 強化状態を初期化したい場合ここに追加
        data.moveSpeed = data.defaultMoveSpeed;
        data.fireRate = data.defaultFireRate;
        data.bulletPrefab.GetComponent<BulletController>().data.damage
            = data.bulletPrefab.GetComponent<BulletController>().data.defaultDamage;
    }

    public void ApplyItem(ItemData item)
    {
        var ui = Object.FindFirstObjectByType<UIController>();
        ui.UpdateHP(hp);

        switch (item.itemType)
        {
            case ItemType.Heal:
                hp = Mathf.Min(maxHP, hp + item.amount);
                ui.UpdateHP(hp);
                break;

            case ItemType.PowerUp:
                data.bulletPrefab.GetComponent<BulletController>().data.damage += item.amount;
                break;

            case ItemType.FireRateUp:
                data.fireRate = Mathf.Max(0.05f, data.fireRate - item.amount * 0.05f);
                break;

            case ItemType.SpeedUp:
                data.moveSpeed += item.amount;
                break;
        }
    }
}
