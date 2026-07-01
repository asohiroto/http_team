using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector2 destination;
    [SerializeField] private bool isMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    /// <summary>
    /// 矢の方向、目的地をセットする
    /// </summary>
    /// <param name="dir">移動方向(正規化済み)</param>
    /// <param name="dest">目的地</param>
    public void SetArrowAttack(Vector2 dir, Vector2 dest)
    {
        direction = dir;
        destination = dest;
        isMove = true;
    }
}
