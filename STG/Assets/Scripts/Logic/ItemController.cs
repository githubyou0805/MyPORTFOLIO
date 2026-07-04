using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemData data;
    public float fallSpeed = 2f;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var player = col.GetComponent<PlayerController>();
            player.ApplyItem(data);
            Destroy(gameObject);
        }
    }
}
