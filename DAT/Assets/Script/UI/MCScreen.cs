using UnityEngine;
using UnityEngine.InputSystem;

public class MCScreen : MonoBehaviour
{
    Vector3 mousePosScreen = Vector3.zero;


    void Start()
    {
        // ゲーム開始時に、ヒエラルキー内で自分を一番下に移動させる
        transform.SetAsLastSibling();


    }

    void Update()
    {
        // 各座標を入力
        mousePosScreen.x = Mouse.current.position.x.ReadValue();
        mousePosScreen.y = Mouse.current.position.y.ReadValue();
        mousePosScreen.z = -Camera.main.transform.position.z;

        transform.position = mousePosScreen;

        Cursor.visible = false;
        //transform.position = Input.mousePosition;

        //Cursor.visible = false;
    }
}