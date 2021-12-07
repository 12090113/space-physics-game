using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoController : MonoBehaviour
{
    public int health = 100;
    private FixedJoint2D hinge;
    private new SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<FixedJoint2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        health -= (int)col.relativeVelocity.magnitude / 4;
        float cargoShade = Mathf.Clamp(health/200.0f + 0.5f, 0.5f, 1);
        renderer.color = new Color(cargoShade, cargoShade, cargoShade, 1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        health -= 50;
        float cargoShade = Mathf.Clamp(health/200.0f + 0.5f, 0.5f, 1);
        renderer.color = new Color(cargoShade, cargoShade, cargoShade, 1f);
    }

    private void Die()
    {
        Destroy(hinge);
        Destroy(this);
    }
}
