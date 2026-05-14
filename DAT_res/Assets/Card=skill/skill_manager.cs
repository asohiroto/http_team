using UnityEngine;
using UnityEngine.InputSystem;

public class skill_manager : MonoBehaviour
{

    heal_manager heal; 
    enhance_manager enhance;
    void Start()
    {
        heal = GetComponent<heal_manager>();
        enhance = GetComponent<enhance_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame) // ｚを押してヒールする
        {
            heal.Heal();

        }
        if(Keyboard.current.xKey.wasPressedThisFrame) // ｘをおすと攻撃力アップ
        {
            enhance.Enhance();
        }
    }
}
