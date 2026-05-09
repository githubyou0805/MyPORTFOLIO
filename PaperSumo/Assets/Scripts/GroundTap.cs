using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GroundTap : MonoBehaviour
{
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject F;
    [SerializeField] private GameObject S;
    public Rigidbody[] rikishis; // —Ím‚ğ“o˜^
    public float tapForce = 3f;
    private bool IsStarted = false;

    private void Awake()
    {
        BG.SetActive(true);
        S.SetActive(true);
        F.SetActive(false);
    }
    void Update()
    {
        if (!IsStarted) return;
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var rb in rikishis)
            {
                // ƒ‰ƒ“ƒ_ƒ€‚È‰¡•ûŒü‚ÌU“®
                Vector3 dir = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f,1f),
                    Random.Range(-1f, 1f)
                ).normalized;

                rb.AddForce(dir * tapForce, ForceMode.Impulse);
            }
        }
    }
    public void OnStart()
    {
        BG.SetActive(false);
        S.SetActive(false);
        IsStarted = true;
    }
    public void OnReStart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Finish()
    {
        BG.SetActive(true);
        F.SetActive(true);
        IsStarted=false;
    }
}
