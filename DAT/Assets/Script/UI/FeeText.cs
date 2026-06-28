using UnityEngine;
using TMPro;

public class FeeText : MonoBehaviour
{
    [SerializeField] private HandManager handManager;
    [SerializeField] private TextMeshProUGUI feeText;

    private int lastFee = -1;
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
    }

    
    void Update()
    {
        if (handManager != null && feeText != null)
        {
            int currentFee = handManager.cardDrawFee;

            if (currentFee != lastFee)
            {
                feeText.text = currentFee.ToString();
                lastFee = currentFee;
            }
        }
    }
}
