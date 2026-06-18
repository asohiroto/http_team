using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillAttack : MonoBehaviour
{
    EnemyController eneController;
    PlayerController playerController;

    GameObject enemyObj;
    GameObject playerObj;

    [SerializeField] Sprite[] attackSprite;

    [SerializeField] float animTime = 0.05f;

    SpriteRenderer spriteRenderer;
    Image image;

    public float attackDamage;

    // アタックエフェクトのアニメーションと当たり判定の管理------------------

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        playerObj = GameObject.Find("Player");
        playerController = playerObj.GetComponent<PlayerController>();

        StartCoroutine(AttackAnim());

    }

    void Update()
    {
        attackDamage = playerController.attackDamage;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemyObj = col.gameObject;
            eneController = enemyObj.GetComponent<EnemyController>();
            eneController.EnemyDamaged(attackDamage);
        }
    }

    IEnumerator AttackAnim()
    {
        for (int i = 0; i < attackSprite.Length; i++)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = attackSprite[i];
            }
            else
            {
                image.sprite = attackSprite[i];
            }

            yield return new WaitForSeconds(animTime);
        }
        Destroy(gameObject);
    }
}
