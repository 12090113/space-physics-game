using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MaxRotate = 400.0f;
    private const float rotationSpeed = 1.0f;
    private const float m_Speed = 10.0f;
    private int detonateTimer = 0;
    private bool up;
    private bool left;
    private bool right;
    private bool input = true;
    private bool makeJoint;
    Rigidbody2D m_Rigidbody;
    public GameObject Launcher;
    public GameObject Explosion;
    public GameObject Exhaust;
    public FixedJoint2D joint;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        joint = gameObject.GetComponent<FixedJoint2D>();
        Destroy(joint);
    }

    void FixedUpdate()
    {
        if (input)
        {
            if (Input.GetKey(KeyCode.W) || up)
            {
                m_Rigidbody.AddForce(transform.up * m_Speed);
                NewExhaust(transform.up * -0.4f, transform.up * -1f, 20);
            }
            if (Input.GetKey(KeyCode.D) || right && m_Rigidbody.angularVelocity > -MaxRotate)
            {
                m_Rigidbody.AddTorque(-rotationSpeed);
                NewExhaust(transform.right * -0.1f + transform.up * 0.4f, transform.right * -1f, 10);
            }
            else if (Input.GetKey(KeyCode.A) || left && m_Rigidbody.angularVelocity < MaxRotate)
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

            if (detonateTimer > 0 && detonateTimer < 100)
                detonateTimer--;
            if (Input.GetKey(KeyCode.Space))
            {
                Detonate();
            }
        }
    }
    void Update()
    {
        if (input)
        {
            up = left = right = false;
            for (int i = Input.touchCount - 1; i >= 0; i--)
            {
                Vector2 tap = new Vector2(Input.touches[i].position.x / Screen.width, Input.touches[i].position.y / Screen.height);
                if (tap.y > 0.75 && Input.touches[i].phase == TouchPhase.Began)
                {
                    if (detonateTimer > 0 && detonateTimer < 100)
                        Detonate();
                    else
                        detonateTimer = 2;
                }
                else if (tap.x < 0.5 && tap.y < 0.75)
                {
                    float direction = Input.touches[i].position.x - Input.touches[i].rawPosition.x;
                    if (direction > 10)
                        right = true;
                    else if (direction < -10)
                        left = true;
                }
                else if (tap.x > 0.5 && tap.y < 0.75)
                {
                    up = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Detonate();
            }
        } else if (makeJoint) {
            makeJoint = false;
            joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = Launcher.GetComponent<Rigidbody2D>();
        }
    }

    public void Detatch()
    {
        Destroy(joint);
        input = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ((int)col.relativeVelocity.magnitude / 4 > 4 && input)
        {
            //Debug.Log((int)col.relativeVelocity.magnitude / 4);
            m_Rigidbody.AddForce(col.relativeVelocity);
            Detonate();
        }
    }

    void Detonate()
    {
        detonateTimer = 1000;
        for (int i = 0; i < 30; i++)
        {
            NewExplosion(12 * i, 100 + UnityEngine.Random.Range(-90.0f, 100.0f));
        }
        NewExplosion(0, 0, true);
        transform.position = Launcher.transform.position + Launcher.transform.up * 6.33f;
        m_Rigidbody.velocity = Launcher.GetComponent<Rigidbody2D>().velocity;
        m_Rigidbody.angularVelocity = 0;
        makeJoint = true;
        transform.rotation = Launcher.transform.rotation;
        input = false;
    }
    void NewExplosion(float Rotation, float Velocity, bool center)
    {
        GameObject newProjectile = Instantiate(Explosion);
        newProjectile.transform.position = transform.position;
        Rigidbody2D newProjectileRigid = newProjectile.GetComponent<Rigidbody2D>();
        newProjectileRigid.velocity = m_Rigidbody.velocity;
        newProjectile.transform.rotation = Quaternion.Euler(0, 0, Rotation);
        newProjectileRigid.AddForce(newProjectile.transform.up * Velocity);
        if (center)
        {
            newProjectile.name = "Explosion Center";
            Camera.main.GetComponent<CameraController>().SwitchToExplosion();
        }
    }
    void NewExplosion(float Rotation, float Velocity)
    {
        NewExplosion(Rotation, Velocity, false);
    }

    void NewExhaust(Vector3 offset, Vector2 direction, int size)
    {
        GameObject newProjectile = Instantiate(Exhaust);
        newProjectile.transform.position = transform.position + offset;
        Rigidbody2D newProjectileRigid = newProjectile.GetComponent<Rigidbody2D>();
        newProjectileRigid.velocity = m_Rigidbody.velocity;
        newProjectileRigid.AddForce(direction);
        newProjectile.GetComponent<ExhaustController>().timer = size;
        newProjectile.transform.localScale = new Vector3(size / 40.0f, size / 40.0f, 0);
    }
}
