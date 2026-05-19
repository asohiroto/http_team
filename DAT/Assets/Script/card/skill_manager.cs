using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class skill_manager : MonoBehaviour
{
    player_manager_kari player;

    [SerializeField] int healAmount;
    [SerializeField] int enhanceAmount;
    [SerializeField] int enhanceTime;
    int time = 0;
    int enhanceFlag = 0;
    

    void Start()
    {
        player = GameObject.Find("Square").GetComponent<player_manager_kari>();
        
    }

    // Update is called once per frame
    void Update()
    {
        time++;

        if(enhanceFlag == 1 && time > enhanceTime * 60)
        {
            player.power = player.firstPower;
            Debug.Log("Power : " + player.power);

            enhanceFlag = 0;
        }


    }


    public void Slash()
    {

    }

    public void Heal()
    {
        if(player.hp > player.maxHp - healAmount)
        {
            player.hp = player.maxHp;
        }
        else
        {
            player.hp += healAmount;
        }

        Debug.Log(player.hp);
    }

    public void Enhance()
    {
        player.power += enhanceAmount;

        enhanceFlag = 1;
        time = 0;

        Debug.Log(player.power);
    }

    public void FireBall()
    {

    }


}

