#include <stdio.h>
#include <stdlib.h>
int main(void)
{
    for (int i = 0; i < 5; i++)
    {
        int r = rand();
        printf("敵に %d のダメージを与えた！\n", r);
    }
}