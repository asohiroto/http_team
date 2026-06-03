using UnityEngine;

public class AreaController : MonoBehaviour
{
    CraftManager craft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        craft = GameObject.Find("CraftManager").GetComponent<CraftManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("UI"))
        {
            craft.craftFlag = false;
            Debug.Log("atari");
        }
    }
}
