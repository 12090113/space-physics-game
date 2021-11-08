using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyShipController : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;
    public GameObject Thrust;
    public bool thrusting = true;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (thrusting)
        {
            NewExhaust(transform.up * -3f, transform.up * -1f, 80);
        }
    }

    void NewExhaust(Vector3 offset, Vector2 direction, int size)
    {
        var newProjectile = Instantiate(Thrust);
        newProjectile.transform.position = transform.position + offset;
        var newProjectileRigid = newProjectile.GetComponent<Rigidbody2D>();
        newProjectileRigid.velocity = m_Rigidbody.velocity;
        newProjectileRigid.AddForce(direction);
        newProjectile.GetComponent<ExhaustController>().timer = size;
        newProjectile.transform.localScale = new Vector3(size / 40.0f, size / 40.0f, 0);
    }
}
