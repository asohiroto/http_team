using UnityEngine;

public class UIstrictFollower : MonoBehaviour
{
    // インスペクターからプレイヤー（Player）をドラッグ＆ドロップする
    [SerializeField] private Transform targetPlayer;

    // プレイヤーの足元にずらすための値（インスペクターで調整可能。例: Yを -1.5 にする）
    [SerializeField] private Vector3 positionOffset = new Vector3(0f, -1.5f, 0f);

    void Awake()
    {
        // 起動した瞬間、何があっても強制的に「表示（オン）」状態にする
        gameObject.SetActive(true);
    }

    // すべての処理（プレイヤーの移動や土台のエラーなど）が終わった後に位置を確定させる
    void LateUpdate()
    {
        // ターゲット（プレイヤー）がセットされていない、または消えている場合は何もしない
        if (targetPlayer == null) return;

        // 【バグ対策】もし勝手に非表示（オフ）にされても、毎フレーム強制的にオンに戻す
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        // プレイヤーの現在位置に、ずらしたい分のオフセットを足す
        Vector3 targetPosition = targetPlayer.position + positionOffset;

        // Z軸は0（2Dゲームの標準位置）に固定し、カメラの裏に隠れるのを防ぐ
        targetPosition.z = 0f;

        // 自分の位置をプレイヤーの位置に強制同期
        transform.position = targetPosition;
    }
}