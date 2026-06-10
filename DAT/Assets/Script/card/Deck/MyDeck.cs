using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MyDeck : MonoBehaviour
{
    public int[] myDeckId = {-1, -1, -1, -1, -1, -1, -1, -1};

    public static MyDeck instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
