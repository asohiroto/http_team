using UnityEngine;

public class MouseFollowerWorld : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform rectTransform;


    void Start()
    {
       
            //マウスポインター非表示(左クリックでマウスポインター非表示、Escキーでポインター表示)
            Cursor.visible = false;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // マウスのスクリーン座標を取得
        Vector3 mousePos = Input.mousePosition;

        // カメラの「手前（Near Clip Plane）」の距離をZに指定する
        mousePos.z = mainCamera.nearClipPlane;

        // ワールド座標に変換
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        // スプライトのZ位置は 0（手前）に固定して移動させる
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f);

        
    }
}