using UnityEngine;

public class PlayerAttackReach : MonoBehaviour
{
    GameObject enemyObj;
    int enemyCount;
    GameObject playerObj;
    PlayerController playerController;

    void Start()
    {
        enemyCount = 0;
        playerObj = transform.parent.gameObject;
        playerController = playerObj.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyCount <= 0)
        {
            enemyCount = 0;
            playerController.canAttack = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            playerController.canAttack = true;
            enemyCount++;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemyCount--;
        }
    }
}
