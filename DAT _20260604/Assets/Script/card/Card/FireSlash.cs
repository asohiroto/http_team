using UnityEngine;
using UnityEngine.XR;

public class FireSlash : MonoBehaviour
{
    int fireSlashCount = 0;

    [SerializeField] int fireSlashDamage;

    GameObject fireSlash;

    [SerializeField] GameObject fireSlashTypePrefab;

    PlayerController player;
    HandManager hand;
    SkillAttack skillAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hand = GameObject.Find("HandManager").GetComponent<HandManager>();
    }

    // Update is called once per frame
    void Update()
    {

        fireSlashCount++;

        if (player.onAttack == true && fireSlashCount >= 60 * player.attackTime * 1000)
        {
            player.onAttack = false;
            Destroy(fireSlash);
        }
    }

    public void Effect(int ind)
    {
        player.SlashTypeAndDir(fireSlashTypePrefab, ref fireSlash); // 強切りを生成して使う方向を決定する
        skillAttack = fireSlashTypePrefab.GetComponent<SkillAttack>();
        skillAttack.attackDamage = player.attackDamage + fireSlashDamage; // 火の強切りのアタックダメージを代入

        hand.DisCard(ind);

        player.onAttack = true;

    }
}
