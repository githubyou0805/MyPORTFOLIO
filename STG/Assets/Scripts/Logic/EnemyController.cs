using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData data;
    public GameObject Bom;
    private float fireTimer = 0f;
    private int currentHp;

    public List<GameObject> itemPrefab = new();
    public float dropRate = 0.3f;

    void Start()
    {
        currentHp = data.hp;
    }

    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        transform.Translate(Vector3.down * data.moveSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= data.fireRate)
        {
            Instantiate(data.bulletPrefab, transform.position, Quaternion.identity);
            fireTimer = 0f;
        }
    }

    public void Damage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0)
        {
            var ui = Object.FindFirstObjectByType<UIController>();
            ui.UpdateScore(100);
            Instantiate(Bom, transform.position, Quaternion.identity);
            if (Random.value < dropRate)
            {
                var i = Random.Range(0, itemPrefab.Count);
                Instantiate(itemPrefab[i], transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
