#include <stdio.h>
int main(void)
{
    int map[3][5] =
    {
        {-1,-2,-3,-4,-5},
        { 1, 2, 3, 4, 5},
        {10,20,30,40,50}
    };
    printf("map[0][0]‚Ì’l‚Í %d\n", map[0][0]);
    printf("map[1][2]‚Ì’l‚Í %d\n", map[1][2]);
    printf("map[2][4]‚Ì’l‚Í %d\n", map[2][4]);
}