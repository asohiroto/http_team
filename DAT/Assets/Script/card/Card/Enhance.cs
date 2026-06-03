using UnityEngine;
using UnityEngine.XR;

public class Enhance : MonoBehaviour
{
    [SerializeField] int enhanceAmount;
    [SerializeField] int enhanceTime;

    int enhanceCount;

    bool enhanceFlag = false;

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
        enhanceCount++;


        if(enhanceFlag == true && enhanceCount >= 60 * enhanceTime)
        {
            skill.AttackCancell();
            enhanceFlag = false;
        }
        
    }

    public void Effect(int ind)
    {
        if (!enhanceFlag) // 非強化状態なら使用可能
        {
            player.attackDamage += enhanceAmount;

            enhanceFlag = true;
            enhanceCount = 0;


            Debug.Log("power = " + player.attackDamage);

            hand.DisCard(ind);
        }
        else
        {
            Debug.Log("同名の強化は重ね掛けできないよ？");
        }
    }
}
