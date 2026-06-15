using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

// 列挙型 enum
// アニメーションの状態
// 待機、移動、攻撃 横、攻撃 上、攻撃 下
public enum EnemyAnimState { Idle, Walk, SideAttack, LowerAttack, UpperAttack, Init }

// 構造体 struct
[System.Serializable]
public struct AnimationData
{
    public EnemyAnimState state;        // 状態 (待機/移動/攻撃など)
    public Sprite[] sprites;            // 使用するスプライトの配列
    public int frameRate;               // アニメーションの再生速度 (fps)
    public bool isLoop;                 // ループ再生するかどうか
}

// スプライトの変更のみに集中する
public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private List<AnimationData> animationDatas = new List<AnimationData>();
    // 辞書機能
    private Dictionary<EnemyAnimState, AnimationData> animDictionary = new Dictionary<EnemyAnimState, AnimationData>();

    private AnimationData currentActiveData;
    [SerializeField] private EnemyAnimState currentAnimState = EnemyAnimState.Init;
    [SerializeField] private int currentFrame = 0;   // 現在のフレーム
    [SerializeField] private float Animtimer = 0.0f;
    [SerializeField] private float timePerFrame = 0.0f;
    [SerializeField] private float blinkTimer = 0.0f;   // 被ダメージ時の点滅処理
    [SerializeField] private float blinkInterval = 0.2f;
    [SerializeField] private bool isBlinking = false;
    [SerializeField] private bool isRed = false;

    EnemyController enemy;
    private SpriteRenderer spr;

    private void Awake()
    {
        // インスペクターで設定したリストをループで回し、Dictionaryに登録する
        foreach (var data in animationDatas)
        {
            // まだDictionaryにそのステートが登録されていなければ追加
            if (!animDictionary.ContainsKey(data.state))
            {
                animDictionary.Add(data.state, data);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<EnemyController>();

        spr = GetComponent<SpriteRenderer>();

        ChangeState(EnemyAnimState.Idle);

        blinkTimer = blinkInterval;
    }

    // Update is called once per frame
    void Update()
    {
        Animtimer -= Time.deltaTime;
        //blinkTimer -= Time.deltaTime;

        if (Animtimer < 0)
        {
            Animtimer += timePerFrame;

            int nextFrame = currentFrame + 1;

            // 最後のフレームのとき
            if (nextFrame >= currentActiveData.sprites.Length)
            {
                if (currentActiveData.isLoop == true)
                {
                    // ループ処理
                    currentFrame = 0;
                    Animtimer = 0;
                }
                else
                {
                    //ChangeState(EnemyAnimState.Idle);

                    enemy.OnAttackAnimationFinished();
                }
            }
            else
            {
                currentFrame = nextFrame;
            }

            this.spr.sprite = currentActiveData.sprites[currentFrame];
        }

        if (isBlinking)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer < 0)
            {
                spr.color = Color.white;
                isBlinking = false;
            }
        }
    }

    /// <summary>
    /// アニメーションの変更
    /// </summary>
    /// <param name="changedState">変更先のState</param>
    public void ChangeState(EnemyAnimState changedState)
    {
        if (currentAnimState == changedState) return;
        currentAnimState = changedState;

        // Dictionaryから指示されたステートのデータを特定して保持する
        if (animDictionary.TryGetValue(changedState, out var foundData))
        {
            currentActiveData = foundData;
            currentFrame = 0;
            Animtimer = 0f;

            //Debug.Log("statusを変更しました：");

            // フレームレートを更新
            timePerFrame = 1f / currentActiveData.frameRate;
        }
    }

    public void StartBlink()
    {
        isBlinking = true;
        blinkTimer = blinkInterval; // タイマーリセット
        isRed = false;
        spr.color = Color.red; // 最初は赤からスタート
    }

    // 点滅用の関数を作る
    // 引数に点滅回数を指定する
}
