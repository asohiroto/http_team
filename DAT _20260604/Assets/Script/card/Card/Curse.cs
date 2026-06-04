using UnityEngine;
using UnityEngine.XR;

public class Curse : MonoBehaviour
{
    [SerializeField] int curseAmount; 

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
        if (player.playerHP < curseAmount)
        {
            player.playerHP = 1;
        }
        else
        {
            player.playerHP -= curseAmount;

            hand.DisCard(ind);
        }
    }
}
