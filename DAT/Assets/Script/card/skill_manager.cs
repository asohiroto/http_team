using UnityEngine;
using UnityEngine.InputSystem;

public class skill_manager : MonoBehaviour
{

    heal_manager heal; 
    enhance_manager enhance;
    player_manager_kari player;
    [SerializeField] int effectTime; // 強化の効果時間
    [SerializeField] int heal_amount; // 回復量

    int time = 0; // 時間カウンタ
    int enhanceFlag = 0; // 攻撃力強化のフラグ

    void Start()
    {
        heal = GetComponent<heal_manager>();
        enhance = GetComponent<enhance_manager>();
        player = GameObject.Find("Square").GetComponent<player_manager_kari>();
    }

    // Update is called once per frame
    void Update()
    {
        time++;

        if (player.hp <= player.maxHp - heal_amount) // 体力の減少量が回復量よりも大きければ、回復量分だけ回復
        {
            if (Keyboard.current.zKey.wasPressedThisFrame)
            {
                heal.Heal(heal_amount);

            }
        }
        else // 体力の減少量が回復量よりも少なければ、体力の最大値になるように回復
        {
            if (Keyboard.current.zKey.wasPressedThisFrame)
                heal.Heal(player.maxHp - player.hp);
        }



        if(Keyboard.current.xKey.wasPressedThisFrame) // xをおすと攻撃力アップ
        {
            enhance.Enhance();
            time = 0;
            enhanceFlag = 1; // フラグ立て
        }

        if(time > effectTime * 60 && enhanceFlag == 1) // 強化から２秒経過かつ、フラグが立っていればリセット
        {
            enhance.Enhance_reset();
            enhanceFlag = 0; // フラグ下げ

        }
    }
}
