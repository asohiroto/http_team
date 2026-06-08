using UnityEngine;

public class OverHeal : MonoBehaviour
{
    [SerializeField] int overHealAmount;


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
        player.playerHP += overHealAmount;

        Debug.Log(player.playerHP);

        hand.DisCard(ind);
    }
}
