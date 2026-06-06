using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardEdit : MonoBehaviour
{
    // カード名
    [SerializeField] public TMP_Text cardName;

    // カードの効果
    [SerializeField] public TMP_Text cardEffect;

    // カードのイメージ
    [SerializeField] public Image cardImage; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // カード名変更
    public void ChangeCardName(string newText)
    {
        if (cardName != null)
        {
            cardName.text = newText;

        }
    }

    public void ChangeCardImage(Sprite newSprite)
    {
        if (newSprite != null)
        {
            cardImage.sprite = newSprite;
        }

    }

    public void ChangeCardEffect(string newText)
    {
        if (cardEffect != null)
        {
            cardEffect.text = newText;
        }
    }
}
