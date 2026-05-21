#include <stdio.h>
int main(void)
{
    int i1 = 10;
    int i2 = 4;
    double d1 = i1 / i2;
    double d2 = (double)i1 / (double)i2;
    printf("i1の値は %d\n", i1);
    printf("i2の値は %d\n", i2);
    printf("d1の値は %f\n", d1);
    printf("d2の値は %f\n", d2);
}