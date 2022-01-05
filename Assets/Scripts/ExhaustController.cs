using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustController : MonoBehaviour
{
    public int timer = 20;
    //private new SpriteRenderer renderer;

    private void Start()
    {
        //renderer = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        timer--;
        transform.localScale = new Vector3(timer / 50.0f, timer / 50.0f, 0);
        //renderer.color = new Color(1, 1-10f/timer, timer / 100f, 0.5f);
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
