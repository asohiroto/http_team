using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEngine.InputManagerEntry;

public class skill_manager : MonoBehaviour
{
    PlayerController player;
    HandManager hand;
    CraftManager craft;

    [SerializeField] int healAmount; // 回復量
    [SerializeField] int enhanceAmount; // 強化量
    [SerializeField] int enhanceTime; // 効果時間
    [SerializeField] int waitTime; // 使用待機時間

    int time = 0;
    int enhanceFlag = 0; // 強化状態の判定
    int hyperFlag = 0;

    public int discardInd1 = -1;
    public int discardInd2 = -1;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hand = GameObject.Find("HandManager").GetComponent<HandManager>();
        craft = GameObject.Find("CraftManager").GetComponent<CraftManager>();

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

        if (Keyboard.current.pKey.wasPressedThisFrame) // 【テスト用】　pを押すと体力を減らす
        {
            player.playerHP -= 15;
            Debug.Log("playerHP : " + player.playerHP);
        }

    }

    public async Task Enhance(int ind) // 攻撃力強化 ID->0
    {
        int cardID = 0;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime) // waitTime秒分だけ左クリックの入力を待つ
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
                    Debug.Log("同名の強化は重ね掛けできないよ？");
                }
            }
            else
            {
                Debug.Log("スキップしたよ");
            }
        }

        CraftMethod(cardID, ind);
    }

    public async Task Heal(int ind) // 回復 ID->1
    {
        int cardID = 1;

        if (craft.craftFrag == 0)
        {
            int waitFrames = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitFrames < 60 * waitTime)
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

        CraftMethod(cardID, ind);
    }

    public async Task Slash(int ind) // 強斬り ID->2
    {
        int cardID = 2;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
            {
                waitTimer++;

                await Task.Yield();
            }


            hand.DisCard(ind);
        }

        CraftMethod(cardID, ind);
    }

    public async Task FireBall(int ind) // 火の玉を飛ばす ID->3
    {
        int cardID = 3;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
            {
                waitTimer++;

                await Task.Yield();
            }


            hand.DisCard(ind);
        }

        CraftMethod(cardID, ind);
    }

    public async Task FireSlash(int ind) // 炎斬り　ID->4
    {
        int cardID = 4;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
            {
                waitTimer++;

                await Task.Yield();
            }


            hand.DisCard(ind);
        }

        CraftMethod(cardID, ind);
    }

    public async Task HyperMode(int ind) // 超強化　ID->5
    {
        int cardID = 5;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime) // waitTime秒分だけ左クリックの入力を待つ
            {
                waitTimer++;

                await Task.Yield();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (hyperFlag == 0) // 非強化状態なら使用可能
                {
                    player.attackDamage += enhanceAmount;

                    enhanceFlag = 1;
                    time = 0;


                    Debug.Log("power = " + player.attackDamage);

                    hand.DisCard(ind);
                }
                else
                {
                    Debug.Log("同名の強化は重ね掛けできないよ？");
                }

                if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
            {
                waitTimer++;

                await Task.Yield();
            }


            hand.DisCard(ind);
        }

        CraftMethod(cardID, ind);
    }

    void CraftMethod(int id, int ind) // カード合成の関数
    {
        if (craft.craftFrag == 2) // 保存されたIDを呼び出し、素材となったカードを破壊し、空いたスペースにカードを合成
        {
            int craftResult = craft.CraftItems(craft.material1, id);

            discardInd2 = ind;
            craft.craftFrag = 0;

            if (craftResult < 0)
            {
                return;
            }

            hand.DisCard(discardInd1);
            hand.DisCard(discardInd2);

            if (discardInd1 > discardInd2)
            {
                discardInd1 = discardInd2;
            }

            GameObject obj = hand.CardGenerate(craftResult, discardInd1);
            hand.ButtonListener(craftResult, obj, discardInd1);
        }

        if (craft.craftFrag == 1) // 場所とIDを保存する
        {
            discardInd1 = ind;
            craft.SettingMaterial1(id);
        }
    }

}

