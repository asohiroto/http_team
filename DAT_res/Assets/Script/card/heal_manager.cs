using System;
using Unity.VisualScripting;
using UnityEngine;

public class heal_manager : MonoBehaviour
{
    player_manager_kari player; // Hpを呼び出し

    void Start()
    {
        player = GameObject.Find("Square").GetComponent<player_manager_kari>(); // 捜すやつ

    }


    public void Heal(int heal_amount) // プレイヤーのhpをheal_amount分だけ増加する
    {
        player.hp += heal_amount;
        Debug.Log("Hp: " + player.hp);

    }

    void Update()
    {
    }

}