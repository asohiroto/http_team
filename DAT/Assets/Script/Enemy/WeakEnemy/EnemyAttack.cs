using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    PlayerController playerCtrl;
    GameObject objParent;
    EnemyController enemyCtrl;
    private int attackPower;

    //private float attackPower => ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        objParent = transform.parent.gameObject;
        //Debug.Log(objParent);

        enemyCtrl = objParent.GetComponent<EnemyController>();
        //Debug.Log(enemyCtrl);

        attackPower = enemyCtrl.GetAttackPower();
        //Debug.Log("attackPower : " + attackPower);
        */
    }

    /// <summary>
    /// 攻撃力をセット
    /// </summary>
    /// <param name="attackPower"></param>
    public void SetAttackPower(int dmg)
    {
        attackPower = dmg;
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            playerCtrl = other.GetComponent<PlayerController>();
            //playerCtrl.Damaged(40);
            //StartCoroutine(playerCtrl.Damaged(attackPower));
            playerCtrl.Damaged(attackPower);
        }
    }

    private void OnDrawGizmos()
    {
        // ギズモの色を緑色に設定
        Gizmos.color = Color.green;
        Vector2 size = new Vector2(1f, 1f);

        // 四角形の枠線を描画
        Vector3 center = transform.position;
        Vector3 drawSize = new Vector3(size.x, size.y, 0.001f);
        Gizmos.DrawWireCube(center, drawSize);
    }
}


