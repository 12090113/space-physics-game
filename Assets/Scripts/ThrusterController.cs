using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    public float size = 100.0f;
    Rigidbody2D m_Rigidbody;
    public GameObject Exhaust;
    public float offset = -1;
    private Vector3 offset3;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }
    /*
    void FixedUpdate()
    {
        
    }
    */
    public void Thrust()
    {
        m_Rigidbody.AddForce(transform.up * size);
        GameObject newProjectile = Instantiate(Exhaust);
        offset3 = transform.up * offset;
        newProjectile.transform.position = transform.position + offset3;
        var newProjectileRigid = newProjectile.GetComponent<Rigidbody2D>();
        newProjectileRigid.velocity = m_Rigidbody.velocity;
        newProjectileRigid.AddForce(-transform.up);
        newProjectile.GetComponent<ExhaustController>().timer = (int) (size * .75);
        newProjectile.transform.localScale = new Vector3(size / 50.0f, size / 50.0f, 0);
    }
}
