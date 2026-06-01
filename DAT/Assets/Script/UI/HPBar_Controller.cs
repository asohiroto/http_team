using UnityEngine;
using UnityEngine.UI;
public class HPBarController : MonoBehaviour
{
    public Image hpBarFill; 
    private float maxHP = 100f;
    private float currentHP;

    [SerializeField] GameObject playerObj;
    PlayerController player;

    void Start() { currentHP = player.maxPlayerHP; UpdateHPBar(); }
    public void TakeDamage(float damage)
    {
        player = playerObj.GetComponent<PlayerController>();
        currentHP = Mathf.Clamp(currentHP - damage, 0,player.maxPlayerHP);
        UpdateHPBar();
    }
    void UpdateHPBar() { if (hpBarFill != null) hpBarFill.fillAmount = currentHP / maxHP; }
    void Update() { if (Input.GetKeyDown(KeyCode.Space)) TakeDamage(10f); } // スペースキーでダメージ
}