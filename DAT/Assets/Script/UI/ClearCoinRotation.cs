using UnityEngine;

public class ClearCoinRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 180 * Time.deltaTime);
    }
}
