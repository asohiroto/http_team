#include <stdio.h>
int main(void)
{
    int life = 1;
    printf("あなたの体力は%dです。\n", life);
    if (life < 0) printf("アンデッドキャラになりました。");
    if (life == 0) printf("あなたはもう戦えません。");
    if (life > 0) printf("あなたはまだ戦えます。");
}