using TMPro;
using UnityEngine;

public class RikishiFallChecker : MonoBehaviour
{
    public float fallAngle = 45f; // ‰½“xŒX‚¢‚½‚ç“|‚ê‚Æ‚Ý‚È‚·‚©
    private bool isFallen = false;
    private bool EnemyFallen = false;
    [HideInInspector] public bool IsFinished = false;
    public float angle;
    [SerializeField] private GameObject EnemyPlayer;
    [SerializeField] private TextMeshProUGUI Winer;
    [SerializeField] private TextMeshProUGUI Looser;
    [SerializeField] private GameObject Ground;
    private GroundTap gt;
    private RikishiFallChecker rc;
    private void Start()
    {
        gt = Ground.GetComponent<GroundTap>();
        rc = EnemyPlayer.GetComponent<RikishiFallChecker>();
    }
    void Update()
    {
        if (isFallen) return;

        // —ÍŽm‚Ìã•ûŒüƒxƒNƒgƒ‹
        Vector3 up = transform.up;

        // ã•ûŒü‚Æƒ[ƒ‹ƒhã•ûŒü‚ÌŠp“x‚ðŒvŽZ
        angle = Vector3.Angle(up, Vector3.up);

        if (angle > fallAngle)
        {
            isFallen = true;
            Debug.Log(name + " ‚ª“|‚ê‚½I");
            OnFall();
        }
        if(rc.angle > fallAngle)
        {
            EnemyFallen=true;
            OnFall();
        }
    }

    void OnFall()
    {
        if (IsFinished) return;

        if (isFallen)
        {
            Winer.text = $"{EnemyPlayer.name}";
            Looser.text = $"{name}";
            gt.Finish();
            IsFinished = true;
            rc.IsFinished = true;
        }
        else if(EnemyFallen)
        {
            Winer.text = $"{name}";
            Looser.text = $"{EnemyPlayer.name}";
            gt.Finish();
            IsFinished = true;
            rc.IsFinished = true;
        }
    }
}
