using UnityEngine;

public class DohyoShaker : MonoBehaviour
{
    public Rigidbody dohyoRb;
    public float shakeForce = 50f;
    public Transform center; // “y•U‚Ì’†S

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            // “y•U‚ÌˆÊ’u‚©‚ç’†S‚Ö‚Ì•ûŒü
            Vector3 dir = (center.position - transform.position).normalized;

            // ­‚µƒ‰ƒ“ƒ_ƒ€«‚ğ¬‚º‚é‚Æ©‘R
            dir += Random.insideUnitSphere * 0.3f;

            dohyoRb.AddForce(dir * shakeForce, ForceMode.Impulse);
        }
    }
}
