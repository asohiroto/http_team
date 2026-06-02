using System;
using UnityEngine;

public class EnemyC : MonoBehaviour
{
    [SerializeField] private int eHp = 10;          // 体力
    [SerializeField] private float moveSpeed = 1f;  // 移動速度
    [SerializeField] private int attackPower = 3;   // 攻撃力

    [SerializeField] private bool alwaysFindPlayer = false; // どの距離でもPlayerを発見する
    [SerializeField] private float findDist = 3f;   // Playerを発見する距離
    [SerializeField] private float lostDist = 5f;   // Playerを見失う距離
    [SerializeField] private float stopDist = 0.8f; // この距離で立ち止まる
    [SerializeField] private float attackRange = 1f;    // 攻撃のレンジ
    

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
