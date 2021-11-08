using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustController : MonoBehaviour
{
    public int timer = 20;

    void FixedUpdate()
    {
        timer--;
        transform.localScale = new Vector3(timer / 50.0f, timer / 50.0f, 0);
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
