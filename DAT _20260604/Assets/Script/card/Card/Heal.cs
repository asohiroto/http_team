using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Heal : MonoBehaviour
{
    [SerializeField] int healAmount;

    PlayerController player;
    HandManager hand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hand = GameObject.Find("HandManager").GetComponent<HandManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Effect(int ind)
    {
        if (player.playerHP > player.maxPlayerHP - healAmount) // 回復して最大HPを超える場合は、最大HPまで回復
        {
            player.playerHP = player.maxPlayerHP;

            hand.DisCard(ind);
        }
        else if (player.playerHP == player.maxPlayerHP)
        {
            Debug.Log("元気すぎやしないかい？");
        }
        else
        {
            player.playerHP += healAmount;

            hand.DisCard(ind);
        }
        Debug.Log(player.playerHP);
    }
}
