using UnityEngine;
public class HpFollowTarget : MonoBehaviour
{
    public Transform target; // プレイヤー
    public Vector3 offset = new Vector3(0, -0.5f, 0); // 頭上の高さ
    private RectTransform rectTransform;
    private Camera mainCamera;

    void Start() 
    { 
        rectTransform = GetComponent<RectTransform>(); mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        if (target == null || mainCamera == null) return;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position + offset);
        if (screenPos.z < 0) { rectTransform.gameObject.SetActive(false); return; }
        else { rectTransform.gameObject.SetActive(true); }
        rectTransform.position = screenPos;
    }
}