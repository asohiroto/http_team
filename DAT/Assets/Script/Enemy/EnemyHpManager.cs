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
    // アプローチはこれで良さそう
    // WaveManager側でこれをおこなう

    //[SerializeField] private bool BossObject;
    //[SerializeField] private NextScene nextScene = NextScene.None;
    //[SerializeField] private float TimeTochangeScene;   // 次のシーンに行くまでの猶予時間
    /*
    private void Update()
    {
        if (!BossObject) return;    // ボスオブジェクトじゃないなら
        if (!CheckDie()) return;    // 体力が0より多いいなら
        if (nextScene == NextScene.None) return;    // 次のシーンがNone(初期値)なら

        TimeTochangeScene -= Time.deltaTime;
        if (TimeTochangeScene < 0)  // 指定時間経過したら
        {
            ChangeScene();
        }
    }*/

    private bool CheckDie()
    {
        if (hp <= 0) { return true; }
        else { return false; }
    }
    /*
    private void ChangeScene()
    {
        switch (nextScene)
        {
            case NextScene.Level2Scene:
                SceneManager.LoadScene("Level2Scene");
                break;

            case NextScene.Level3Scene:
                SceneManager.LoadScene("Level3Scene");
                break;
            
            case NextScene.ClearScene:
                SceneManager.LoadScene("ClesrScene");
                break;
        }
    }*/

    public int GetCurrentHp()
    {
        return hp;
    }

    public void EnemyDamaged(int dmg)
    {
        takeDamage = true;
        hp -= dmg;
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
