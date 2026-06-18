using UnityEngine;

public class EneBoss3 : MonoBehaviour
{
    enum State { Idle, Beem, Attack2, Attack3 };
    State state = 0;
    // Shotに使う変数---------------------------
    enum BeemState { Aim, Shot};
    BeemState beemState = 0;
    [SerializeField] GameObject beemPrefab;
    [SerializeField] GameObject beemRangePrefab;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Beem()
    {
        switch(beemState)
        {
            case BeemState.Aim:
            break;
            case BeemState.Shot:
            break;
        }
    }
}
