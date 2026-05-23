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
    [SerializeField] int waitTime; // 使用待機時間

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

        if(Keyboard.current.pKey.wasPressedThisFrame) // 【テスト用】　pを押すと体力を減らす
        {
            player.playerHP -= 15;
            Debug.Log("playerHP : " + player.playerHP);
        }

    }


    public async Task Slash(int ind) // 強斬り
    {
        int waitTimer = 0;

        while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
        {
            waitTimer++;

            await Task.Yield();
        }


        hand.DisCard(ind);
    }

    public async Task Heal(int ind) // 回復
    {
        int waitFrames = 0;

        while (!Mouse.current.rightButton.wasPressedThisFrame && waitFrames < 60 * waitTime) // waitTime秒分だけ左クリックの入力を待つ
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

    public async Task Enhance(int ind) // 攻撃力強化
    {
        int waitTimer = 0;

        while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
        {
            waitTimer++;

            await Task.Yield();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (enhanceFlag == 0) // 非強化状態なら使用可能
            {
                player.attackDamage += enhanceAmount;

                enhanceFlag = 1;
                time = 0;


                Debug.Log("power = " + player.attackDamage);

                hand.DisCard(ind);
            } 
            else
            {
                Debug.Log("同名強化は重ね掛けできないよ？");
            }

        }
        else
        {
            Debug.Log("スキップしたよ");
        }

    }

    public async Task FireBall(int ind) // 火の玉を飛ばす
    {
        int waitTimer = 0;

        while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
        {
            waitTimer++;

            await Task.Yield();
        }


        hand.DisCard(ind);
    }


}

