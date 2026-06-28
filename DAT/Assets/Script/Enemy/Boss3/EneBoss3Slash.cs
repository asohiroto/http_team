using UnityEngine;

public class EneBoss3Slash : MonoBehaviour
{
    float viewAngle = 90f;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Vector3 forwardDirection = (transform.up + transform.right).normalized;

            // 自分からターゲットへの方向ベクトルを計算する
            Vector3 dirToTarget = (col.transform.position - transform.position);

            // 正面方向とターゲットへの方向のなす角
            float angle = Vector3.Angle(forwardDirection, dirToTarget);

            // なす角が設定角度の半分以下なら扇形の範囲内にいるとみなす
            if(angle <= viewAngle / 2f)
            {
                Debug.Log("扇形内にいる");
            }
        }
    }
}
