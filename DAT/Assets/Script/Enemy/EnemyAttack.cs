using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    PlayerController playerCtrl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject objParent = transform.parent.gameObject;
        GameObject enemy = objParent.GetComponent<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("attack!");
            GameObject playerObj = other.gameObject;
            playerCtrl = playerObj.GetComponent<PlayerController>();
            playerCtrl.Damaged(10);
        }
    }
}
