using UnityEngine;

public class SlideLoop : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;       // スライドする速度
    [SerializeField] private float stopPositionX = 3.0f; // 目的地（このX座標に達したらリセット）

    private float startPositionX; // ゲーム開始時の初期位置を覚える変数

    void Start()
    {
        // ゲームが始まった時のX座標を自動的に記憶する
        startPositionX = transform.position.x;
    }

    void Update()
    {
        // 右へ移動
        transform.Translate(Vector3.right * speed * Time.unscaledDeltaTime);

        // もし目標のX座標（stopPositionX）を越えたら
        if (transform.position.x >= stopPositionX)
        {
            // 最初の位置（startPositionX）にワープして戻す
            SetPositionX(startPositionX);
        }
    }

    // X座標を書き換えるヘルパー関数
    private void SetPositionX(float newX)
    {
        Vector3 pos = transform.position;
        pos.x = newX;
        transform.position = pos;
    }
}