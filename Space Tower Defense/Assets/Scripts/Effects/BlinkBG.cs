using System.Collections;
using UnityEngine;

public class BlinkBG : MonoBehaviour
{
   float value = 0f;
    SpriteRenderer sr;
    int randomDelay;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        randomDelay = Random.Range(1, 101);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % randomDelay == 0)
        {
            value = Mathf.PingPong(Time.time, 3f) + 0.2f;
            sr.color = new Color(1f, 1f, 1f, value);
        }
    }

}
