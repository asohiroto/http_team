using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector2 destination;
    [SerializeField] private int dmg;
    [SerializeField] private float angle;
    [SerializeField] private float OnColTime;
    [SerializeField] private bool isMove;

    PlayerController playerCtrl;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 発射音鳴らす

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    // 到達を検知してコラーダー出現＆サウンド
    private void OnCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;

        OnColTime -= Time.fixedDeltaTime;
    }

    private void Move()
    {
        if (!isMove) { return; }
        transform.position = Vector2.MoveTowards(
            transform.position,
            destination,
            moveSpeed * Time.fixedDeltaTime);

        if ((Vector2)transform.position == destination)
        {
            OnCollider();

            if (OnColTime < 0)
            {
                isMove = false;
                Destroy(gameObject);
            }
        }
    }

    private void SetAngle()
    {
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }


    /// <summary>
    /// 矢の方向、目的地をセットする
    /// </summary>
    /// <param name="dir">移動方向(正規化済み)</param>
    /// <param name="dest">目的地</param>
    public void SetArrowAttack(Vector2 dir, Vector2 dest, int damage)
    {
        // collider無効化
        GetComponent<BoxCollider2D>().enabled = false;
        // 方向(回転用)
        direction = dir;
        // 目的地(移動用)
        destination = dest;
        // 攻撃力
        dmg = damage;

        isMove = true;
        SetAngle();
        Debug.Log("Arrow");
    }
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            playerCtrl = other.GetComponent<PlayerController>();
            playerCtrl.Damaged(dmg);
        }
    }
}
