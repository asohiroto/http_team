using UnityEngine;

public class PlayerAttackReach : MonoBehaviour
{
    GameObject enemyObj;
    int enemyCount;

    void Start()
    {
        enemyCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            
        }
    }
}
