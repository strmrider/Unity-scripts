using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    public float x = 0.0f;
    public float y = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
        SetMouseView();
    }

    void SetMouseView()
    {
        y += speedH * Input.GetAxis("Mouse X");
        x -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(x, y, 0.0f);
    }
}
