#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
int main(void)
{
    char txt[11];
    printf("※要素数11の配列は半角10文字まで、全角は5文字まで代入できます\n");
    printf("あなたの名前は？\n");
    scanf("%s", txt);
    printf("%sよ、いよいよ、冒険の旅に出発じゃ。", txt);
}