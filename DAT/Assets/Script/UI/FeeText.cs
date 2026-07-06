using UnityEngine;
using TMPro;

public class FeeText : MonoBehaviour
{
    [SerializeField] private HandManager handManager;
    [SerializeField] private TextMeshProUGUI feeText;

    private int lastFee = -1;
    private int predictedDrawCount = 0;
    void Start()
    {
        if(handManager == null)
        {
            handManager = Object.FindFirstObjectByType<HandManager>();
        }

        if (feeText == null)
        {
            feeText = GetComponent<TextMeshProUGUI>();
        }

        if (handManager != null)
        {
            int nextFee = handManager.cardDrawFee + 1;
            feeText.text = nextFee.ToString();
            lastFee = handManager.cardDrawFee;
            predictedDrawCount = 1;
        }
    }

    
    void Update()
    {
        if(handManager == null || feeText == null) return;

        if (handManager.cardDrawFee != lastFee)
        {
            predictedDrawCount++;


            int nextFee = handManager.cardDrawFee + predictedDrawCount;

            feeText.text = nextFee.ToString();
        }
        lastFee = handManager.cardDrawFee;
    }
}
