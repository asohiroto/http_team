using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    //private int currentHp;

    [SerializeField] private Image hpBarImage;
    public int damage;
    PlayerController player;

    //  [SerializeField] private float invincibilityTime = 1.0f;// 無敵時間
    private float lastDamageTime;   // 最後にダメージを受けた時間

    private bool isDead = true;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in objs)
        {
            player = obj.GetComponent<PlayerController>();
            if (player != null) break;
        }
        if (player != null)
        {
            player.playerHP = player.maxPlayerHP;
        }
        //currentHp = player.maxPlayerHP;
    }

    void Update()
    {
        if (player != null)
        {
            Debug.Log($"実際のHP:{player.playerHP} / {player.maxPlayerHP}");
            if (player.playerHP <= 0 && !isDead)
            {
                Die();
            }
        }
        UpdateHpBar();
    }
    private void UpdateHpBar() //HPのUIを更新
    {
        if (hpBarImage != null && player != null)
        {
            hpBarImage.fillAmount = player.playerHP / player.maxPlayerHP;
        }
    }
    private void Die()
{
        isDead = true;
        SceneManager.LoadScene("GameOverScene");
}
}

