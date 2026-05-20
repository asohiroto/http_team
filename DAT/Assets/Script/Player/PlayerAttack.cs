using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    EnemyController eneController;
    int attackDamage = 1;
    GameObject enemyObj;

    [SerializeField] Sprite[] attackSprite;
    float animTime = 0.05f;
    int idx = 0;
    SpriteRenderer spriteRenderer;

    // アタックエフェクトのアニメーションと当たり判定の管理------------------

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AttackAnim());
    }

    void Update()
    {
        
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
        
    }
}
