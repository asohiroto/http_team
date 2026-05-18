using UnityEngine;
using UnityEngine.InputSystem;

public class skill_manager : MonoBehaviour
{

    heal_manager heal;
    enhance_manager enhance;
    player_manager_kari player;
    HandManager hand;

    [SerializeField] int effectTime; // 強化の効果時間
    [SerializeField] int heal_amount; // 回復量

    int time = 0; // 時間カウンタ
    int enhanceFlag = 0; // 攻撃力強化のフラグ

    void Start()
    {
        heal = GetComponent<heal_manager>();
        enhance = GetComponent<enhance_manager>();
        hand = GameObject.Find("HandManager").GetComponent<HandManager>();
        player = GameObject.Find("Square").GetComponent<player_manager_kari>();

    }

    // Update is called once per frame
    void Update()
    {
        time++;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            switch (hand.cardUseId[hand.cardUse]) // カーソル位置に応じて発動分岐
            {
                case 0: // 強化発動
                    if (enhanceFlag == 0)
                    {
                        enhance.Enhance();
                        time = 0;
                        enhanceFlag = 1; // フラグ立て
                    }

                    Debug.Log(hand.cardUseId[hand.cardUse]);

                    Debug.Log("選択中　1");

                    break;

                case 1: // 回復発動
                    
                    if (player.hp <= player.maxHp - heal_amount) // 体力の減少量が回復量よりも大きければ、回復量分だけ回復
                    {
                        heal.Heal(heal_amount);


                    }
                    else // 体力の減少量が回復量よりも少なければ、体力の最大値になるように回復
                    {
                        heal.Heal(player.maxHp - player.hp);
                    }

                    Debug.Log("選択中　2");
                    Debug.Log(hand.cardUseId[hand.cardUse]);

                    break;

                case 2:

                    Debug.Log("選択中　3");
                    Debug.Log(hand.cardUseId[hand.cardUse]);

                    break;

                case 3:

                    Debug.Log("選択中　4");
                    Debug.Log(hand.cardUseId[hand.cardUse]);

                    break;

                default:
                    break;

            }

        }

        if (time > effectTime * 60 && enhanceFlag == 1) // 強化から２秒経過かつ、フラグが立っていればリセット
        {
            enhance.Enhance_reset();
            enhanceFlag = 0; // フラグ下げ

        }
    }
}
