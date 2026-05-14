using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class h_PlayerController : MonoBehaviour
{
    public Vector2 pInput = new Vector2(0, 0);

    [SerializeField] private float p_moveSpeed = 1;

    public Vector2 pPos = new Vector2(0, 0);

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyInput();
        PlayerPos();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerPos()
    {
        pPos = rb.position;
    }

    void CheckKeyInput()
    {
        // 上下
        if (Keyboard.current.wKey.wasPressedThisFrame) pInput.y = 1;
        if (Keyboard.current.sKey.wasPressedThisFrame) pInput.y = -1;
        // 左右
        if (Keyboard.current.dKey.wasPressedThisFrame) pInput.x = 1;
        if (Keyboard.current.aKey.wasPressedThisFrame) pInput.x = -1;
        // 入力なし
        if (!Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed) pInput.y = 0;
        if (!Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed) pInput.x = 0;

        // 正規化
        pInput.Normalize();

    }

    void PlayerMove()
    {
        rb.linearVelocity = new Vector2(pInput.x * p_moveSpeed, pInput.y * p_moveSpeed);
    }
}
