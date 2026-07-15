using UnityEngine;
using TMPro;

public class WaveTimerUI : MonoBehaviour
{
    [Header("参照スクリプト")]
    [SerializeField] private WaveManager waveManager;

    [Header("UIテキスト")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Waveテキスト")]
    [SerializeField] private TextMeshProUGUI waveText;

    void Update()
    {

        // テキストがセットされていないとエラー
        if (waveManager == null || timerText == null || waveText == null) return;

        // ウェーブ表示
        WaveState currentState = waveManager.GetCurrentWaveState();

        timerText.gameObject.SetActive(true);


        switch (currentState)
        {
            case WaveState.Wave1:
                waveText.text = "<WAVE 1>";
                timerText.gameObject.SetActive(true);
                break;

            case WaveState.Wave2:
                waveText.text = "<WAVE 2>";
                timerText.gameObject.SetActive(true);
                break;

            case WaveState.Boss:
                waveText.text = "<ボスを倒せ!>";
                break;

            

            case WaveState.Interval:
                waveText.text = "<NEXT WAVE>";
                timerText.gameObject.SetActive(true);
                break;

            case WaveState.Shop:
                waveText.text = "<SHOP>";
                break;
            
            case WaveState.LevelClearWait:
                int currentLevel = waveManager.GetCurrentLevelIndex();

                if (currentLevel >= 2)
                {
                    waveText.text = "Congratulations!";
                    timerText.gameObject.SetActive(false);
                }
                else
                {
                    waveText.text = "<LEVEL CLEAR!>";
                }
                break;

            default:
                waveText.text = currentState.ToString();
                break;
        }

        float timeLeft = waveManager.GetTimeLeft();

        if (timeLeft <= 0f || timeLeft > 3600f)
        {
            // 0秒以下にならない
            timerText.text = "00:00";
            // return;
        }
        else
        {
            // 時間をテキストに変換表示
            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = Mathf.FloorToInt(timeLeft % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }


    }
}
