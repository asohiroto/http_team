using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    //private int currentHp;

    [SerializeField] private Image hpBarImage;
    public int damage;
    PlayerController player;

    [SerializeField] private float invincibilityTime = 1.0f;// 無敵時間
    private float lastDamageTime;   // 最後にダメージを受けた時間

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

        UpdateHpBar();

        lastDamageTime = Time.time;
        //currentHp = player.maxPlayerHP;
    }

    void Update()
    {
        if (player != null)
        {
            Debug.Log($"実際のHP:{player.playerHP} / {player.maxPlayerHP}");
        }

        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            DecreaseHP(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 接触した敵のTagが"Enemy"だったらダメージ
        if (collision.CompareTag("Enemy"))
        {
            DecreaseHP(damage);
            //lastDamageTime = Time.time; // ダメージの時間を最新に更新
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 接触した敵のTagが"Enemy"だったらダメージ
        if (collision.CompareTag("Enemy"))
        {
            TryDamage();
            //lastDamageTime = Time.time; 
        }
    }


    private void TryDamage()
    {
        if (Time.time >= lastDamageTime + invincibilityTime)
        {
            DecreaseHP(damage);
            lastDamageTime = Time.time; // ダメージを受けた時間を現在に更新
        }
    }

    private void DecreaseHP(int damage)
    {
        if (player != null)
        {
            player.playerHP -= damage;
            player.playerHP = Mathf.Max(player.playerHP, 0);

            UpdateHpBar();

            if (player.playerHP <= 0)
            {
                Die();
            }
        }
    }

    private void UpdateHpBar() //HPのUIを更新
    {
        if (hpBarImage != null && player != null)
        {
            hpBarImage.fillAmount = (float)player.playerHP / player.maxPlayerHP;
        }
    }

    private void Die()
    {
        Debug.Log("死亡");
        SceneManager.LoadScene("GameOverScene");

    }
}