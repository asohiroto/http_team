using UnityEngine;
using UnityEngine.InputSystem;

public class player_manager_kari : MonoBehaviour
{
    [SerializeField] public int Hp = 100;
    [SerializeField] public int power = 100;
    void Start()
    {
        
    }

    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Hp -= 10;
            Debug.Log(Hp);
        }
    }
}
