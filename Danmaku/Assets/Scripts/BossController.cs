using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public int maxHP = 300;
    private int currentHP;

    public BossBulletSpawner spawner;

    private int phase = 1;

    void Start()
    {
        currentHP = maxHP;
        StartCoroutine(BossRoutine());
    }

    IEnumerator BossRoutine()
    {
        while (currentHP > 0)
        {
            if (phase == 1)
            {
                yield return StartCoroutine(spawner.Phase1Pattern());
            }
            else if (phase == 2)
            {
                yield return StartCoroutine(spawner.Phase2Pattern());
            }
            else if (phase == 3)
            {
                yield return StartCoroutine(spawner.Phase3Pattern());
            }

            yield return new WaitForSeconds(1f);
        }

        // HP0 → ボス撃破
        Destroy(gameObject);
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= maxHP * 0.66f && phase == 1)
        {
            phase = 2;
        }
        else if (currentHP <= maxHP * 0.33f && phase == 2)
        {
            phase = 3;
        }
    }
}
