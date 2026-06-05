using System.Collections;
using UnityEngine;

public class EneBossAttack : MonoBehaviour
{
    EneBoss2 eneCtrl;
    PlayerController playerCtrl;
    GameObject enemyObj;
    GameObject playerObj;

    [SerializeField] Sprite[] attackSprite;
    float animTime = 0.05f;
    SpriteRenderer spriteRenderer;
    public int attackDamage;

    [SerializeField] int startColFrame;
    [SerializeField] int endColFrame;
    CircleCollider2D circleCol;

    // アタックエフェクトのアニメーションと当たり判定の管理------------------

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyObj = GameObject.Find("Boss2");
        eneCtrl = enemyObj.GetComponent<EneBoss2>();
        circleCol = GetComponent<CircleCollider2D>();
        circleCol.enabled = false;

        StartCoroutine(AttackAnim());
    }

    void Update()
    {
        attackDamage = eneCtrl.attackPower;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerObj = col.gameObject;
            playerCtrl = playerObj.GetComponent<PlayerController>();
            playerCtrl.Damaged(attackDamage);
        }
    }

    IEnumerator AttackAnim()
    {
        for (int i = 0; i < attackSprite.Length; i++)
        {
            spriteRenderer.sprite = attackSprite[i];

            yield return new WaitForSeconds(animTime);
            if (i + 1 >= startColFrame && i + 1 <= endColFrame)
            {
                circleCol.enabled = true;
            }
            else
            {
                circleCol.enabled = false;
            }
        }
        Destroy(gameObject);
    }
}

