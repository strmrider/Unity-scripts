using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public GameObject scopeImage;
    public Camera scopeCamera;

    [SerializeField] float[] zoomStates = { 15f, 5f, 1f };
    private int state = 0;
    private float smooth = 5f;
    private float normal;
    private float zoom = 0f;
    private bool scopeOn = false;
    private float wheelInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (scopeOn)
        {
            wheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (wheelInput != 0f)
            {
                // zoom in
                if (wheelInput > 0f)
                {
                    if (state + 1 <= zoomStates.Length - 1)
                        state++;
                }
                // zoom out
                else if (wheelInput < 0f)
                {
                    if (state - 1 >= 0)
                        state--;
                }

                zoom = zoomStates[state];
                Zoom();
            }

        }
    }

    public void DisplayScope(bool active)
    {
        scopeOn = active;
        scopeImage.SetActive(active);
        if (active)
        {
            state = 0;
            normal = scopeCamera.fieldOfView;
            zoom = zoomStates[state];
            Zoom();
        }
        else
        {
            scopeCamera.fieldOfView = normal;
        }

    }


    public void Zoom()
    {
        //scopeCamera.fieldOfView = Mathf.Lerp(scopeCamera.fieldOfView, zoom, Time.deltaTime * smooth);
        scopeCamera.fieldOfView = zoom;
    }

    public bool ScopeOn
    {
        get { return scopeOn; }
    }
}
