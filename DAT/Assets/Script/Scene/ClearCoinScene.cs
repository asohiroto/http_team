using UnityEngine;

public class ClearCoinScene : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 180 * Time.deltaTime);

        bool appeared = false;

        void OnBecameVisible()
        {
            appeared = true;
        }

        void OnBecameInvisible()
        {
            if (appeared)
            {
                Destroy(gameObject);
            }
        }
    }
}