using UnityEngine;

public class DestroyByPosition : MonoBehaviour
{
    // 消去する下限のY座標（ゲームの画面に合わせて調整してください）
    public float thresholdY = -6f; 

    void Update()
    {
        // コインのY座標が指定した値より小さくなったら
        if (transform.position.y < thresholdY)
        {
            Destroy(gameObject);
        }
    }
}