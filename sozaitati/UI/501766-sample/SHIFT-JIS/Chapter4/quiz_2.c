#define _CRT_SECURE_NO_WARNINGS
#define QUIZ_MAX 5
#include <stdio.h>
#include <string.h>
int main(void)
{
    char QUIZ[QUIZ_MAX][101] =
    {
    "2020年に発売され、ヒットしたNintendo Switchのゲーム「〇〇〇〇 どうぶつの森」。〇〇〇〇に入る言葉は？",
    "2010年代にスマートフォンでヒットしたソーシャルゲーム「パズドラ」の正式名称は？",
    "2000年代にガラケーでヒットした、自転車に乗った棒人間を操作して遊ぶゲームの名称は？",
    "1990年代にゲームセンターに設置され、ブームとなった写真シール機「プリクラ」の正式名称は？",
    "1980年代に大ヒットした家庭用ゲーム機「ファミコン」の正式名称は？"
    };
    char ANS[QUIZ_MAX][23] =
    {
    "あつまれ",
    "パズル＆ドラゴンズ",
    "チャリ走",
    "プリント倶楽部",
    "ファミリーコンピュータ"
    };
    int score = 0;
    char ans[31];
    printf("クイズを%d問、出題します。答えを15文字以内で入力してください。\n", QUIZ_MAX);
    for (int i = 0; i < QUIZ_MAX; i++)
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
            printf("間違いです。正しい答えは、%s\n", ANS[i]);
        }
    }
    printf("あなたは%d問、正解しました。", score);
}