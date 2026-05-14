using System;
using UnityEngine;

public class heal_manager : MonoBehaviour
{
    player_manager_kari player; // Hpを呼び出し

    void Start()
    {
        player = GameObject.Find("Square").GetComponent<player_manager_kari>(); // 捜すやつ

    }


    public void Heal() // プレイヤーのHPを20増加する
    {
        player.Hp += 20;
        Debug.Log("Hp: " + player.Hp);

    }

    void Update()
    {
    }

}