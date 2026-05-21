#include <stdio.h>
int main(void)
{
    int strength = 1;
    printf("腕力(strength)の初期値は%d。\n", strength);
    printf("腕力倍増の魔法をかけ続けるぞ！\n");
    while (strength < 128)
    {
        strength = strength * 2;
        printf("腕力が%dになった！\n", strength);
    }
}