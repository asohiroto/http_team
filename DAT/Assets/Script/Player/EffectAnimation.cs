using System.Collections;
using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] effectSprite;
    float animSecPerFrame = 0.05f; // 1コマあたりの秒数
        void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Anim());
    }
    
    IEnumerator Anim()
    {
        for(int i = 0; i < effectSprite.Length; i++)
        {
            spriteRenderer.sprite = effectSprite[i];

            yield return new WaitForSeconds(animSecPerFrame);
        }
        Destroy(gameObject);
    }
}
