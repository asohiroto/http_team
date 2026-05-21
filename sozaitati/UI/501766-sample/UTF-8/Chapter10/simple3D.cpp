#include "DxLib.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    // 定数の定義
    const int WIDTH = 960, HEIGHT = 640; // ウィンドウの幅と高さのピクセル数

    SetWindowText("３Ｄの基礎"); // ウィンドウのタイトル
    SetGraphMode(WIDTH, HEIGHT, 32); // ウィンドウの大きさとカラービット数の指定
    ChangeWindowMode(TRUE); // ウィンドウモードで起動
    if (DxLib_Init() == -1) return -1; // ライブラリ初期化 エラーが起きたら終了
    SetBackgroundColor(0, 0, 0); // 背景色の指定
    SetDrawScreen(DX_SCREEN_BACK); // 描画面を裏画面にする

    int timer = 0; // 経過時間を数える変数
    float camX = 0.0f, camY = 200.0f, camZ = 0.0f; // カメラ座標
    float objX = 0.0f, objY = 0.0f, objZ = 500.0f; // 物体の座標

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする

        timer++; // 時間のカウント
        DrawFormatString(0, 0, GetColor(255, 255, 0), "%d", timer);

        VECTOR camPos = VGet(camX, camY, camZ); // カメラ位置（座標）
        VECTOR camTar = VGet(objX, objY, objZ); // カメラの注視点（座標）
        SetCameraPositionAndTarget_UpVecY(camPos, camTar); // カメラ位置と注視点をセット

        // 三次元空間に三角形を配置
        VECTOR objPos = VGet(objX, objY, objZ); // 三角形の基本座標
        VECTOR v1 = VAdd(VGet(-100.0f, 0.0f, 0.0f), objPos); // 頂点1
        VECTOR v2 = VAdd(VGet(0.0f, 200.0f, 0.0f), objPos);  // 頂点2
        VECTOR v3 = VAdd(VGet(100.0f, 0.0f, 0.0f), objPos);  // 頂点3
        DrawTriangle3D(v1, v2, v3, GetColor(0, 255, 255), TRUE); // 三角形を配置（描画）

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(16); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}