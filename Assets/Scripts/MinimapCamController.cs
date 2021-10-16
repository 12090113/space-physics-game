using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -20);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = target.position + offset;
    }
}
