#include "DxLib.h"
#include <math.h>

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

    int x1 = 0, y1 = 0, r1 = 80; // マウスで動かせる円の座標と半径
    int x2 = WIDTH / 2, y2 = HEIGHT / 2, r2 = 120;

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする

        GetMousePoint(&x1, &y1); // 図形をマウスポインタの位置にする
        int col1 = GetColor(255, 0, 0); // 赤い色の値
        int col2 = GetColor(0, 0, 255); // 青い色の値
        int d = sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        DrawFormatString(0, 0, WHITE, "中心間距離 %d", d);
        if (d <= r1 + r2) // ヒットチェック
        {
            col1 = GetColor(255, 255, 0);
            col2 = GetColor(0, 255, 255);
        }
        DrawCircle(x1, y1, r1, col1, TRUE);
        DrawCircle(x2, y2, r2, col2, TRUE);

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(33); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}