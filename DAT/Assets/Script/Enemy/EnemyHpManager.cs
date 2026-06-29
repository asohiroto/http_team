using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyHpManager : MonoBehaviour
{
    [SerializeField] private int hp;
    public int GetCurrentHp()
    {
        return hp;
    }

    public void EnemyDamaged(int dmg)
    {
        hp -= dmg;
    }
}
