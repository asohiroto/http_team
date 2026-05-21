#include <stdio.h>
int main(void)
{
    char monster[3][10] = { "Slime", "Ghost", "Vampire" };
    for (int i = 0; i < 3; i++)
    {
        printf("“G%d‚Ì–¼‘O‚Íu%sv\n", i, monster[i]);
    }
}