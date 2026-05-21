#include <stdio.h>
int main(void)
{
    int life = 100;
    printf("体力(life)の初期値は%d\n", life);
    life = life + 50;
    printf("回復薬を飲み体力が50増え、%dになった。\n", life);
    life = life - 70;
    printf("敵の攻撃で体力が70減り、%dになった。\n", life);
    life = life * 3;
    printf("魔法を使って体力を3倍し、%dになった。\n", life);
    life = life / 2;
    printf("敵の攻撃で体力が半分の、%dになった。\n", life);
}
