using UnityEngine;
using UnityEngine.InputSystem;

public class player_manager_kari : MonoBehaviour
{
    [SerializeField] public int maxHp; // 最大HP
    public int hp; // 現在のHP

    [SerializeField] public int firstPower; // 通常時の攻撃力
    public int power; // 現在の攻撃力
    void Start()
    {
        Application.targetFrameRate = 60;

        hp = maxHp; // ゲーム開始時に最大HPに設定
        firstPower = power; // ゲーム開始時に通常攻撃力に設定
    }

    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame) // 【テスト用】HPを減らして動作を確認
        {
            hp -= 15;
            Debug.Log("Now HP :" + hp);
        }
    }
}
