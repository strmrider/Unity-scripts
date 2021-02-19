using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{

    float timer = 0.0f;
    [SerializeField] float bobbingSpeed = 0.16f;
    [SerializeField] float bobbingAmount = 0.15f;
    [SerializeField] Transform PlayerController;
    float midpoint = 0.0f;

    float waveslice;
    float horizontal;
    float vertical;
    float translateChange;
    float totalAxes;

    bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        midpoint = transform.localPosition.y;
        PlayerController.GetComponent<PlayerMovement>().Headbobing += new PlayerMovement.SetBool(SetBobbingActivity);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
            Headbobing();
    }

    private void Headbobing()
    {
        waveslice = 0.0f;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            translateChange = waveslice * bobbingAmount;
            totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            Vector3 p = transform.localPosition;
            p.y = midpoint + translateChange;
            transform.localPosition = p;
        }
        else
        {
            Vector3 p = transform.localPosition;
            p.y = midpoint;
            transform.localPosition = p;
        }
    }

    public void SetBobbingActivity(bool activity)
    {
        isActive = activity;
    }
}
