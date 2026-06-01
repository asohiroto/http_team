using System.Collections.Generic;
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
    public EnemyAnimState state;            // 状態 (待機/移動/攻撃)
    public Sprite[] sprites;            // 使用するスプライトの配列
    public int frameRate;               // アニメーションの再生速度 (fps)
    public bool isLoop;                 // ループ再生するかどうか
}

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private List<AnimationData> animationDatas = new List<AnimationData>();
    // 辞書機能
    private Dictionary<EnemyAnimState, AnimationData> animDictionary = new Dictionary<EnemyAnimState, AnimationData>();

    // current = 現在
    private AnimationData animData;
    private AnimationData currentActiveData;
    [SerializeField] private EnemyAnimState currentState = EnemyAnimState.Init;
    [SerializeField] private int currentFrame = 0;   // 現在のフレーム
    [SerializeField] private float timer = 0;
    [SerializeField] private float timePerFrame = 0;

    EnemyController enemyCtrl;
    private SpriteRenderer spr;

    // 先に済ませないとtimePerFrameがInfinityになる(0で除算してしまう)
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
        enemyCtrl = GetComponent<EnemyController>();

        spr = GetComponent<SpriteRenderer>();

        ChangeState(EnemyAnimState.Idle);

        //timePerFrame = 1f / animData.frameRate; // 1フレームにかかる時間を計算
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timePerFrame)
        {
            timer -= timePerFrame;

            int nextFrame = currentFrame + 1;

            // 最後のフレームのとき
            if (nextFrame >= currentActiveData.sprites.Length)
            {
                if (currentActiveData.isLoop == true)
                {
                    // ループ処理
                    currentFrame = 0;
                }
                else
                {
                    // 仮
                    enemyCtrl.FinishAnim();
                    enemyCtrl.IsAttackFalse();

                    ChangeState(EnemyAnimState.Idle);
                }
            }
            else
            {
                currentFrame = nextFrame;
            }

            this.spr.sprite = currentActiveData.sprites[currentFrame];
        }
    }

    public void ChangeState(EnemyAnimState changedState)
    {
        if (currentState == changedState) return;
        currentState = changedState;

        // Dictionaryから指示されたステートのデータを特定して保持する
        if (animDictionary.TryGetValue(changedState, out var foundData))
        {
            currentActiveData = foundData;
            currentFrame = 0;
            timer = 0f;

            //Debug.Log("statusを変更しました：");

            // 新しいスプライトのフレームレートに変更
            timePerFrame = 1f / currentActiveData.frameRate;
        }
    }
}
