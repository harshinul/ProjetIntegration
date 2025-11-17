using UnityEngine;

public class spinComponent : MonoBehaviour
{
    float time = 0;
    float height;
    float lastHeight = 0;
    float initialHeight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        height = (0.07f * Mathf.Sin(time * 7)) + initialHeight;
        height -= lastHeight;
        transform.Translate(new Vector3(0, height, 0));
        transform.Rotate(new Vector3(0, -100, 0) * Time.deltaTime, Space.Self);
        lastHeight = transform.position.y;
    }
}
