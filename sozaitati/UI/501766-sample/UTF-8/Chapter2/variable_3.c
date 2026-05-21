#include <stdio.h>
int main(void)
{
    int life = 100;
    double gold = 100;
    printf("体力(life)の初期値は%d\n", life);
    printf("所持金(gold)の初期値は%f\n", gold);
    life /= 3;
    gold /= 3;
    printf("3で割ったlifeの値は%d\n", life);
    printf("3で割ったgoldの値は%f\n", gold);
}