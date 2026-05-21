#include "DxLib.h"

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    // 定数の定義
    const int WIDTH = 960, HEIGHT = 640; // ウィンドウの幅と高さのピクセル数

    SetWindowText("迷路の表示と、壁と床の判定"); // ウィンドウのタイトル
    SetGraphMode(WIDTH, HEIGHT, 32); // ウィンドウの大きさとカラービット数の指定
    ChangeWindowMode(TRUE); // ウィンドウモードで起動
    if (DxLib_Init() == -1) return -1; // ライブラリ初期化 エラーが起きたら終了
    SetBackgroundColor(0, 0, 0); // 背景色の指定
    SetDrawScreen(DX_SCREEN_BACK); // 描画面を裏画面にする

    // 迷路の定義
    const int MAZE_PIXEL = 50; // 1マスを何ピクセルとするか
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

    while (1) // メインループ
    {
        ClearDrawScreen(); // 画面をクリアする

        // 迷路の描画
        int c, x, y, sx, sy;
        for (y = 0; y < MAZE_H; y++)
        {
            for (x = 0; x < MAZE_W; x++)
            {
                sx = x * MAZE_PIXEL; // マスのx座標
                sy = y * MAZE_PIXEL; // マスのy座標
                c = 0xffffe0; // 床の色（通れるところ）
                if (maze[y][x] == 1) c = 0x804000; // 壁の色（通れないところ）
                DrawBox(sx, sy, sx + MAZE_PIXEL - 1, sy + MAZE_PIXEL - 1, c, TRUE);
            }
        }

        // 壁と床の判定
        int mouseX, mouseY; // マウスポインタの座標を代入する変数
        GetMousePoint(&mouseX, &mouseY); // マウスポインタの座標を取得
        int mx = mouseX / MAZE_PIXEL; // ポインタのx座標をマスのピクセル数で割る
        int my = mouseY / MAZE_PIXEL; // ポインタのy座標をマスのピクセル数で割る
        if (0 <= mx && mx < MAZE_W && 0 <= my && my < MAZE_H) // ポインタが迷路上にある
        {
            if (maze[my][mx] == 0) DrawString(5, 5, "ここは床です", 0xffffff);
            if (maze[my][mx] == 1) DrawString(5, 5, "ここは壁です", 0xffff00);
        }
        else
        {
            DrawString(5, 5, "迷路の外側です", 0xff0000);
        }

        ScreenFlip(); // 裏画面の内容を表画面に反映させる
        WaitTimer(16); // 一定時間待つ
        if (ProcessMessage() == -1) break; // Windowsから情報を受け取りエラーが起きたら終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) == 1) break; // ESCキーが押されたら終了
    }

    DxLib_End(); // ＤＸライブラリ使用の終了処理
    return 0; // ソフトの終了
}