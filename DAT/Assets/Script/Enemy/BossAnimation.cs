#if false
using System.Collections.Generic;
using UnityEngine;

// 列挙型 enum
// アニメーションの状態
// 待機、移動、攻撃 横、攻撃 上、攻撃 下
public enum BossAnimState { Idle, Walk, SideAttack, LowerAttack, UpperAttack, Init }

// 構造体 struct
[System.Serializable]
public struct BossAnimationData
{
    public BossAnimState state;        // 状態 (待機/移動/攻撃など)
    public Sprite[] sprites;            // 使用するスプライトの配列
    public int frameRate;               // アニメーションの再生速度 (fps)
    public bool isLoop;                 // ループ再生するかどうか
}

// スプライトの変更のみに集中する
public class BossAnimation : MonoBehaviour
{
    [SerializeField] private List<BossAnimationData> bossAnimationDatas = new List<BossAnimationData>();
    // 辞書機能
    private Dictionary<BossAnimState, BossAnimationData> bossAnimDictionary = new Dictionary<BossAnimState, BossAnimationData>();

    // current = 現在
    private BossAnimationData currentBossActiveData;
    [SerializeField] private BossAnimState currentBossAnimState = BossAnimState.Init;
    [SerializeField] private int currentFrame = 0;   // 現在のフレーム
    [SerializeField] private float Animtimer = 0.0f;
    [SerializeField] private float timePerFrame = 0.0f;
    [SerializeField] private float blinkTimer = 0.0f;   // 被ダメージ時の点滅処理
    [SerializeField] private float blinkInterval = 0.2f;
    [SerializeField] private bool isBlinking = false;
    [SerializeField] private bool isRed = false;

    BossController bossCtrl;
    private SpriteRenderer spr;

    // 先に済ませないとtimePerFrameがInfinityになる(0で除算してしまう)
    private void Awake()
    {
        // インスペクターで設定したリストをループで回し、Dictionaryに登録する
        foreach (var data in bossAnimationDatas)
        {
            // まだDictionaryにそのステートが登録されていなければ追加
            if (!bossAnimDictionary.ContainsKey(data.state))
            {
                bossAnimDictionary.Add(data.state, data);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossCtrl = GetComponent<BossController>();

        spr = GetComponent<SpriteRenderer>();

        ChangeState(BossAnimState.Idle);

        blinkTimer = blinkInterval;
    }

    // Update is called once per frame
    void Update()
    {
        Animtimer -= Time.deltaTime;

        if (Animtimer < 0)
        {
            Animtimer += timePerFrame;

            int nextFrame = currentFrame + 1;

            // 最後のフレームのとき
            if (nextFrame >= currentBossActiveData.sprites.Length)
            {
                if (currentBossActiveData.isLoop == true)
                {
                    // ループ処理
                    currentFrame = 0;
                    Animtimer = 0;
                }
                else
                {
                    //ChangeState(EnemyAnimState.Idle);

                    bossCtrl.OnAttackAnimationFinished();
                }
            }
            else
            {
                currentFrame = nextFrame;
            }

            this.spr.sprite = currentBossActiveData.sprites[currentFrame];
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
    public void ChangeState(BossAnimState changedState)
    {
        if (currentBossAnimState == changedState) return;
        currentBossAnimState = changedState;

        // Dictionaryから指示されたステートのデータを特定して保持する
        if (bossAnimDictionary.TryGetValue(changedState, out var foundData))
        {
            currentBossActiveData = foundData;
            currentFrame = 0;
            Animtimer = 0f;

            //Debug.Log("statusを変更しました：");

            // フレームレートを更新
            timePerFrame = 1f / currentBossActiveData.frameRate;
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
#endif