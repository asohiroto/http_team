#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
int main(void)
{
    char data[3][9];
    FILE* fp;
    fp = fopen("save_data.txt", "r");
    if (fp == NULL)
    {
        printf("ファイルを開くことができません");
        return -1;
    }
    for (int i = 0; i < 3; i++) fscanf(fp, "%s", data[i]);
    fclose(fp);
    for (int i = 0; i < 3; i++) printf("%s\n", data[i]);
}