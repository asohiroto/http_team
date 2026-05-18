using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    EnemyController eneController;
    int attackDamage = 1;
    GameObject enemyObj;

    void Start()
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
}
