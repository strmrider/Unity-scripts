using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(_ShakeCamera(duration, magnitude));
    }

    private IEnumerator _ShakeCamera(float duration, float magnitude)
    {
        /* Vector3 pos = transform.localRotation;
         float elapsedTime = 0f;
         float xShake = Random.Range(-1f, 1f) * magnitude;
         float yShake = Random.Range(-1f, 1f) * magnitude;

         while (elapsedTime < duration)
         {
             transform.localPosition = new Vector3(xShake, yShake, pos.z);
             elapsedTime += Time.deltaTime;
             yield return null;
         }

         transform.localPosition = pos;
     }

        Quaternion originalRotation = transform.localRotation;
        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xShake = Random.Range(-1f, 1f) * magnitude;
            float yShake = Random.Range(-1f, 1f) * magnitude;
            Vector3 v = new Vector3(transform.eulerAngles.x - xShake,
            transform.eulerAngles.y, transform.eulerAngles.z);
            transform.eulerAngles = v;
            transform.Translate(0, 0, -0.01f * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.3f);
            //yield return null;
            /*v = new Vector3(transform.eulerAngles.x + 1f,
                transform.eulerAngles.y + 1f, transform.eulerAngles.z);
            transform.eulerAngles = v;

        }

        transform.localRotation = originalRotation;
        transform.localPosition = originalPosition;
        */
        animator.SetBool("Shake", true);
        yield return null;
        animator.SetBool("Shake", false);
    }

    public void EnableAnimation(bool status)
    {
        animator.enabled = status;
    }
}
