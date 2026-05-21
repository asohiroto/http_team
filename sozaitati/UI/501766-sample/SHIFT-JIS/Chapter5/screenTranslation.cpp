#include "DxLib.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    // 定数の宣言
    const int WIDTH = 960, HEIGHT = 640; // ウィンドウの幅と高さのピクセル数
    const int WHITE = GetColor(255, 255, 255); // よく使う色を定義
    const int RED = GetColor(255, 0, 0); // 赤い色を定義

    SetWindowText("画面遷移"); // ウィンドウのタイトル
    SetGraphMode(WIDTH, HEIGHT, 32); // ウィンドウの大きさとカラービット数の指定
    ChangeWindowMode(TRUE); // ウィンドウモードで起動
    if (DxLib_Init() == -1) return -1; // ライブラリ初期化 エラーが起きたら終了
    SetBackgroundColor(0, 0, 0); // 背景色の指定
    SetDrawScreen(DX_SCREEN_BACK); // 描画面を裏画面にする

    int timer = 0; // 経過時間を数える変数
    enum { TITLE, PLAY, MENU, CLEAR, OVER }; // 各シーンを定める定数
    int scene = TITLE; // どのシーンの処理を行うか

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする
        timer++; // 時間のカウント
        SetFontSize(16);
        DrawFormatString(0, 0, WHITE, "%d", timer);

        switch (scene) // 画面遷移を行うswitch文
        {
        case TITLE: // タイトル画面の処理
            SetFontSize(50);
            DrawString(100, 50, "タイトル画面", WHITE);
            SetFontSize(20);
            DrawString(100, 200, "Sキーを押すとゲーム開始", WHITE);
            if (CheckHitKey(KEY_INPUT_S) == 1) scene = PLAY;
            break;

        case PLAY: // ゲームをプレイする処理
            SetFontSize(50);
            DrawString(100, 50, "ゲームプレイ画面", WHITE);
            SetFontSize(20);
            DrawString(100, 200, "Mキーでメニュー画面へ", WHITE);
            SetFontSize(20);
            DrawString(100, 300, "Oキーでゲームオーバー", RED);
            if (CheckHitKey(KEY_INPUT_M) == 1) scene = MENU;
            if (CheckHitKey(KEY_INPUT_O) == 1)
            {
                scene = OVER;
                timer = 0;
            }
            break;

        case MENU: // メニュー画面の処理
            SetFontSize(50);
            DrawString(100, 50, "メニュー画面", WHITE);
            SetFontSize(20);
            DrawString(100, 200, "Rキーでゲームに戻る", WHITE);
            if (CheckHitKey(KEY_INPUT_R) == 1) scene = PLAY;
            break;

        case CLEAR: // ゲームクリアの処理
            // このプログラムでは未記入
            break;

        case OVER: // ゲームオーバーの処理
            SetFontSize(50);
            DrawString(100, 50, "GAME OVER", RED);
            if (timer > 30 * 5) scene = TITLE;
            break;
        }

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(33); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}