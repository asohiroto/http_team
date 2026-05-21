#include <stdio.h>
int main(void)
{
    int strength = 1000;
    int defense = 1000;
    printf("あなたの腕力は%dです。\n", strength);
    printf("あなたの防御力は%dです。\n", defense);
    if (strength >= 1000 && defense >= 1000)
        printf("あなたは屈強な戦士になりました！");
    else
        printf("更なる修業が必要です。");
}
