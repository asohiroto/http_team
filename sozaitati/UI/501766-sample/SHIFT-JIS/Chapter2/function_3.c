#include <stdio.h>

int rect_area(int w, int h)
{
    int a = w * h;
    return a;
}

int main(void)
{
    int a = rect_area(200, 120);
    printf("•200A‚‚³120‚Ì—Ì“y‚ğè‚É“ü‚ê‚½B\n");
    printf("‚»‚Ì—Ì“y‚Ì–ÊÏ‚Í%d‚Å‚ ‚éB\n", a);
}