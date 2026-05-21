#include <stdio.h>
int main(void)
{
    int scene = 1;
    printf("変数sceneの値は%d\n", scene);
    switch (scene)
    {
    case 1:
        printf("タイトル画面の処理を行います。");
        break;
    case 2:
        printf("移動画面の処理を行います。");
        break;
    case 3:
        printf("戦闘画面の処理を行います。");
        break;
    default:
        printf("1～3以外の処理を行うブロックです。");
        break;
    }
}