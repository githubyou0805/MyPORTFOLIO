using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float PlayerSpeed = 5.0f;
    private Rigidbody2D _rb;
    private float _hori;
    [SerializeField] private float ShotLate;
    [SerializeField] private GameObject Bullet;
    private float coolDawn;
    void Start()
    {
        coolDawn = ShotLate;
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _hori = Input.GetAxis("Horizontal");
        ShotLate -= Time.deltaTime;
        if (ShotLate < 0 && Input.GetKey(KeyCode.Space))
        {
            var spawnBullet = Instantiate(Bullet,new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1f), Quaternion.identity);
            ShotLate = coolDawn;
        }
    }
    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_hori * PlayerSpeed, _rb.linearVelocity.y);
    }
}
