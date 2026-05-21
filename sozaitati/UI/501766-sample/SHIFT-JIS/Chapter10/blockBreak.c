#include<stdio.h>
#include<conio.h>
#include<windows.h> // Sleep()を使うため
#define ROW 24 // ゲーム画面の行数
#define COL 60 // ゲーム画面の列数
#define BLOCK_ROW 5 // ブロックが縦にいくつ並ぶか
#define BLOCK_COL 10 // ブロックが横にいくつ並ぶか

int block[BLOCK_ROW][BLOCK_COL]; // ブロックの有無を管理する配列
int px = COL / 2 - 3, py = ROW - 1; // バー（パドル）の座標
int bx = 6, by = 10; // ボールの座標
int vx = 1, vy = -1; // ボールの移動量
int score = 0; // スコア
int game = 0; // プレイ中は0、ゲームオーバーは1、ゲームクリアは2

int main(void) {
    int x, y;
    for (y = 0; y < BLOCK_ROW; y++) { // ブロックの初期化
        for (x = 0; x < BLOCK_COL; x++) block[y][x] = 1;
    }
    printf("\x1b[2J"); // 画面クリア
    while (game == 0) { // ゲーム用のループ
        // ブロックを描画
        printf("\x1b[1;1H"); // カーソル位置を左上角に
        for (y = 0; y < BLOCK_ROW; y++) {
            printf("\x1b[3%dm", y + 1); // ブロックの色
            for (x = 0; x < BLOCK_COL; x++) {
                if (block[y][x] == 1)
                    printf("######"); // ブロックは半角6文字分
                else
                    printf("      ");
            }
            printf("\n");
        }

        // バーの操作
        printf("\x1b[%d;%dH", py + 1, px + 1);
        printf("        "); // 1フレーム前のバーを消す
        int key = 0;
        if (_kbhit()) key = _getch(); // キー入力
        if (key == 75) { // 左キー
            px -= 4;
            if (px < 0) px = 0;
        }
        if (key == 77) { // 右キー
            px += 4;
            if (px >= COL - 8) px = COL - 8;
        }
        printf("\x1b[36m"); // 水色
        printf("\x1b[%d;%dH", py + 1, px + 1);
        printf("========"); // バーを描く

        // ボールの移動
        printf("\x1b[%d;%dH", by + 1, bx + 1);
        printf(" "); // 1フレーム前のボールを消す
        bx += vx;
        by += vy;
        if (bx < 1) vx = 1; // 画面左端にきた時
        if (bx > COL - 2) vx = -1; // 画面右端にきた時
        if (by < 1) vy = 1; // 画面上端にきた時
        if (by > ROW - 1) game = 1; // 画面下端に達したらゲームオーバー
        if (px - 1 <= bx && bx <= px + 8 && by == py - 1) vy = -1; // バーで打ち返した時
        printf("\x1b[31m"); // 赤
        printf("\x1b[%d;%dH", by + 1, bx + 1);
        printf("o"); // ボールを描く

        if (0 <= by && by < BLOCK_ROW) { // ボールがブロックに当たったか判定
            x = bx / 6; // 6はブロックの幅（半角文字数）
            y = by;
            if (block[y][x] == 1) { // ブロックがある
                printf("\x1b[%d;%dH", y + 1, x * 6 + 1);
                printf("++++++"); // 壊した演出
                block[y][x] = 0; // ブロックを存在しない状態にする
                vy = -vy; // y軸方向の移動量を反転
                score += 1; // スコアを増やす
                if (score == BLOCK_ROW * BLOCK_COL) game = 2; // 全て壊したらクリア
            }
        }

        printf("\x1b[%d;%dH", ROW + 2, 1);
        printf("\x1b[37m"); // 白
        printf("SCORE %d00\n", score); // スコアの表示
        Sleep(80); // 処理の一時停止
    }

    if (game == 1) { // ゲームオーバー
        printf("\x1b[%d;%dH", ROW / 2, COL / 2 - 4);
        printf("\x1b[31m"); // 赤
        printf("GAME OVER");
    }
    if (game == 2) { // ゲームクリア
        printf("\x1b[%d;%dH", ROW / 2, COL / 2 - 7);
        printf("\x1b[37m"); // 白
        printf("Congratulations!");
    }
    printf("\x1b[%d;%dH", ROW + 3, 1); // 画面が崩れないようにカーソル位置を画面下部にする
    Sleep(5000); // 5秒ほど停止した後、処理を終える
    return 0;
}