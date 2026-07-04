using UnityEngine;

public class BomDestoy : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Death), 1f);
    }
    void Death()
    {
        Destroy(gameObject);
    }

}
