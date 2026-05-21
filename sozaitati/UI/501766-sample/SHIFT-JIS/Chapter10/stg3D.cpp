#include "DxLib.h"
#include <stdlib.h> // abs()を用いる

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    // 定数の定義
    const int WIDTH = 1200, HEIGHT = 720; // ウィンドウの幅と高さのピクセル数

    SetWindowText("３Ｄシューティングゲーム"); // ウィンドウのタイトル
    SetGraphMode(WIDTH, HEIGHT, 32); // ウィンドウの大きさとカラービット数の指定
    ChangeWindowMode(TRUE); // ウィンドウモードで起動
    if (DxLib_Init() == -1) return -1; // ライブラリ初期化 エラーが起きたら終了
    SetBackgroundColor(0, 0, 0); // 背景色の指定
    SetDrawScreen(DX_SCREEN_BACK); // 描画面を裏画面にする

    int timer = 0; // 経過時間を数える変数

    int mdl[4] = // モデルの読み込み
    {
        MV1LoadModel("model/fighter.mqoz"), // 自機
        MV1LoadModel("model/missile.mqoz"), // 弾
        MV1LoadModel("model/object0.mqoz"), // 敵1
        MV1LoadModel("model/object1.mqoz")  // 敵2
    };

    // 自機用の変数
    float playerX = 0.0f, playerY = 0.0f, playerZ = 0.0f; // 座標
    int playerZa = 0; // 機体の傾き
    int spcBk = 0; // 1フレーム前のスペースキーの状態

    // 弾用の変数 ※フラグが0なら存在しない
    const int MISSILE_MAX = 20; // 弾の数
    float missileX[MISSILE_MAX], missileY[MISSILE_MAX], missileZ[MISSILE_MAX];
    int missileFlg[MISSILE_MAX]; // 存在するかのフラグ
    for (int i = 0; i < MISSILE_MAX; i++) missileFlg[i] = 0;

    // 敵機用の変数 ※ 学習用プログラムのため、敵は常に存在する状態
    const int ENEMY_MAX = 10; // 敵の数
    float enemyX[ENEMY_MAX], enemyY[ENEMY_MAX], enemyZ[ENEMY_MAX];
    for (int i = 0; i < ENEMY_MAX; i++) { // 初期座標を代入
        enemyX[i] = -300.0f + rand() % 600;
        enemyY[i] = -200.0f + rand() % 400;
        enemyZ[i] = 10000.0f;
    }

    ChangeLightTypeDir(VGet(1.0f, -1.0f, 0.5f)); // ディレクショナルライトの向きをセット

    VECTOR camPos = VGet(0, 100, -600); // カメラ位置（座標）※整数値を与えても問題なし
    VECTOR camTar = VGet(0, 0, 1000); // カメラの注視点（座標）
    SetCameraPositionAndTarget_UpVecY(camPos, camTar); // カメラ位置と注視点をセット

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする

        timer++; // 時間のカウント
        DrawFormatString(0, 0, GetColor(255, 255, 0), "%d", timer);

        // 線分を描く命令で宇宙の道を表現
        for (int n = 0; n < 20; n++) {
            float bz = 800.0f * n - (timer % 20) * 40; // z座標
            int c = (30 - n) * 6; // 色
            DrawLine3D(VGet(-400.0f, -240.0f, bz), VGet(400.0f, -240.0f, bz), GetColor(c, c, c));
        }

        // 自機の移動
        if (CheckHitKey(KEY_INPUT_UP) && playerY < 200) playerY += 8; // 上キー
        if (CheckHitKey(KEY_INPUT_DOWN) && playerY > -200) playerY -= 8; // 下キー
        if (CheckHitKey(KEY_INPUT_LEFT)) { // 左キー
            if (playerX > -300) playerX -= 12;
            if (playerZa < 30) playerZa += 10; // 機体の傾き
        }
        else if (CheckHitKey(KEY_INPUT_RIGHT)) { // 右キー
            if (playerX < 300) playerX += 12;
            if (playerZa > -30) playerZa -= 10; // 機体の傾き
        }
        else {
            playerZa /= 2; // キー入力が無ければ傾きを戻す
        }
        MV1SetRotationXYZ(mdl[0], VGet(0.0f, 0.0f, 3.1416 * playerZa / 180)); // 回転角を指定
        MV1SetPosition(mdl[0], VGet(playerX, playerY, playerZ)); // 自機を三次元空間に配置
        MV1DrawModel(mdl[0]); // 自機の描画

        // 弾の処理
        if (CheckHitKey(KEY_INPUT_SPACE) && spcBk == 0) { // スペースキーで発射
            for (int i = 0; i < MISSILE_MAX; i++) {
                if (missileFlg[i] == 1) continue; // 空いている配列を探す
                missileX[i] = playerX;
                missileY[i] = playerY;
                missileZ[i] = playerZ + 100;
                missileFlg[i] = 1; // 存在する状態に
                break;
            }
        }
        spcBk = CheckHitKey(KEY_INPUT_SPACE); // スペースキーの状態を保持
        for (int i = 0; i < MISSILE_MAX; i++) { // 弾の移動
            if (missileFlg[i] == 0) continue;
            missileZ[i] += 200; // 画面奥に向かって飛ばす
            MV1SetPosition(mdl[1], VGet(missileX[i], missileY[i], missileZ[i])); // 弾を配置
            MV1DrawModel(mdl[1]); // 弾の描画
            if (missileZ[i] > 10000) missileFlg[i] = 0; // 奥まで飛んだら存在しない状態にする
        }

        // 敵機の処理
        for (int i = 0; i < ENEMY_MAX; i++) {
            enemyZ[i] = enemyZ[i] - 10 - i * 2; // 奥から手前に移動する
            if (enemyZ[i] < -200) enemyZ[i] = 10000; // 手前まで来たら再び奥に出現
            int mn = 2 + i % 2; // 2つのモデルを交互に使う
            MV1SetPosition(mdl[mn], VGet(enemyX[i], enemyY[i], enemyZ[i])); // 敵機を配置
            MV1DrawModel(mdl[mn]); // 敵機の描画
            for (int j = 0; j < MISSILE_MAX; j++) { // 弾とのヒットチェック
                if (missileFlg[j] == 0) continue;
                int dx = abs((int)(enemyX[i] - missileX[j])); // x軸方向の距離
                int dy = abs((int)(enemyY[i] - missileY[j])); // y軸方向の距離
                int dz = abs((int)(enemyZ[i] - missileZ[j])); // z軸方向の距離
                if (dx < 100 && dy < 100 && dz < 120) { // 直方体による当たり判定
                    enemyZ[i] = -200; // 衝突したら簡易的に敵を消す
                    missileFlg[j] = 0; // 弾を消す
                }
            }
        }

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(16); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}