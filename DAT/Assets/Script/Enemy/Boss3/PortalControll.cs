using UnityEngine;

public class PortalControll : MonoBehaviour
{
    GameObject enemyObj;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] animSprite;
    int animFrameTimer = 0;
    // 1コマあたりのフレーム数
    int animFrame = 5;
    // アニメーション全体のコマ数
    int animMax = 0;
    // 現在のアニメーションのコマ
    int animIdx = 0;
    // ポータルから技を撃つタイミング
    int shotFrame;
    int shotFrameMax = 150;
    int shotFrameMin = 400;
    int frameTimer = 0;
    [SerializeField] GameObject[] magicPrefab;
    GameObject magicObj;
    int magicMax;
    int magicIdx;
    int destroyDelayFrame = 10;
    bool isGene = false;


    void Start()
    {
        enemyObj = GameObject.Find("EneBoss3");
        //transform.position = enemyObj.transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animMax = animSprite.Length;
        shotFrame = Random.Range(shotFrameMin, shotFrameMax);
        magicMax = magicPrefab.Length;
        magicIdx = Random.Range(0, magicMax);
    }

    void FixedUpdate()
    {
        Animation();
        Shot();
    }

    // 攻撃を生み出す処理
    void Shot()
    {
        frameTimer++;
        if(frameTimer >= shotFrame)
        {
            if(!isGene)
            {
                magicObj = Instantiate(magicPrefab[magicIdx]);
                magicObj.transform.position = transform.position;
                isGene = true;
            }
            
            if(frameTimer >= shotFrame + destroyDelayFrame)
            {
                Destroy(gameObject);
            }
        }
    }

    // アニメーションの処理
    void Animation()
    {
        animFrameTimer++;
        if(animFrameTimer >= animFrame)
        {
            animIdx++;
            animFrameTimer = 0;
            if(animIdx >= animMax)
            {
                animIdx = 0;
            }
            spriteRenderer.sprite = animSprite[animIdx];
        }
    }
}
