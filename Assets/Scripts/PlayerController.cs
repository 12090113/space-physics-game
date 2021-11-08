using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MaxRotate = 400.0f;
    private const float rotationSpeed = 1.0f;
    private const float m_Speed = 10.0f;
    Rigidbody2D m_Rigidbody;
    public GameObject Launcher;
    public GameObject Explosion;
    public GameObject Exhaust;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
            if (Input.GetKey(KeyCode.W))
            {
                m_Rigidbody.AddForce(transform.up * m_Speed);
                NewExhaust(transform.up * -0.4f, transform.up * -1f, 20);
            }
            if (Input.GetKey(KeyCode.D) && m_Rigidbody.angularVelocity > -MaxRotate)
            {
                m_Rigidbody.AddTorque(-rotationSpeed);
                NewExhaust(transform.right * -0.15f + transform.up * 0.4f, transform.right * -1f, 10);
            }
            else if (Input.GetKey(KeyCode.A) && m_Rigidbody.angularVelocity < MaxRotate)
            {
                m_Rigidbody.AddTorque(rotationSpeed);
                NewExhaust(transform.right * 0.1f + transform.up * 0.4f, transform.right * 1f, 10);
            }
            else if (m_Rigidbody.angularVelocity < -12)
            {
                m_Rigidbody.AddTorque(rotationSpeed);
                NewExhaust(transform.right * 0.1f + transform.up * 0.4f, transform.right * 1f, 10);
            }
            else if (m_Rigidbody.angularVelocity > 12)
            {
                m_Rigidbody.AddTorque(-rotationSpeed);
                NewExhaust(transform.right * -0.1f + transform.up * 0.4f, transform.right * -1f, 10);
            }
            else
            {
                m_Rigidbody.angularVelocity = 0;
            }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Detonate();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ((int)col.relativeVelocity.magnitude / 4 > 4)
        {
            //Debug.Log((int)col.relativeVelocity.magnitude / 4);
            m_Rigidbody.AddForce(col.relativeVelocity);
            Detonate();
        }
    }

    void Detonate()
    {
        for (int i = 0; i < 30; i++)
        {
            NewExplosion(12 * i, 100 + UnityEngine.Random.Range(-90.0f, 100.0f));
        }
        NewExplosion(0, 0, true);
        transform.position = Launcher.transform.position + Launcher.transform.up * 10;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        m_Rigidbody.velocity = Launcher.GetComponent<Rigidbody2D>().velocity;
        m_Rigidbody.angularVelocity = 0;
    }
    void NewExplosion(float Rotation, float Velocity, bool center)
    {
        var newProjectile = Instantiate(Explosion);
        newProjectile.transform.position = transform.position;
        newProjectile.transform.rotation = transform.rotation;
        var newProjectileRigid = newProjectile.GetComponent<Rigidbody2D>();
        newProjectileRigid.velocity = m_Rigidbody.velocity;
        newProjectile.transform.rotation = Quaternion.Euler(0, 0, Rotation);
        newProjectileRigid.AddForce(newProjectile.transform.up * Velocity);
        if (center)
        {
            newProjectile.name = "Explosion Center";
            GameObject.Find("Main Camera").GetComponent<CameraController>().SwitchToExplosion();
        }
    }
    void NewExplosion(float Rotation, float Velocity)
    {
        NewExplosion(Rotation, Velocity, false);
    }

    void NewExhaust(Vector3 offset, Vector2 direction, int size)
    {
        var newProjectile = Instantiate(Exhaust);
        newProjectile.transform.position = transform.position + offset;
        var newProjectileRigid = newProjectile.GetComponent<Rigidbody2D>();
        newProjectileRigid.velocity = m_Rigidbody.velocity;
        newProjectileRigid.AddForce(direction);
        newProjectile.GetComponent<ExhaustController>().timer = size;
        newProjectile.transform.localScale = new Vector3(size / 40.0f, size / 40.0f, 0);
    }
}
