using UnityEngine;

public class RecycleEffect : MonoBehaviour
{

    float elapsed = 0;
    float soundTime;
    public void Recycle(float time)
    {
        elapsed = 0;
        soundTime = time;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed > soundTime)
        {
            this.gameObject.SetActive(false);
        }
    }
}
