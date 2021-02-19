using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePush : MonoBehaviour
{
    public float force = 50f;
    public float range = 50f;
    public Camera fpsCam;
    private RaycastHit target;
    private bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SelectTarget();
        }
        if (Input.GetKeyUp(KeyCode.F))
            Push();
    }

    private void SelectTarget()
    {
        isHit = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out target, range);

        if (isHit)
        {
            //Debug.Log("Selected: " + target.transform.name);
        }
    }

    private void Push()
    {
        if (isHit)
        {
            target.transform.GetComponent<Rigidbody>().AddForce(target.transform.up * force/2, ForceMode.Impulse);
            target.transform.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * force, ForceMode.VelocityChange);
        }
    }
}
