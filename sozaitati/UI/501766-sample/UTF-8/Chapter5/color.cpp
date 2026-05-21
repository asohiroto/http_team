#include "DxLib.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    SetWindowText("RGB値による色指定"); // ウィンドウのタイトル
    SetGraphMode(1040, 300, 32); // ウィンドウの大きさとカラービット数の指定
    ChangeWindowMode(TRUE); // ウィンドウモードで起動
    if (DxLib_Init() == -1) return -1; // ライブラリ初期化 エラーが起きたら終了
    SetBackgroundColor(128, 128, 128); // 背景色の指定
    ClearDrawScreen(); // 画面をクリアする
    int y = 10;
    for (int i = 0; i <= 255; i++)
    {
        int x = 5 + i * 4;
        DrawBox(x, y, x + 4, y + 80, GetColor(i, 0, 0), TRUE);
        DrawBox(x, y + 100, x + 4, y + 180, GetColor(0, i, 0), TRUE);
        DrawBox(x, y + 200, x + 4, y + 280, GetColor(0, 0, i), TRUE);
    }
    WaitKey(); // キー入力があるまで待つ
    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}