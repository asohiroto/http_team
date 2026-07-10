using UnityEngine;

public class Boss1AttackCol : MonoBehaviour
{
    PlayerController playerCtrl;
    private int attackPower;
    private float startupTimer;
    private float colTimer;
    private bool isAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // コライダー無効化
        GetComponent<Collider>().enabled = false;
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttack) return;
        startupTimer -= Time.deltaTime;

        if (startupTimer > 0) return;
        colTimer -= Time.deltaTime;
        GetComponent<Collider>().enabled = true;

        if (colTimer > 0) return;
        finishAttackCol();
    }

    public void InitAttackCol(int atkPower, float attackStartUpDuration, float onColDuration)
    {
        attackPower = atkPower;
        startupTimer = attackStartUpDuration;
        colTimer = onColDuration;
        isAttack = true;
    }

    private void finishAttackCol()
    {
        GetComponent<Collider>().enabled = false;
        isAttack = false;
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
    
}
