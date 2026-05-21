#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
int main(void)
{
    int count = 0;
    int answer;
    int rnd = 1 + rand() % 99;
    printf("私が思い浮かべる数を当てましょう。\n");
    printf("その数は1～99のいずれかの整数です。\n");
    while (1)
    {
        printf("その数は?");
        scanf("%d", &answer);
        count++;
        if (rnd > answer)
            printf("違います。それより大きな数です。\n");
        else if (rnd < answer)
            printf("違います。それより小さな数です。\n");
        else
        {
            printf("正解です。あなたは%d回で当てました。", count);
            break;
        }
    }
}