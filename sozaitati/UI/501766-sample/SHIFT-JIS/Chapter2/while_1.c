#include <stdio.h>
int main(void)
{
    int strength = 1;
    printf("˜r—Í(strength)‚Ì‰Šú’l‚Í%dB\n", strength);
    printf("˜r—Í”{‘‚Ì–‚–@‚ğ‚©‚¯‘±‚¯‚é‚¼I\n");
    while (strength < 128)
    {
        strength = strength * 2;
        printf("˜r—Í‚ª%d‚É‚È‚Á‚½I\n", strength);
    }
}