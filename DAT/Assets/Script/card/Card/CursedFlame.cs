using UnityEngine;
using UnityEngine.XR;

public class CursedFlame : MonoBehaviour
{
    [SerializeField] int cfSpeed;
    [SerializeField] int curseAmount;

    [SerializeField] GameObject cursedFlame;

    public GameObject cursedFlamePrefab;

    Vector2 destPos = Vector2.zero;
    Vector2 cfPos = Vector2.zero;

    bool cfFlag = false;

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
        if (cfFlag) skill.BallMove(cursedFlamePrefab, destPos, cfSpeed, cfPos, ref cfFlag); // ファイアーボール使用後の挙動
    }

    public void Effect(int ind, Vector2 mousePos)
    {
        if (!cfFlag)
        {
            player.playerHP -= curseAmount * 2;

            destPos.x = mousePos.x - player.currentPos.x;
            destPos.y = mousePos.y - player.currentPos.y;

            cfPos = destPos;

            destPos.Normalize();

            // オブジェクトを作成する
            GameObject obj = Instantiate(cursedFlame);
            obj.transform.position = player.currentPos;
            obj.transform.name = ("CursedFlame");

            cursedFlamePrefab = obj;

            cfFlag = true; // フラグを立てる

            hand.DisCard(ind);
        }
    }
}
