using UnityEngine;
using UnityEngine.UI;
public class HPBarController : MonoBehaviour
{
    public Image hpBarFill; 
    //private float maxHP = 100f;
    private float currentHP;


    PlayerController player;

    void Start() 
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in objs)
        {
            player = obj.GetComponent<PlayerController>();

            if (player != null) break;
        }

        currentHP = player.maxPlayerHP; UpdateHPBar(); 
    }
    public void TakeDamage(float damage)
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0,player.maxPlayerHP);
        UpdateHPBar();
    }
    void UpdateHPBar() 
    { 
        if (hpBarFill != null) hpBarFill.fillAmount = currentHP / player.maxPlayerHP; 
    }
}