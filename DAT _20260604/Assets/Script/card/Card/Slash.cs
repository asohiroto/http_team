using UnityEngine;

public class Slash : MonoBehaviour
{
    int slashCount = 0;

    [SerializeField] int slashDamage;

    GameObject slash;

    [SerializeField] GameObject slashTypePrefab;

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
        slashCount++;

        if(player.onAttack == true && slashCount >= 60 * player.attackTime * 1000)
        {
            player.onAttack = false;
            Destroy(slash);
        }
    }

    public void Effect(int ind)
    {
        player.SlashTypeAndDir(slashTypePrefab, ref slash); // 強切りを生成して使う方向を決定する
        skillAttack = slashTypePrefab.GetComponent<SkillAttack>();
        skillAttack.attackDamage = player.attackDamage + slashDamage; // 強切りのアタックダメージを代入

        hand.DisCard(ind);

        player.onAttack = true;

    }
}
