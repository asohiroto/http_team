using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Absorb : MonoBehaviour
{
    [SerializeField] GameObject absorbPrefab;

    [SerializeField] int absorbDamage;
    [SerializeField] int absorbHealAmount;

    int absorbCount = 0;
    int absorbTime = 1;

    public bool absorbFlag = false;

    GameObject absorb;

    PlayerController player;
    HandManager hand;
    SkillManager skill;
    SkillAttack skillAttack;

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
        absorbCount++;

        if (player.onAttack == true && absorbCount >= 60 * absorbTime)
        {
            player.onAttack = false;
            Destroy(absorb);
        }

        if (absorbFlag) // アブソーブがヒットしたら体力吸収
        {
            if (player.playerHP > player.maxPlayerHP - absorbHealAmount) // 回復して最大HPを超える場合は、最大HPまで回復
            {
                player.playerHP = player.maxPlayerHP;
            }
            else if (player.playerHP == player.maxPlayerHP)
            {
                Debug.Log("元気すぎやしないかい？");
            }
            else
            {
                player.playerHP += absorbHealAmount;
            }

            Debug.Log(player.playerHP);
        }
    }

    public void Effect(int ind)
    {
        player.SlashTypeAndDir(absorbPrefab, ref absorb); // アブソーブを生成して使う方向を決定する
        skillAttack = absorbPrefab.GetComponent<SkillAttack>();
        skillAttack.attackDamage = player.attackDamage + absorbDamage; // アブソーブのアタックダメージを代入

        hand.DisCard(ind);

        player.onAttack = true;

        absorbCount = 0;

    }

}
