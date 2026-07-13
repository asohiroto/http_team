using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    public GameObject loadingScreenUI;  // ロード画面全体(親オブジェクト)
    public Image animImage;          // 右下に配置するアニメーション用Image

    public Sprite[] animationFrames;    // アニメーションのコマ画像(複数枚)
    public int frameInterval = 3;       // 何フレームごとに切り替えるか

    private int currentFrameIndex = 0;
    private int frameCounter = 0;

    float minDisplayTime = 1.0f;
    float timer = 0f;

    public void LoadingScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreenUI.SetActive(true);
        currentFrameIndex = 0;
        frameCounter = 0;

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

    private void UpdateAnimation()
    {
        if (animationFrames == null || animationFrames.Length == 0) return;

        frameCounter++;

        if (frameCounter >= frameInterval)
        {
            frameCounter = 0; // カウンターをリセット
            currentFrameIndex = (currentFrameIndex + 1) % animationFrames.Length; // %でループ
            animImage.sprite = animationFrames[currentFrameIndex];
        }
    }
}