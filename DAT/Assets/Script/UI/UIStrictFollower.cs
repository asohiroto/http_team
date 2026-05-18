using UnityEngine;

public class UIFollowerFix : MonoBehaviour
{
    [SerializeField] private Transform target; // プレイヤーのTransform
    [SerializeField] private Vector3 offset;    // プレイヤーからのズレ（最初は X:0, Y:-50, Z:0 くらいがおすすめ）

    private Camera mainCamera;
    private RectTransform rectTransform;

    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();

        // 【重要】ゲーム開始時に強制的にオブジェクトとコンポーネントを有効化
        gameObject.SetActive(true);
        if (rectTransform != null)
        {
            // 画面のどこにでも動けるようにアンカーを真ん中に固定
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        }
    }

    void LateUpdate()
    {
        if (target == null || rectTransform == null || mainCamera == null) return;

        // 1. プレイヤーのワールド座標を取得
        Vector3 playerWorldPos = target.position;

        // 2. プレイヤーの座標を、画面上のスクリーン座標（ピクセル単位）に変換
        Vector2 screenPoint = mainCamera.WorldToScreenPoint(playerWorldPos);

        // 3. UIの RectTransform.position に直接代入する（これが一番ズレない）
        // オフセット（Y: -50 など）を足して、プレイヤーの足元にずらす
        rectTransform.position = new Vector3(screenPoint.x + offset.x, screenPoint.y + offset.y, 0f);
    }
}