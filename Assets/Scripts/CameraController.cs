using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform ship;
    private Transform target;
    private float speed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);
    private Camera m_Camera;
    private float scroll;
    private float oldScroll = 12;
    private bool explosion = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
        scroll = m_Camera.orthographicSize;
        target = ship;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (explosion)
        {
            scroll += 0.1f;
        }
        transform.position = Vector3.Lerp (transform.position, target.position + offset, speed);
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            scroll += -m_Camera.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
            scroll = Mathf.Clamp(scroll, 1, 10000);
        }
        m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, scroll, 0.1f);
        if (offset.z == -10 && m_Camera.orthographicSize > 70)
        {
            offset.z = -20;
        } else if (offset.z == -20 && m_Camera.orthographicSize <= 70)
        {
            offset.z = -10;
        }
    }

    public void SwitchToExplosion()
    {
        explosion = true;
        oldScroll = scroll;
        target = GameObject.Find("Explosion Center").transform;
    }
    public void SwitchToPlayer()
    {
        scroll = oldScroll;
        explosion = false;
        target = player;
    }
}
