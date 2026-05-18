using UnityEngine;

public class WeakArcher : MonoBehaviour
{
    [SerializeField] private Sprite[] staySpr;
    [SerializeField] private int stayIdx;

    [SerializeField] private int timer;

    SpriteRenderer spr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // staySpr の枚数カウント
        int count = staySpr.Length;

        this.spr.sprite = staySpr[timer / 10 % count];
        timer++;
    }
}
