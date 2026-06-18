using UnityEngine;

public class CardUseEffectManager : MonoBehaviour
{
    PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
