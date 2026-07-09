using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHpUi : MonoBehaviour
{
    [Header("対象ボス")]
    [SerializeField] private EnemyHpManager bossHpManager;

    [Header("参照UI")]
    [SerializeField] private Image hpImage; // HPバー

    private float maxHp;
    void Start()
    {
        if (bossHpManager != null)
        {
            maxHp = bossHpManager.GetCurrentHp();
        }
    }

    
    void Update()
    {
        if (bossHpManager == null) return;

        float currentHp = bossHpManager.GetCurrentHp();

        hpImage.fillAmount = currentHp / maxHp;
    }
}
