using UnityEngine;

public class Animation : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField]private int fps;
    private int oldFps;
    private int currentFrame;
    private float secondsPerFrame;
    private float timer;
    private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyFrameRate();
    }

    // フレームレート変更時の処理
   private void Update()
    {
        ApplyFrameRate();
        timer -= Time.deltaTime;
        UpdateAnimation();
    }

    private void ApplyFrameRate()
    {
        if (oldFps == fps) return;
        fps = Mathf.Max(1, fps);
        oldFps = fps;

        secondsPerFrame = 1 / (float)fps;
        timer = secondsPerFrame;
        currentFrame = 0;
    }

    // アニメーションの更新
    private void UpdateAnimation()
    {
        if (timer > 0) return;
        timer += secondsPerFrame;   //+=にすることでずれをなくす
        spriteRenderer.sprite = sprites[currentFrame];
        currentFrame = (currentFrame + 1) % sprites.Length;
    }
}
