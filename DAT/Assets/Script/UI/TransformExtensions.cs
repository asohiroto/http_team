using UnityEngine;

public class UIAlwaysOn : MonoBehaviour
{
    [SerializeField] private Transform target; // プレイヤー
    [SerializeField] private Vector3 offset;    // ズレ

    private Camera mainCamera;
    private RectTransform rectTransform;

    // ゲームが起動した「一番最初」に実行される（Startや土台のエラーよりも前）
    void Awake()
    {
        // 自分自身（LifeSquare (1)）を強制的に「常時オン」にする
        gameObject.SetActive(true);
    }

    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        // クリックされたかどうかに関わらず、毎フレーム「オン」であることを強制する
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        if (target == null || rectTransform == null || mainCamera == null) return;

        // プレイヤーの足元に座標を合わせる
        Vector2 screenPoint = mainCamera.WorldToScreenPoint(target.position);
        rectTransform.position = new Vector3(screenPoint.x + offset.x, screenPoint.y + offset.y, 0f);
    }
}