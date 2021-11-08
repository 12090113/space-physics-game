using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;
    public GameObject Exhaust;
    public ThrusterController mainThruster;
    public ThrusterController rightThruster;
    public ThrusterController leftThruster;
    public Transform ship;
    private const float m_Speed = 100.0f;
    private const float rotationSpeed = 0.5f;
    public bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (started)
        {
            var shipDistance = UnsquaredDistance(ship);
            var lookPos = ship.position - transform.position;
            var rotation = ship.rotation;//Quaternion.LookRotation(lookPos);
            float angle = Mathf.Atan2(ship.position.y - transform.position.y, ship.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            var angleDifference = targetRotation.eulerAngles.z - transform.eulerAngles.z;
            var right = transform.eulerAngles.z > targetRotation.eulerAngles.z;
            if (transform.eulerAngles.z - 6 <= targetRotation.eulerAngles.z && targetRotation.eulerAngles.z <= transform.eulerAngles.z + 6)
            {
                m_Rigidbody.angularVelocity /= 2;
            }
            else if (right)
            {
                //m_Rigidbody.AddTorque(-3);
                //NewExhaust(transform.right * -3f + transform.up * 6f, transform.right * -1f, 30);
                rightThruster.Thrust();
            }
            else if (!right)
            {
                //m_Rigidbody.AddTorque(3);
                //NewExhaust(transform.right * 3f + transform.up * 5.5f, transform.right * 1f, 30);
                leftThruster.Thrust();
            }
            if (shipDistance > 600 && Math.Abs(angleDifference) < 90)
            {
                //m_Rigidbody.AddForce(transform.up * m_Speed);
                //NewExhaust(transform.up * -9f, transform.up * -1f, 80);
                mainThruster.Thrust();
            }
        } else
        {
            NewExhaust(transform.up * -9f, transform.up * -1f, 80);
        }
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

    double UnsquaredDistance(Transform target)
    {
        return Math.Pow((int)target.position.x - (int)transform.position.x, 2) + Math.Pow((int)target.position.y - (int)transform.position.y, 2);
    }
}
