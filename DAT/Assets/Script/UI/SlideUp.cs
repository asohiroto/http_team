using UnityEngine;

public class SlideUp : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;       // スライドする速度
    [SerializeField] private float stopPositionY = 5.0f; // 目的地（このY座標に達したらリセット）

    private float startPositionY; // ゲーム開始時の初期位置を覚える変数

    void Start()
    {
        // ゲームが始まった時のY座標を自動的に記憶する
        startPositionY = transform.position.y;
    }

    void Update()
    {
        // 上へ移動 (Vector3.right から Vector3.up に変更)
        transform.Translate(Vector3.up * speed * Time.unscaledDeltaTime);

        // もし目標のY座標（stopPositionY）を越えたら
        if (transform.position.y >= stopPositionY)
        {
            // 最初の位置（startPositionY）にワープして戻す
            SetPositionY(startPositionY);
        }
    }

    // Y座標を書き換えるヘルパー関数
    private void SetPositionY(float newY)
    {
        Vector3 pos = transform.position;
        pos.y = newY;
        transform.position = pos;
    }
}