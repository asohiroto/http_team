using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class WeakTorch : MonoBehaviour
{
    [SerializeField] private Sprite[] idleSpr;          // 待機時
    [SerializeField] private Sprite[] moveSpr;          // 移動時
    [SerializeField] private Sprite[] horAttackSpr;     // 横方向の攻撃
    [SerializeField] private Sprite[] verAttackSpr;     // 縦方向の攻撃


    [SerializeField] private int timer;

    private bool isMove = false;
    [SerializeField] private bool isAttack = false;

    SpriteRenderer spr;
    EnemyController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.spr = GetComponent<SpriteRenderer>();
        controller = GetComponent<EnemyController>();
    }
        

    private void FixedUpdate()
    {
        Attack();
        Animation();
    }

    private void Attack()
    {
        if (controller.CanAttack)
        {
            isAttack = true;
        }
    }

    private void Animation()
    {
        if (isAttack)
        {

        }
        else if (isMove)
        {

        }
        else
        {
            IdelAnimation();
        }
    }

    // Update is called once per frame
    private void IdelAnimation()
    {
        // staySpr の枚数カウント
        int count = idleSpr.Length;

        this.spr.sprite = idleSpr[timer / 5 % count];
        timer++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttack)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("attack!");
                GameObject playerObj = other.gameObject;

            }
        }
    }
}
