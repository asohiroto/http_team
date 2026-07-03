using UnityEngine;
using TMPro;

public class DeleteText : MonoBehaviour
{
    public GameObject textObject;

    public void HideText()
    { 
        if(textObject != null)
        {
            // テキスト非表示
            textObject.SetActive(false);
        }

    }
}
