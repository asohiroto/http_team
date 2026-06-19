using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;// アニメーション開始
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreditSceneChange()
    {
        SceneManager.LoadScene("CreditScene");
    }
}
