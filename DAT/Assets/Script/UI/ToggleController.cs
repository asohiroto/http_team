using UnityEngine;
using UnityEngine.UI; // uGUIを使うために必要

public class ToggleController : MonoBehaviour
{
    [SerializeField] private Toggle myToggle;

    void Start()
    {
        // コードからイベントを登録する場合
        myToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    // トグルの状態が変わったときに呼ばれる関数
    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("スイッチONになりました！");
        }
        else
        {
            Debug.Log("スイッチOFFになりました！");
        }
    }
}