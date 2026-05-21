#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <string.h>
int main(void)
{
    char QUIZ[3][100] = {
        "モンスターボールを投げてモンスターを捕まえるゲームのタイトルは？",
        "スマートフォンでヒットした、モンスターを引っ張って飛ばすゲームは？",
        "「DQ」はある有名なゲームのタイトルの略語。そのゲームの正式名称は？"
    };
    char ANS[3][30] = {
        "ポケットモンスター",
        "モンスターストライク",
        "ドラゴンクエスト"
    };
    int score = 0;
    char ans[30];
    printf("クイズの答えを入力し、Enterを押してください。\n");
    for (int i = 0; i < 3; i++)
    {
        printf("%s\n", QUIZ[i]);
        scanf("%s", ans);
        if (strcmp(ans, ANS[i]) == 0)
        {
            printf("正解です。\n");
            score = score + 1;
        }
        else
        {
            printf("間違いです。正しい答えは%s\n", ANS[i]);
        }
    }
    printf("あなたは%d問、正解しました。", score);
}