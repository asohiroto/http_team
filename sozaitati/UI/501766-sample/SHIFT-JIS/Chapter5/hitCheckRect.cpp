#include "DxLib.h"
#include <stdlib.h>

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    // 定数の宣言
    const int WIDTH = 960, HEIGHT = 640; // ウィンドウの幅と高さのピクセル数
    const int WHITE = GetColor(255, 255, 255); // よく使う色を定義

    SetWindowText("ヒットチェック"); // ウィンドウのタイトル
    SetGraphMode(WIDTH, HEIGHT, 32); // ウィンドウの大きさとカラービット数の指定
    ChangeWindowMode(TRUE); // ウィンドウモードで起動
    if (DxLib_Init() == -1) return -1; // ライブラリ初期化 エラーが起きたら終了
    SetBackgroundColor(0, 0, 0); // 背景色の指定
    SetDrawScreen(DX_SCREEN_BACK); // 描画面を裏画面にする

    int x1 = 0, y1 = 0, w1 = 120, h1 = 80; // マウスで動かせる矩形の座標、幅、高さ
    int x2 = WIDTH / 2, y2 = HEIGHT / 2, w2 = 160, h2 = 240;

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする

        GetMousePoint(&x1, &y1); // 図形をマウスポインタの位置にする
        int col1 = GetColor(255, 0, 0); // 赤い色の値
        int col2 = GetColor(0, 0, 255); // 青い色の値
        int dx = abs((x1 - x2)); // x軸方向の距離
        int dy = abs((y1 - y2)); // y軸方向の距離
        if (dx <= (w1 + w2) / 2 && dy <= (h1 + h2) / 2) // ヒットチェック
        {
            col1 = GetColor(255, 255, 0);
            col2 = GetColor(0, 255, 255);
        }
        DrawBox(x1 - w1 / 2, y1 - h1 / 2, x1 + w1 / 2, y1 + h1 / 2, col1, TRUE);
        DrawBox(x2 - w2 / 2, y2 - h2 / 2, x2 + w2 / 2, y2 + h2 / 2, col2, TRUE);

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(33); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}