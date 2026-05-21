#include <stdio.h>
struct CHARACTER
{
    char name[11];
    int life;
    int strength;
    int defense;
};

struct CHARACTER enemy[3] =
{
    {"スライム",    200,  80,  30},
    {"スケルトン",  500, 240, 120},
    {"ドラゴン",   3000, 800, 300}
};

int main(void)
{
    for (int i = 0; i < 3; i++)
    {
        printf("%s\n", enemy[i].name);
        printf("体力 %d\n", enemy[i].life);
        printf("腕力 %d\n", enemy[i].strength);
        printf("防御力 %d\n", enemy[i].defense);
        printf("----------\n");
    }
}