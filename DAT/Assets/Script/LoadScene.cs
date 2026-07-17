using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    public GameObject loadingScreenUI;  // ロード画面全体(親オブジェクト)
    public Image animImage;          // 右下に配置するアニメーション用Image

    public Sprite[] animationFrames;    // アニメーションのコマ画像(複数枚)

    private int currentFrameIndex = 0;

    float minDisplayTime = 1.0f;
    float timer = 0f;

    public float updatesPerSecond = 60f; // 1秒間に何回更新するか
    private float animInterval;          // 1回あたりの間隔(秒)
    private float animTimer = 0f;

    DeckRegistrate regis;

    void Awake()
    {
        animInterval = 1f / updatesPerSecond;
        regis = GameObject.Find("Panel").GetComponentInChildren<DeckRegistrate>();
    }

    public void LoadingScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (regis.CheckMyDeck(-1))
        {
            loadingScreenUI.SetActive(true);
            currentFrameIndex = 0;

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false; // 読み込み完了後すぐに切り替えない

            while (!operation.isDone)
            {
                timer += Time.deltaTime;

                // 右下アイコンのコマ送りアニメーションを更新
                UpdateAnimation();


                // progressは0.0〜0.9までしか上がらない仕様
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                if (operation.progress >= 0.9f && timer >= minDisplayTime)
                {
                    operation.allowSceneActivation = true;
                }

                yield return null;
            }

            loadingScreenUI.SetActive(false);
        }
    }

    private void UpdateAnimation()
    {
        if (animationFrames == null || animationFrames.Length == 0) return;

        animTimer += Time.deltaTime;

        // フレームレートが低くて複数回分溜まった場合もwhileで消化する
        while (animTimer >= animInterval)
        {
            animTimer -= animInterval;
            currentFrameIndex = (currentFrameIndex + 1) % animationFrames.Length;
            animImage.sprite = animationFrames[currentFrameIndex];
        }
    }
}