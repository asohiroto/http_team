using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class FireBall : MonoBehaviour
{
    [SerializeField] int fbSpeed;

    [SerializeField] GameObject fireBall;

    public GameObject fireBallPrefab;

    Vector2 destPos = Vector2.zero;
    Vector2 fbPos = Vector2.zero;

    bool fbFlag = false;

    PlayerController player;
    HandManager hand;
    SkillManager skill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hand = GameObject.Find("HandManager").GetComponent<HandManager>();
        skill = GameObject.Find("SkillManager").GetComponent<SkillManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fbFlag) skill.BallMove(fireBallPrefab, destPos, fbSpeed, fbPos, ref fbFlag); // ファイアーボール使用後の挙動
    }

    public void Effect(int ind, Vector2 mousePos)
    {
        if (!fbFlag)
        {

            destPos.x = mousePos.x - player.currentPos.x;
            destPos.y = mousePos.y - player.currentPos.y;

            fbPos = destPos;

            // マウスがさしたポイントへの単位ベクトルを作成
            destPos.Normalize();

            float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;

            // オブジェクトを作成する
            GameObject obj = Instantiate(fireBall);
            obj.transform.position = player.currentPos;
            obj.transform.name = ("FireBall");
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);

            fireBallPrefab = obj;

            fbFlag = true; // フラグを立てる

            hand.DisCard(ind);
        }
    }
}
