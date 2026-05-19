using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HPManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI hpText;  // TextMeshProのテキスト
    [SerializeField] private Slider hpSlider;         // 現在のHPバー
    [SerializeField] private Slider effectSlider;     // 遅れて削れる演出用バー（任意）

    [Header("HP Settings")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private string spriteTag = "<sprite name=\"heart\">"; // 使用するスプライトタグ

    private float currentHP;
    private Coroutine larpCoroutine;

    void Start()
    {
        currentHP = maxHP;
        UpdateUI(instantly: true);
    }

    // ダメージを受ける関数（他のスクリプトから呼び出す）
    public void TakeDamage(float damage)
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);
        UpdateUI(instantly: false);
    }

    // UIの更新
    private void UpdateUI(bool instantly)
    {
        // 1. テキストの更新 (例: ❤️ HP: 80 / 100)
        hpText.text = $"{spriteTag} HP: {Mathf.CeilToInt(currentHP)} / {maxHP}";

        // 2. HPバーの更新
        float hpRatio = currentHP / maxHP;
        hpSlider.value = hpRatio;

        // 3. 遅れて削れる演出
        if (instantly)
        {
            if (effectSlider != null) effectSlider.value = hpRatio;
        }
        else
        {
            if (larpCoroutine != null) StopCoroutine(larpCoroutine);
            larpCoroutine = StartCoroutine(AnimateEffectBar(hpRatio));
        }
    }

    // バーがじわじわ削れるアニメーション
    private IEnumerator AnimateEffectBar(float targetRatio)
    {
        if (effectSlider == null) yield break;

        // 0.5秒待ってから削れ始める（タメを作る）
        yield return new WaitForSeconds(0.5f);

        float speed = 2f; // 削れるスピード
        while (Mathf.Abs(effectSlider.value - targetRatio) > 0.001f)
        {
            effectSlider.value = Mathf.MoveTowards(effectSlider.value, targetRatio, speed * Time.deltaTime);
            yield return null;
        }
        effectSlider.value = targetRatio;
    }
}