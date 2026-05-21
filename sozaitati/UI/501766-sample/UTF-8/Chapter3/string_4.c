#include <stdio.h>
int main(void)
{
    char job[][7] = { "勇者", "戦士", "僧侶", "魔術師", "踊り子" };
    for (int i = 0; i < 5; i++) printf("職業%d %s\n", i + 1, job[i]);
}