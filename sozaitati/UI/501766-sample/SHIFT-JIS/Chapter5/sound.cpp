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
    int bgm = LoadSoundMem("sound/battle.mp3"); // BGMの読み込み
    int se = LoadSoundMem("sound/recover.mp3"); // SEの読み込み
    ChangeVolumeSoundMem(128, bgm); // BGMの音量を指定
    PlaySoundMem(bgm, DX_PLAYTYPE_LOOP); // BGMをループ再生

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする
        timer++; // 時間のカウント
        DrawFormatString(0, 0, WHITE, "%d", timer);

        // 音に関する処理
        DrawString(0, 20, "Sキーを押すとＢＧＭを停止します", WHITE);
        DrawString(0, 40, "スペースキーを押すと効果音を出力します", WHITE);
        if (CheckHitKey(KEY_INPUT_S)) StopSoundMem(bgm); // BGM停止
        if (CheckHitKey(KEY_INPUT_SPACE)) PlaySoundMem(se, DX_PLAYTYPE_BACK); // SE出力

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(33); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}