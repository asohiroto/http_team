using UnityEngine;
using TMPro;

public class WaveTimerUI : MonoBehaviour
{
    [Header("参照マネージャー")]
    [SerializeField] private WaveManager waveManager;

    [Header("UIテキスト")]
    [SerializeField] private TextMeshProUGUI timerText;

    void Update()
    {
        // テキストがセットされていないとエラー
        if (waveManager == null || timerText == null) return;

        float timeLeft = waveManager.GetTimeLeft();

        if (timeLeft <= 0f || timeLeft > 3600f)
        {
            // 0以下にならない
            timerText.text = "00:00";
            return;
        }

        // 時間をテキストに変換表示
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
