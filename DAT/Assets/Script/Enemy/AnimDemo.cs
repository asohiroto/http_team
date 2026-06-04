using UnityEngine;

public class AnimDemo : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int fps;
    private int oldFps;
    private int currentFrame;
    private float secondsPerFrame;
    private float timer;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyFrameRate();
    }

    private void Update()
    {
        ApplyFrameRate();
        timer -= Time.deltaTime;
        UpdateAnimation();
    }

    /// <summary>
    /// フレームレート変更時の処理
    /// </summary>
    private void ApplyFrameRate()
    {
        if (oldFps == fps) return;
        fps = Mathf.Max(1, fps);
        oldFps = fps;

        secondsPerFrame = 1 / (float)fps;
        timer = secondsPerFrame;
        currentFrame = 0;
    }

    /// <summary>
    /// アニメーションの更新
    /// </summary>
    private void UpdateAnimation()
    {
        if (timer > 0) return;
        timer += secondsPerFrame;   // +=にすることでずれをなくす
        spriteRenderer.sprite = sprites[currentFrame];
        currentFrame = (currentFrame + 1) % sprites.Length;
    }
}