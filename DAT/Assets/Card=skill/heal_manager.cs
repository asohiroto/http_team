using System;
using UnityEngine;

public class enhance_manager : MonoBehaviour
{

    [SerializeField] public int player_Hp = 100;

    public void Heal()
    {
        player_Hp += 20;
    }

    void Update()
    {
        Debug.Log(player_Hp);
    }

}