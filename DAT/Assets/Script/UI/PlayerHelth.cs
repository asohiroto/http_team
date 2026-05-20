using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;
    private int currentHp;

    // ★ここに新しく「スペースキーで受けるダメージ量」の変数を追加します！
    [SerializeField] private int spaceKeyDamage = 20;

    [SerializeField] private Image hpBarImage;

    void Start()
    {
        currentHp = maxHp;
        UpdateHpBar();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ★カッコ内を、上で作った変数（spaceKeyDamage）に変えます
            TakeDamage(spaceKeyDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);

        UpdateHpBar();

        // ログで引かれているダメージ量（damage）も表示するようにします
        Debug.Log($"スペースキー入力！ {damage} ダメージ受けた！ 残りHP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void UpdateHpBar()
    {
        if (hpBarImage != null)
        {
            hpBarImage.fillAmount = (float)currentHp / maxHp;
        }
    }

    private void Die()
    {
        Debug.Log("プレイヤーは倒れた！");
       
    }
}