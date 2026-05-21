#include <stdio.h>
int main(void)
{
    char title[12] = "Holy Dragon";
    title[4] = '+';
    for (int i = 0; i < 11; i++) printf("%c,", title[i]);
}