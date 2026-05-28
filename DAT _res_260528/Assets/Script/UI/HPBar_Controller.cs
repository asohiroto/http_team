using UnityEngine;
using UnityEngine.UI;
public class HPBarController : MonoBehaviour
{
    public Image hpBarFill; 
    private float maxHP = 100f;
    private float currentHP;

    void Start() { currentHP = maxHP; UpdateHPBar(); }
    public void TakeDamage(float damage)
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);
        UpdateHPBar();
    }
    void UpdateHPBar() { if (hpBarFill != null) hpBarFill.fillAmount = currentHP / maxHP; }
    void Update() { if (Input.GetKeyDown(KeyCode.Space)) TakeDamage(10f); } // スペースキーでダメージ
}