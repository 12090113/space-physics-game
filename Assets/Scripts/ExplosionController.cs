using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private int timer = 100;
    bool center = false;

    // Start is called before the first frame update
    void Start()
    {
        if (name != "Explosion Center")
        {
            timer -= Random.Range(0, 20);
        } else
        {
            center = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer--;
        transform.localScale = new Vector3(timer/50.0f, timer/50.0f, 0);
        if (timer <= 0)
        {
            if (center)
            {
                GameObject.Find("Main Camera").GetComponent<CameraController>().SwitchToPlayer();
            }
            Destroy(this.gameObject);
        }
    }
}
