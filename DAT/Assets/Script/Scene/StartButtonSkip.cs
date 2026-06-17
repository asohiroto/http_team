using UnityEngine;

public class AnimationSkip : MonoBehaviour
{
    public Animator animator;
    public string stateName = "GameStartButton"; // アニメーション名

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // アニメーションを最後まで進める
            animator.Play(stateName, 0, 1.0f);
            animator.Update(0);
        }
    }
}