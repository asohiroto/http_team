using UnityEngine;

public class CursorUI : MonoBehaviour
{
    void Start()
    {
        // ゲーム開始時に、ヒエラルキー内で自分を一番下に移動させる
        transform.SetAsLastSibling();

       
    }

    void Update()
    {
        transform.position = Input.mousePosition;

        Cursor.visible = false;
    }
}