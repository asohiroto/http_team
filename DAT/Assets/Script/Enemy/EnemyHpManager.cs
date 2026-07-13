using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHpManager : MonoBehaviour
{
    [SerializeField] private int hp;
    private bool takeDamage = false;

    // このオブジェクトがボスだった場合、
    // 体力が０以下になると次のレベルのシーンに遷移します
    private enum NextScene
    {
        None,
        Level2Scene,
        Level3Scene,
        ClearScene
    }

    private void CheckDie()
    {/*
        if (hp <= 0)
        {
            GameObject.FindWithTag("EnemySpawner").
                GetComponent<WaveManager>().
                NotifyBossDefeated();
        }*/
    }

    public int GetCurrentHp()
    {
        return hp;
    }

    public void EnemyDamaged(int dmg)
    {
        takeDamage = true;
        hp -= dmg;

        CheckDie();
    }

    public bool TakeDamage()
    {
        if (takeDamage)
        {
            takeDamage = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
