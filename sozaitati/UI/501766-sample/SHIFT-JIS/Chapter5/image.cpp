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
    int imgBG = LoadGraph("image/bg.png"); // 変数に背景画像を読み込む
    int imgDog[4] = { // 配列に犬の画像を読み込む
        LoadGraph("image/dog0.png"),
        LoadGraph("image/dog1.png"),
        LoadGraph("image/dog2.png"),
        LoadGraph("image/dog3.png")
    };
    int dogX = 0, dogY = 400; // 犬の座標用の変数

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする

        DrawGraph(0, 0, imgBG, FALSE); // 背景の表示
        dogX = dogX + 10;
        if (dogX > WIDTH) dogX = -200;
        DrawGraph(dogX, dogY, imgDog[(timer / 5) % 4], TRUE);

        timer++; // 時間のカウント
        DrawFormatString(0, 0, WHITE, "%d", timer);

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(33); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}