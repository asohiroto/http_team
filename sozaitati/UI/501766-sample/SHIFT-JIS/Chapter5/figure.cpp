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

        DrawLine(0, 0, WIDTH, HEIGHT, GetColor(255, 0, 0)); // 線
        DrawBox(0, HEIGHT - 400, 200, HEIGHT - 100, GetColor(0, 255, 0), TRUE); // 矩形(長方形)
        DrawBox(WIDTH - 200, 100, WIDTH - 100, 200, GetColor(0, 0, 255), TRUE); // 矩形(正方形)
        DrawCircle(400, 200, 100, GetColor(0, 255, 255), TRUE); // 円
        DrawOval(400, 400, 200, 100, GetColor(255, 0, 255), FALSE); // 楕円
        DrawTriangle(600, 0, 500, 300, 700, 300, GetColor(255, 192, 0), TRUE); // 三角形
        DrawPixel(400, 200, GetColor(0, 0, 0)); // 点

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(33); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}