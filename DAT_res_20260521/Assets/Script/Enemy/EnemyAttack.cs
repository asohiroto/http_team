using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    PlayerController playerCtrl;
    GameObject objParent;
    Component  enemyCtrl;
    private int attackPower = 0;

    //private float attackPower => ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objParent = transform.parent.gameObject;
        Debug.Log(objParent);
        enemyCtrl = objParent.GetComponent<EnemyController>();
        Debug.Log(enemyCtrl);
    }

    public void AttackDamage(int dmg)
    {
        attackPower = dmg;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("attack!");
            GameObject playerObj = other.gameObject;
            playerCtrl = playerObj.GetComponent<PlayerController>();
            playerCtrl.Damaged(attackPower);
        }
    }
}
