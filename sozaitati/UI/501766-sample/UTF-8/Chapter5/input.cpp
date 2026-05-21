#include "DxLib.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    // 定数の宣言
    const int WIDTH = 960, HEIGHT = 640; // ウィンドウの幅と高さのピクセル数
    const int WHITE = GetColor(255, 255, 255); // よく使う色を定義

    SetWindowText("ＤＸライブラリの使い方"); // ウィンドウのタイトル
    SetGraphMode(WIDTH, HEIGHT, 32); // ウィンドウの大きさとカラービット数の指定
    ChangeWindowMode(TRUE); // ウィンドウモードで起動
    if (DxLib_Init() == -1) return -1; // ライブラリ初期化 エラーが起きたら終了
    SetBackgroundColor(0, 0, 0); // 背景色の指定
    SetDrawScreen(DX_SCREEN_BACK); // 描画面を裏画面にする

    int timer = 0; // 経過時間を数える変数

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする
        timer++; // 時間のカウント
        DrawFormatString(0, 0, WHITE, "%d", timer);

        // カーソルキーの入力
        if (CheckHitKey(KEY_INPUT_UP))    DrawString(0, 20, "上キー", WHITE);
        if (CheckHitKey(KEY_INPUT_DOWN))  DrawString(0, 40, "下キー", WHITE);
        if (CheckHitKey(KEY_INPUT_LEFT))  DrawString(0, 60, "左キー", WHITE);
        if (CheckHitKey(KEY_INPUT_RIGHT)) DrawString(0, 80, "右キー", WHITE);

        // マウスの座標を出力、マウスボタンの入力
        int mouseX, mouseY; // ポインタの座標を代入する変数
        GetMousePoint(&mouseX, &mouseY);
        DrawFormatString(400, 0, WHITE, "(%d, %d)", mouseX, mouseY);
        if (GetMouseInput() & MOUSE_INPUT_LEFT)  DrawString(400, 20, "左ボタン", WHITE);
        if (GetMouseInput() & MOUSE_INPUT_RIGHT) DrawString(400, 40, "右ボタン", WHITE);

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(33); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}