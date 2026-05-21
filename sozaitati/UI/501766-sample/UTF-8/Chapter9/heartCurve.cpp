#include "DxLib.h"
#include <math.h>

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    const int WIDTH = 640, HEIGHT = 640; // ウィンドウの幅と高さのピクセル数
    SetWindowText("ハートを描く計算式"); // ウィンドウのタイトル
    SetGraphMode(WIDTH, HEIGHT, 32); // ウィンドウの大きさとカラービット数の指定
    ChangeWindowMode(TRUE); // ウィンドウモードで起動
    if (DxLib_Init() == -1) return -1; // ライブラリ初期化 エラーが起きたら終了
    SetBackgroundColor(0, 0, 0); // 背景色の指定

    // 計算式でハートを描く
    const int SCALING = 200; // スケール
    for (double a = 0; a < 3.141592 * 2; a += 0.01) // aを0～2πまで変化させる
    {
        double x = sin(a) * sin(a) * sin(a);
        double y = cos(a) - cos(a * 2) / 3 - cos(a * 3) / 5;
        int cx = x * SCALING + WIDTH / 2;   // ┬ コンピューターの座標に変換
        int cy = -y * SCALING + HEIGHT / 2; // ┘
        DrawBox(cx - 5, cy - 5, cx + 5, cy + 5, 0xff80ff, TRUE); // 正方形を並べる
    }

    WaitKey(); // キー入力があるまで待つ
    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}
