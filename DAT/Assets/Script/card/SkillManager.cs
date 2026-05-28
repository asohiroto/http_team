using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEngine.InputManagerEntry;

public class SkillManager : MonoBehaviour
{
    PlayerController player;
    HandManager hand;
    CraftManager craft;

    [SerializeField] int healAmount;         // 回復量
    [SerializeField] int enhanceTime;        // エンハンス効果時間
    [SerializeField] int hyperTime;          // ハイパー効果時間
    [SerializeField] int fbCooldownTime;     // ファイアーボール使用可能間隔
    [SerializeField] int waitTime;           // 使用待機時間
    [SerializeField] int enhanceAmount;      // エンハンス強化量
    [SerializeField] int hyperDamageAmount;  // ハイパーモード攻撃力強化量
    [SerializeField] int curseAmount;       // カースHP減少量

    [SerializeField] float hyperSpeedAmount; // ハイパーモード素早さ強化量
    [SerializeField] float fbSpeed;          // ファイアーボールで生成したオブジェクトの速度

    int enhanceCount = 0;                    // エンハンスの効果時間カウンタ
    int hyperCount = 0;                      // ハイパーモードの効果時間カウンタ
    int fbCount = 0;                         // ファイアーボールの効果時間カウンタ
    int enhanceFlag = 0;                     // エンハンス強化状態の判定
    int hyperFlag = 0;                       // ハイパーモード強化状態の判定
    int fbFlag = 0;                          // ファイアーボール状態の判定
    int fbEffectFlag = 0;                    // ファイアーボールが画面内に存在するか

    public int discardInd1 = -1;             // 捨てるカードの住所その１
    public int discardInd2 = -1;             //                   その２

    Vector3 mousePosScreen = new Vector3();  // スクリーン座標系でのマウスの位置
    Vector3 mousePosWorld = new Vector3();   // ワールド座標系でのマウスの位置

    Vector2 fbMousePos = new Vector2();      // ファイアーボール実行時点でのマウスの位置
    Vector2 destPosFb = new Vector2();       // ファイアーボールの目的地ベクトル

    [SerializeField] GameObject fireBall;    // ファイアーボールで生成するオブジェクト
    [SerializeField] GameObject fireBallHit; // ファイアーボールの当たり判定兼エフェクト
    GameObject fireBallPreFab;               // 生成したファイアーボール
    GameObject fireBallHitCheckPreFab;       // 生成したファイアーボールの当たり判定

    // 福田げんきが追加
    [SerializeField] GameObject[] slashTypePrefab; // 0に強斬り、1に火の強切りを入れる予定
    PlayerAttack playerAttack;
    int strongSlashDamage = 10; // 仮のアタックダメージ
    int fireSlashDamage = 10; // 仮のファイヤースラッシュダメージ

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hand = GameObject.Find("HandManager").GetComponent<HandManager>();
        craft = GameObject.Find("CraftManager").GetComponent<CraftManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enhanceCount++;
        hyperCount++;
        fbCount++;

        // 各座標を入力
        mousePosScreen.x = Mouse.current.position.x.ReadValue();
        mousePosScreen.y = Mouse.current.position.y.ReadValue();
        mousePosScreen.z = -Camera.main.transform.position.z;

        mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);　// 座標系の変換

        if (enhanceFlag == 1 && enhanceCount > enhanceTime * 60) // エンハンスの強化解除
        {
            player.attackDamage = player.defaultAttackDamage;
            Debug.Log("Power : " + player.attackDamage);

            enhanceFlag = 0;
        }

        if (hyperFlag == 1 && hyperCount > hyperTime * 60) // ハイパーモードの効果解除
        {
            player.attackDamage = player.defaultAttackDamage;
            player.speed = player.defaultSpeed;

            Debug.Log("Power : " + player.attackDamage);
            Debug.Log("Speed : " + player.speed);

            hyperFlag = 0;
        }

        if (fbFlag == 1)
        {
            fireBallPreFab.transform.Translate(destPosFb * fbSpeed); // ファイアーボール自身を目的地に向かってすすませる

            float dist = Vector2.Distance(fireBallPreFab.transform.position, fbMousePos);

            Debug.Log(dist);

            if (dist < 0.3f)
            {
                fireBallHitCheckPreFab = Instantiate(fireBallHit);
                fireBallHitCheckPreFab.transform.position = fireBallPreFab.transform.position;
                fireBallHitCheckPreFab.name = ("FireBallHitCheck");


                Destroy(fireBallPreFab);

                fbCount = 0;

                fbEffectFlag = 1;
                fbFlag = 0;
            }
            else if (dist > 20.0f)
            {
                Destroy(fireBallPreFab);

                fbCount = 0;

                fbEffectFlag = 1;
                fbFlag = 0;
            }
        }

        if (fbEffectFlag == 1 && fbCount > fbCooldownTime * 60)
        {
            Destroy(fireBallHitCheckPreFab);

            fbEffectFlag = 0;
        }

        if (Keyboard.current.pKey.wasPressedThisFrame) // 【テスト用】　pを押すと体力を減らす
        {
            player.playerHP -= 15;
            Debug.Log("playerHP : " + player.playerHP);
        }

    }

    public async Task Enhance(int ind) // 攻撃力強化 ID->0
    {
        int cardID = 0;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime) // waitTime秒分だけ左クリックの入力を待つ
            {
                waitTimer++;

                await Task.Yield();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (enhanceFlag == 0) // 非強化状態なら使用可能
                {
                    player.attackDamage += enhanceAmount;

                    enhanceFlag = 1;
                    enhanceCount = 0;


                    Debug.Log("power = " + player.attackDamage);

                    hand.DisCard(ind);
                }
                else
                {
                    Debug.Log("同名の強化は重ね掛けできないよ？");
                }
            }
            else
            {
                Debug.Log("スキップしたよ");
            }
        }

        CraftMethod(cardID, ind);
    }

    public async Task Heal(int ind) // 回復 ID->1
    {
        int cardID = 1;

        if (craft.craftFrag == 0)
        {
            int waitFrames = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitFrames < 60 * waitTime)
            {
                waitFrames++;

                await Task.Yield();
            }


            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (player.playerHP > player.maxPlayerHP - healAmount) // 回復して最大HPを超える場合は、最大HPまで回復
                {
                    player.playerHP = player.maxPlayerHP;

                    hand.DisCard(ind);
                }
                else if (player.playerHP == player.maxPlayerHP)
                {
                    Debug.Log("元気すぎやしないかい？");
                }
                else
                {
                    player.playerHP += healAmount;

                    hand.DisCard(ind);
                }

                Debug.Log(player.playerHP);
            }
            else
            {
                Debug.Log("スキップしたよ");
            }
        }

        CraftMethod(cardID, ind);
    }

    public async Task Slash(int ind) // 強斬り ID->2 （福田）追加した引数は今日切りのエフェクトとダメージ
    {
        int cardID = 2;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
            {
                waitTimer++;

                await Task.Yield();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                /*// publicにしたもの
                // attackDir lastDir strongAttackObj defaultFXRot onAttack attackTime

                Debug.Log("slash!");

                float flipX = 0;
                float rotZ = 0;
                //onAttack = true;
                player.attackDir = player.lastDir;

                GameObject obj = Instantiate(player.strongAttackObj, transform);
                obj.transform.position = transform.position + player.attackDir * 0.50f;

                // 斬撃の方向を決定
                if (player.attackDir.x > 0)
                {
                    flipX = 0;
                }
                else if (player.attackDir.x < 0)
                {
                    flipX = 180;
                }

                if (player.attackDir.x == 0)
                {
                    // 真上・真下（左右の入力がないとき）
                    if (player.attackDir.y > 0) rotZ = 90 + player.defaultFXRot;
                    else if (player.attackDir.y < 0) rotZ = -90 + player.defaultFXRot;
                    else rotZ = 0 + player.defaultFXRot; // 入力なし（真横など）
                }
                else
                {
                    // 斜め入力（左右の入力があるとき）
                    if (player.attackDir.y > 0) rotZ = 45 + player.defaultFXRot;
                    else if (player.attackDir.y < 0) rotZ = -45 + player.defaultFXRot;
                    else rotZ = 0 + player.defaultFXRot; // 真横
                }

                obj.transform.rotation = Quaternion.Euler(0, flipX, rotZ);

                hand.DisCard(ind);

                await Task.Delay((int)(0.1f * 1000));

                player.onAttack = true;

                await Task.Delay((int)(player.attackTime * 1000));

                player.onAttack = false;*/
                player.SlashTypeAndDir(slashTypePrefab[0]);　// 強切りを生成して使う方向を決定する
                playerAttack = slashTypePrefab[0].GetComponent<PlayerAttack>();
                playerAttack.attackDamage = player.attackDamage + strongSlashDamage; // 強切りのアタックダメージを代入

                hand.DisCard(ind);

                await Task.Delay((int)(0.1f * 1000)); // アタック方向の固定を少し遅らせる

                player.onAttack = true;

                await Task.Delay((int)(player.attackTime * 1000));

                player.onAttack = false;
            }
        }

        CraftMethod(cardID, ind);
    }

    public async Task FireBall(int ind) // 火の玉を飛ばす ID->3
    {
        int cardID = 3;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;
            float distX, distY;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
            {
                waitTimer++;

                await Task.Yield();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (fbEffectFlag == 0)
                {
                    fbMousePos = new Vector2(mousePosWorld.x, mousePosWorld.y);

                    distX = fbMousePos.x - player.currentPos.x;
                    distY = fbMousePos.y - player.currentPos.y;

                    // マウスがさしたポイントへの単位ベクトルを作成
                    destPosFb = new Vector2(distX, distY);
                    destPosFb.Normalize();

                    // オブジェクトを作成する
                    GameObject obj = Instantiate(fireBall);
                    obj.transform.position = player.currentPos;
                    obj.transform.name = ("FireBall");

                    fireBallPreFab = obj;

                    fbFlag = 1; // フラグを立てる

                    hand.DisCard(ind);
                }
                else
                {
                    Debug.Log("魔術回路冷却中");
                }
            }
        }

        CraftMethod(cardID, ind);
    }

    public async Task FireSlash(int ind) // 炎斬り　ID->4
    {
        int cardID = 4;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime)
            {
                waitTimer++;

                await Task.Yield();
            }
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    player.SlashTypeAndDir(slashTypePrefab[1]); // 強切りを生成して使う方向を決定する
                    playerAttack = slashTypePrefab[1].GetComponent<PlayerAttack>();
                    playerAttack.attackDamage = player.attackDamage + fireSlashDamage; // 火の強切りのアタックダメージを代入

                    hand.DisCard(ind);

                    await Task.Delay((int)(0.1f * 1000)); // アタック方向の固定を少し遅らせる

                    player.onAttack = true;

                    await Task.Delay((int)(player.attackTime * 1000));

                    player.onAttack = false;
                }
            }
        }

        CraftMethod(cardID, ind);
    }

    public async Task HyperMode(int ind) // 超強化　ID->5
    {
        int cardID = 5;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime) // waitTime秒分だけ左クリックの入力を待つ
            {
                waitTimer++;

                await Task.Yield();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (hyperFlag == 0) // 非強化状態なら使用可能
                {
                    player.attackDamage += hyperDamageAmount;
                    player.speed += hyperSpeedAmount;

                    hyperFlag = 1;
                    hyperCount = 0;


                    Debug.Log("power = " + player.attackDamage);

                    hand.DisCard(ind);
                }
                else
                {
                    Debug.Log("同名の強化は重ね掛けできないよ？");
                }
            }
        }

        CraftMethod(cardID, ind);
    }

    public async Task Curse(int ind) // カース　ID->6
    {
        int cardID = 6;

        if (craft.craftFrag == 0)
        {
            int waitTimer = 0;

            while (!Mouse.current.rightButton.wasPressedThisFrame && waitTimer < 60 * waitTime) // waitTime秒分だけ左クリックの入力を待つ
            {
                waitTimer++;

                await Task.Yield();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                player.playerHP -= curseAmount;

                hand.DisCard(ind);
            }
        }
        CraftMethod(cardID, ind);

    }

    public void CraftMethod(int id, int ind) // カード合成の関数
    {
        if (craft.craftFrag == 2) // 保存されたIDを呼び出し、素材となったカードを破壊し、空いたスペースにカードを合成
        {
            int craftResult = craft.CraftItems(craft.material1, id);

            discardInd2 = ind;
            craft.craftFrag = 0;

            if (craftResult < 0)
            {
                return;
            }

            hand.DisCard(discardInd1);
            hand.DisCard(discardInd2);

            int spawnIndex = Mathf.Min(discardInd1, discardInd2);

            GameObject obj = hand.CardGenerate(craftResult, spawnIndex);
            hand.ButtonListener(craftResult, obj, spawnIndex);
        }
        else if (craft.craftFrag == 1) // 場所とIDを保存する
        {
            discardInd1 = ind;
            craft.SettingMaterial1(id);
        }
    }
}


