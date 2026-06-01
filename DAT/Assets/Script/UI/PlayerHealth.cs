using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;
    private int currentHp;

   

    [SerializeField] private Image hpBarImage;

    PlayerController player;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in objs)
        {
            player = obj.GetComponent<PlayerController>();
            if (player != null) break;
        }
        currentHp = player.maxPlayerHP;
        UpdateHpBar();
    }

    void Update()
    {
       
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;　//現在のHPからダメージ分引く
        currentHp = Mathf.Max(currentHp, 0);

        UpdateHpBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void UpdateHpBar() //HPのUIを更新
    {
        if (hpBarImage != null)
        {
            hpBarImage.fillAmount = (float)currentHp /player.maxPlayerHP;
        }
    }

    private void Die()
    {
        Debug.Log("死亡");
       
    }
}