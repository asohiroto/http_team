/*
第２章
入出力の学習用プログラム
*/
#define _CRT_SECURE_NO_WARNINGS // Visual Studioでscanf()を使うために記述
#include <stdio.h>
int main(void) // これがmain関数
{
    int life; // 整数を扱う変数
    printf("主人公の体力値を入力してください\n");
    scanf("%d", &life); /* 入力を受け付ける状態になる */
    printf("主人公の体力は%dです", life);
}
