using System;
using UnityEngine;

public class enhance_manager : MonoBehaviour
{
    player_manager_kari player; // Hpを呼び出し

    void Start()
    {
        player = GameObject.Find("Square").GetComponent<player_manager_kari>(); // 捜すやつ

    }


    public void Enhance() // プレイヤーの攻撃力を20増加する
    {
        player.power += 20;
        Debug.Log("Power : " + player.power);

    }

    public void Enhance_reset() // プレイヤーの攻撃力をリセットする
    {
        player.power = player.firstPower;
        Debug.Log("Power : " + player.power);
    }

    void Update()
    {

    }

}
