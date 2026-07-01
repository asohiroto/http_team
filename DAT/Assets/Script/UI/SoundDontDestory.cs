using UnityEngine;

public class SoundDontDestory : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
