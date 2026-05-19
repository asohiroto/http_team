using UnityEngine;
using UnityEngine.UI;

public class LifeSquare : MonoBehaviour
{
    [SerializeField] private Image hpBarImage; // スライダーの代わりのImage

    public void UpdateLifeUI(int currentHp)
    {
        // 最大HPが3だと仮定（必要に応じて引数で最大HPも受け取ってください）
        float maxHp = 3f;

        // Imageの「Filled」の割合（0.0 ～ 1.0）を計算して減らす
        hpBarImage.fillAmount = currentHp / maxHp;
    }
}