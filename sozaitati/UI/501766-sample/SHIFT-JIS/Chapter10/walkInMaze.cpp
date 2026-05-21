#include "DxLib.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    // 定数の定義
    const int WIDTH = 960, HEIGHT = 704; // ウィンドウの幅と高さのピクセル数

    SetWindowText("キャラクターの移動、壁と床の判定"); // ウィンドウのタイトル
    SetGraphMode(WIDTH, HEIGHT, 32); // ウィンドウの大きさとカラービット数の指定
    ChangeWindowMode(TRUE); // ウィンドウモードで起動
    if (DxLib_Init() == -1) return -1; // ライブラリ初期化 エラーが起きたら終了
    SetBackgroundColor(0, 0, 0); // 背景色の指定
    SetDrawScreen(DX_SCREEN_BACK); // 描画面を裏画面にする

    // 迷路の定義
    const int MAZE_PIXEL = 64; // 1マスを何ピクセルとするか
    const int MAZE_W = 15; // 横に何マスあるか
    const int MAZE_H = 11; // 縦に何マスあるか
    char maze[MAZE_H][MAZE_W] = { // 迷路のデータ 0が床、1が壁
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,1,0,0,0,0,0,0,0,1},
        {1,0,1,1,1,0,0,0,1,1,0,1,1,0,1},
        {1,0,1,0,0,0,1,0,0,0,0,0,1,0,1},
        {1,0,1,0,1,0,1,0,1,1,1,0,1,0,1},
        {1,0,0,0,1,0,1,1,1,0,1,0,1,0,1},
        {1,0,1,1,1,0,1,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,1,1,1,0,1,0,1},
        {1,0,1,1,1,0,1,0,1,0,1,1,1,0,1},
        {1,0,0,0,0,0,1,0,0,0,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

    int chip[2] = { // 配列に背景の画像を読み込む
        LoadGraph("image/floor.png"),
        LoadGraph("image/wall.png")
    };
    int player[4] = { // 配列にロボットの画像を読み込む
        LoadGraph("image/robot_u.png"),
        LoadGraph("image/robot_d.png"),
        LoadGraph("image/robot_l.png"),
        LoadGraph("image/robot_r.png")
    };

    // プレイヤーキャラであるロボット用の変数
    int playerX = 96; // x座標
    int playerY = 96; // y座標
    int playerD = 1; // 向き 0=上、1=下、2=左、3=右

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする

        // 迷路の描画
        int x, y, sx, sy;
        for (y = 0; y < MAZE_H; y++)
        {
            for (x = 0; x < MAZE_W; x++)
            {
                sx = x * MAZE_PIXEL; // マスのx座標
                sy = y * MAZE_PIXEL; // マスのy座標
                DrawGraph(sx, sy, chip[maze[y][x]], FALSE); // 背景の画像を表示
            }
        }

        // プレイヤーキャラを動かす処理
        int newX = playerX, newY = playerY; // 移動先の座標（いったん現在の座標を代入）
        if (CheckHitKey(KEY_INPUT_UP)) { newY = playerY - 2;  playerD = 0; } // 上キー
        else if (CheckHitKey(KEY_INPUT_DOWN)) { newY = playerY + 2;  playerD = 1; } // 下キー
        else if (CheckHitKey(KEY_INPUT_LEFT)) { newX = playerX - 2;  playerD = 2; } // 左キー
        else if (CheckHitKey(KEY_INPUT_RIGHT)) { newX = playerX + 2;  playerD = 3; } // 右キー
        int mx1 = (newX - 20) / MAZE_PIXEL, my1 = (newY - 20) / MAZE_PIXEL; // 左上角
        int mx2 = (newX + 19) / MAZE_PIXEL, my2 = (newY - 20) / MAZE_PIXEL; // 右上角
        int mx3 = (newX - 20) / MAZE_PIXEL, my3 = (newY + 19) / MAZE_PIXEL; // 左下角
        int mx4 = (newX + 19) / MAZE_PIXEL, my4 = (newY + 19) / MAZE_PIXEL; // 右下角
        if (maze[my1][mx1] == 0 && maze[my2][mx2] == 0 && maze[my3][mx3] == 0 && maze[my4][mx4] == 0)
        {
            playerX = newX; // ┬ 4隅とも床なら座標を変化させる
            playerY = newY; // ┘
        }
        DrawGraph(playerX - 20, playerY - 20, player[playerD], TRUE); // ロボットを表示

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(16); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}