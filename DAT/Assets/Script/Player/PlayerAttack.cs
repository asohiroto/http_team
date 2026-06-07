using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //EnemyController eneController;
    EnemyC eneController;
    PlayerController playerController;
    GameObject enemyObj;
    GameObject playerObj;

    [SerializeField] Sprite[] attackSprite;
    float animTime = 0.05f;
    SpriteRenderer spriteRenderer;
    public float attackDamage;

    [SerializeField] int startColFrame;
    [SerializeField] int endColFrame;
    BoxCollider2D boxCol;

    // アタックエフェクトのアニメーションと当たり判定の管理------------------

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerObj = GameObject.Find("Player");
        playerController = playerObj.GetComponent<PlayerController>();
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.enabled = false;

        StartCoroutine(AttackAnim());
        
    }

    void Update()
    {
        attackDamage = playerController.attackDamage;
        transform.position = playerObj.transform.position + playerController.attackDir * playerController.distanceAttackFX;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemyObj = col.gameObject;
            eneController = enemyObj.GetComponent<EnemyC>();
            eneController.EnemyDamaged(attackDamage);
        }
    }

    IEnumerator AttackAnim()
    {
        for(int i = 0; i < attackSprite.Length; i++)
        {
            spriteRenderer.sprite = attackSprite[i];

            yield return new WaitForSeconds(animTime);
            if (i+ 1 >= startColFrame && i + 1 <= endColFrame)
            {
                boxCol.enabled = true;
            }
            else
            {
                boxCol.enabled = false;
            }
        }
        Destroy(gameObject);
    }
}
