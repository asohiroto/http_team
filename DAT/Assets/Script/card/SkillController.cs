using UnityEngine;

public class SkillController : MonoBehaviour
{
    EnemyController enem;
    SkillManager skill;

    GameObject enemyObj;

    [SerializeField] int skillDmg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Card");

        foreach(GameObject obj in objs)
        {
            skill = obj.GetComponent<SkillManager>();

            if (skill != null) break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemyObj = col.gameObject;
            enem = enemyObj.GetComponent<EnemyController>();
            enem.EnemyDamaged(skillDmg);

            skill.absorbFlag = true;

        }
    }
}
