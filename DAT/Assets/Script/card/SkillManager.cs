using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    PlayerController player;
    HandManager hand;
    CraftManager craft;

    // ボール系のカードの挙動を管理するためのフラグ
    public bool useFlag = true;

    // マウスの座標を保存するための変数
    Vector3 mousePosScreen = new Vector3();
    public Vector3 mousePosWorld = new Vector3();

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Card");
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        foreach (GameObject obj in objs)
        {
            if (hand == null) hand = obj.GetComponent<HandManager>();

            if (craft == null) craft = obj.GetComponent<CraftManager>();

            if (hand != null && craft != null) break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // マウスのスクリーン座標を取得
        mousePosScreen.x = Mouse.current.position.x.ReadValue();
        mousePosScreen.y = Mouse.current.position.y.ReadValue();
        mousePosScreen.z = -Camera.main.transform.position.z;

        // スクリーン座標をワールド座標に変換
        mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
    }

    // 攻撃力の初期化
    public void AttackCancell()
    {
        player.attackDamage = player.defaultAttackDamage;
        Debug.Log("Power : " + player.attackDamage);
    }

    // 移動速度の初期化
    public void SpeedCancell()
    {
        player.speed = player.defaultSpeed;

        Debug.Log("Speed : " + player.speed);
    }

    // ボール系の挙動
    public void BallMove(GameObject preFab, Vector2 dest, float speed, Vector2 mousePos,ref bool flag)
    {
        if (preFab != null)
        {
            // プレハブを目的地に向かって移動させる
            preFab.transform.Translate(dest * speed, Space.World);

            // プレハブとマウスの距離を計算
            float dist = Vector2.Distance(preFab.transform.position, mousePos);

            // プレハブとマウスの距離が一定以上になったらプレハブを消す
            if (dist > 20.0f)
            {
                dist = 0f;
                flag = false;
                Destroy(preFab);
            }
        }
        else
        {
            flag = false;
        }
    }
}


