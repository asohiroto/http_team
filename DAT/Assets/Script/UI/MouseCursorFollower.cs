using UnityEngine;

public class SimpleCursor : MonoBehaviour
{
    void Start()
    {
        // ゲーム開始時に、ヒエラルキー内で自分を一番下に移動させる
        transform.SetAsLastSibling();
    }

    void Update()
    {
        transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false;
        }
    }
}