using UnityEngine;

public class EnemyHpManager : MonoBehaviour
{
    [SerializeField] private int hp;
    private bool takeDamage = false;

    public int GetCurrentHp()
    {
        return hp;
    }

    public void EnemyDamaged(int dmg)
    {
        takeDamage = true;
        hp -= dmg;
    }

    public bool TakeDamage()
    {
        if (takeDamage)
        {
            takeDamage = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
