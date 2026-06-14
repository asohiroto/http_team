using UnityEngine;

public class TitleCardSkip : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Animationウィンドウで作った、フェードインアニメーションの「ステート名」
    [SerializeField] private string fadeAnimationName = "TitleCardAnimation";

    void Update()
    {
        // 画面がクリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            // 現在の再生状態を取得
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // もし今「FadeIn」アニメーションが再生中、かつ、まだ途中の場合
            if (stateInfo.IsName(fadeAnimationName) && stateInfo.normalizedTime < 1.0f)
            {
                // 再生位置を「1.0（100%＝最後まで完了）」に強制変更する
                animator.Play(fadeAnimationName, 0, 1.0f);
            }
        }
    }
}