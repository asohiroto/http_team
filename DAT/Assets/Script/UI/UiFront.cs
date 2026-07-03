using UnityEngine;

public class UiFront : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ゲーム開始時に、ヒエラルキー内で自分を一番下に移動させる
        transform.SetAsLastSibling();
    }
}
