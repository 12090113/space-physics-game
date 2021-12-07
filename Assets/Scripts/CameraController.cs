using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform ship;
    private Transform target;
    private Transform background;
    private float speed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);
    private Camera m_Camera;
    private float scroll;
    private float touchPos = -2;
    private float touchID = -1;
    private bool explosion = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
        scroll = m_Camera.orthographicSize;
        target = ship;
        background = transform.GetChild(0);
    }
    void FixedUpdate()
    {
        if (explosion)
        {
            scroll += 0.1f;
        }
        transform.position = Vector3.Lerp (transform.position, target.position + offset, speed);
        for (int i = Input.touchCount - 1; i >= 0; i--)
        {
            if (Input.touches[i].rawPosition.y/Screen.height > 0.75)
            {
                Touch touch = Input.touches[i];
                if (touch.fingerId == touchID && touch.phase == TouchPhase.Moved) {
                    Debug.Log(touchID);
                    scroll += -m_Camera.orthographicSize * (10 * (touch.position.x - touchPos) / Screen.width);
                    scroll = Mathf.Clamp(scroll, 1, 10000);
                }
                touchPos = touch.position.x;
                touchID = touch.fingerId;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            scroll += -m_Camera.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
            scroll = Mathf.Clamp(scroll, 1, 10000);
        }
        m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, scroll, 0.1f);
        background.localScale = new Vector3(m_Camera.orthographicSize*0.08f, m_Camera.orthographicSize*0.08f, 0);
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
        target = GameObject.Find("Explosion Center").transform;
    }
    public void SwitchToPlayer()
    {
        if (scroll > 11)
            scroll -= 10;
        explosion = false;
        target = player;
        player.GetComponent<PlayerController>().Detatch();
    }
}
