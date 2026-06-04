using UnityEngine;
using UnityEngine.XR;

public class HyperMode : MonoBehaviour
{
    [SerializeField] int hyperDamageAmount; 
    [SerializeField] float hyperSpeedAmount;
    [SerializeField] int hyperTime;

    int hyperCount;

    bool hyperFlag = false;

    PlayerController player;
    HandManager hand;
    SkillManager skill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hand = GameObject.Find("HandManager").GetComponent<HandManager>();
        skill = GameObject.Find("SkillManager").GetComponent<SkillManager>();
    }

    // Update is called once per frame
    void Update()
    {
        hyperCount++;

        if (hyperFlag && hyperCount > hyperTime * 60) // ハイパーモードの解除
        {
            skill.AttackCancell();
            skill.SpeedCancell();

            hyperFlag = false;
        }
    }

    public void Effect(int ind)
    {
        if (!hyperFlag) // 非強化状態なら使用可能
        {
            player.attackDamage += hyperDamageAmount;
            player.speed += hyperSpeedAmount;

            hyperFlag = true;
            hyperCount = 0;


            Debug.Log("power = " + player.attackDamage);

            hand.DisCard(ind);
        }
        else
        {
            Debug.Log("同名の強化は重ね掛けできないよ？");
        }
    }
}
