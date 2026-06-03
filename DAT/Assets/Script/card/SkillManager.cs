using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEngine.InputManagerEntry;

public class SkillManager : MonoBehaviour
{
    PlayerController player;
    HandManager hand;
    CraftManager craft;
    CoinManager coin;



    public int discardInd1 = -1;                // 捨てるカードの住所その１
    public int discardInd2 = -1;                //                   その２

    Vector3 mousePosScreen = new Vector3();     // スクリーン座標系でのマウスの位置
    public Vector3 mousePosWorld = new Vector3();      // ワールド座標系でのマウスの位置
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Card");

        player = GameObject.Find("Player").GetComponent<PlayerController>();

        foreach (GameObject obj in objs) // それぞれ探す
        {
            if (hand == null) hand = obj.GetComponent<HandManager>();

            if (craft == null) craft = obj.GetComponent<CraftManager>();

            if (hand != null && craft != null) break;
        }

        coin = GameObject.Find("CoinManager").GetComponent<CoinManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // 各座標を入力
        mousePosScreen.x = Mouse.current.position.x.ReadValue();
        mousePosScreen.y = Mouse.current.position.y.ReadValue();
        mousePosScreen.z = -Camera.main.transform.position.z;

        mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);　// 座標系の変換

    }

    public void AttackCancell() // 攻撃力の初期化
    {
        player.attackDamage = player.defaultAttackDamage;
        Debug.Log("Power : " + player.attackDamage);
    }

    public void SpeedCancell() // 移動速度の初期化
    {
        player.speed = player.defaultSpeed;

        Debug.Log("Speed : " + player.speed);
    }

    public void BallMove(GameObject preFab, Vector2 dest, float speed, Vector2 mousePos,ref bool flag) // ボール系の挙動
    {
        if (preFab != null)
        {


            preFab.transform.Translate(dest * speed, Space.World); // ファイアーボール自身を目的地に向かってすすませる

            float dist = Vector2.Distance(preFab.transform.position, mousePos);

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


