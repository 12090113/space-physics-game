using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDC : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private Transform ship;
    private Transform missile;
    public Transform target;
    private HingeJoint2D hinge;
    private const float rotationSpeed = 0.5f;
    public GameObject Projectile;
    public float bulletVelocity = 1000.0f;
    public float barrelOffset = 1f;
    public int fireSpeed = 20;
    private int fireTimer;
    private int health = 100;
    double missileDistance = 0;
    //private SpriteRenderer Renderer;
    public bool inverted = true;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        hinge = GetComponent<HingeJoint2D>();
        ship = GameObject.Find("FriendlyShip").transform;
        missile = GameObject.Find("Player").transform;
        fireTimer = UnityEngine.Random.Range(0, fireSpeed);
        //Renderer = GetComponent<SpriteRenderer>();
        if (hinge.jointAngle < hinge.limits.min && hinge.jointAngle > hinge.limits.max)
        {
            inverted = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        missileDistance = UnsquaredDistance(missile);
        if (missileDistance <= UnsquaredDistance(ship) || missileDistance < 600)
        {
            target = missile;
        } else
        {
            target = ship;
        }
        var lookPos = target.position - transform.position;
        var rotation = target.rotation;
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        var angleDifference = targetRotation.eulerAngles.z - transform.eulerAngles.z;
        //var hingeTarget = hinge.jointAngle + angleDifference;
        //Debug.Log("hinge.jointAngle = " + hinge.jointAngle + " but hingeTarget = " + hingeTarget);
        //Debug.Log("hingeTarget = " + hinge.jointAngle + " + " + angleDifference);
        //Debug.Log("!(" + (hingeTarget < hinge.limits.min) + " ^ " + (hingeTarget > hinge.limits.max) + ") ^ " + inverted + " == (" + !(hingeTarget < hinge.limits.min ^ hingeTarget > hinge.limits.max) + ") ^ " + inverted + " == " + (!(hingeTarget < hinge.limits.min ^ hingeTarget > hinge.limits.max) ^ inverted));
        /*if (!(hingeTarget < hinge.limits.min ^ hingeTarget > hinge.limits.max) ^ inverted)
        {
            //Debug.Log("true == " + hinge.jointAngle + " < " + hinge.limits.min + " && " + hinge.jointAngle + " > " + hinge.limits.max);
            Renderer.material.color = new Color(255, 0, 0, 1.0f);
        }
        else
        {
            //Debug.Log("false == " + hinge.jointAngle + " < " + hinge.limits.min + " && " + hinge.jointAngle + " > " + hinge.limits.max);
            Renderer.material.color = new Color(0, 0, 0, 1.0f);
        }*/
        var right = transform.eulerAngles.z > targetRotation.eulerAngles.z;
        if (transform.eulerAngles.z - 3 <= targetRotation.eulerAngles.z && targetRotation.eulerAngles.z <= transform.eulerAngles.z + 3)
        {
            m_Rigidbody.angularVelocity /= 2;
        }
        else if (right)
        {
            m_Rigidbody.angularVelocity = Mathf.Lerp(transform.eulerAngles.z, targetRotation.eulerAngles.z, Math.Abs(angleDifference / 180)) * -1 + m_Rigidbody.angularVelocity / 2;
        }
        else if (!right)
        {
            m_Rigidbody.angularVelocity = Mathf.Lerp(transform.eulerAngles.z, targetRotation.eulerAngles.z, Math.Abs(angleDifference / 180)) + m_Rigidbody.angularVelocity / 2;
        }
        if (fireTimer >= fireSpeed && Math.Abs(angleDifference) < 90)
        {
            var newProjectile = Instantiate(Projectile);
            newProjectile.transform.position = transform.position + transform.up * barrelOffset;
            newProjectile.transform.rotation = transform.rotation;
            var newProjectileRigid = newProjectile.GetComponent<Rigidbody2D>();
            newProjectileRigid.velocity = m_Rigidbody.velocity;
            newProjectileRigid.AddForce(transform.up * bulletVelocity);
            m_Rigidbody.AddForce(transform.up * -bulletVelocity/50);
            fireTimer = 0;
        }
        if (fireTimer < fireSpeed)
        {
            fireTimer++;
        }
        if (health <= 0)
        {
            Die();
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        health -= (int) col.relativeVelocity.magnitude / 8;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        health -= 50;
    }
    private void Die()
    {
        Destroy(hinge);
        GameObject.Find("Text").GetComponent<TextController>().PDCdestroyed();
        Destroy(this);
    }

    double UnsquaredDistance(Transform target)
    {
        return Math.Pow((int) target.position.x - (int) transform.position.x, 2) + Math.Pow((int) target.position.y - (int) transform.position.y, 2);
    }
}
