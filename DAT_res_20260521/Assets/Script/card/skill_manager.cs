using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Threading.Tasks;

public class skill_manager : MonoBehaviour
{
    PlayerController player;
    HandManager hand;

    [SerializeField] int healAmount; // 回復量
    [SerializeField] int enhanceAmount; // 強化量
    [SerializeField] int enhanceTime; // 効果時間
    int time = 0;
    int enhanceFlag = 0;


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hand = GameObject.Find("HandManager").GetComponent<HandManager>();

    }

    // Update is called once per frame
    void Update()
    {
        time++;

        if (enhanceFlag == 1 && time > enhanceTime * 60) // 効果時間経過後、攻撃力を最初の状態に戻す
        {
            player.attackDamage = player.defaultAttackDamage;
            Debug.Log("Power : " + player.attackDamage);

            enhanceFlag = 0;
        }


    }


    public void Slash(int ind) // 強斬り
    {
        hand.DisCard(ind);
    }

    public async void Heal(int ind) // 回復
    {
        int waitFrames = 0;

        while (!Mouse.current.rightButton.wasPressedThisFrame && waitFrames < 120)
        {
            waitFrames++;

            await Task.Yield();
        }


        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (player.playerHP > player.maxPlayerHP - healAmount) // 回復して最大HPを超える場合は、最大HPまで回復
            {
                player.playerHP = player.maxPlayerHP;
            }
            else
            {
                player.playerHP += healAmount;
            }

            hand.DisCard(ind);

            Debug.Log(player.playerHP);
        }
        else
        {
            Debug.Log("スキップしたよ");
        }

    }

    public async Task Enhance(int ind) // 強化
    {
        int waitTimer = 0;

        while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 120)
        {
            waitTimer++;

            await Task.Yield();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (enhanceFlag == 0)
            {
                player.attackDamage += enhanceAmount;

                enhanceFlag = 1;
                time = 0;

                hand.DisCard(ind);

            }

            Debug.Log("power = "+ player.attackDamage);

        }
        else
        {
            Debug.Log("スキップしたよ");
        }

    }

    public void FireBall(int ind) // 火の玉を飛ばす
    {
        hand.DisCard(ind);
    }


}

