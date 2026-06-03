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
        objParent = transform.parent.gameObject;
        //Debug.Log(objParent);
        enemyCtrl = objParent.GetComponent<EnemyController>();
        //Debug.Log(enemyCtrl);
        attackPower = enemyCtrl.AttackPower;
        //Debug.Log(attackPower);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("attack!");
            GameObject playerObj = other.gameObject;
            playerCtrl = playerObj.GetComponent<PlayerController>();
            //playerCtrl.Damaged(40);
            //StartCoroutine(playerCtrl.Damaged(attackPower));
            playerCtrl.Damaged(attackPower);
        }
    }
}
