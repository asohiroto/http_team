using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    EnemyController eneController;
    PlayerController playerController;
    GameObject enemyObj;
    GameObject playerObj;

    [SerializeField] Sprite[] attackSprite;
    float animTime = 0.05f;
    SpriteRenderer spriteRenderer;

    public int attackDamage;

    // アタックエフェクトのアニメーションと当たり判定の管理------------------

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        for(int i = 0; i < attackSprite.Length; i++)
        {
            spriteRenderer.sprite = attackSprite[i];

            yield return new WaitForSeconds(animTime);
        }
        Destroy(gameObject);
    }
}
