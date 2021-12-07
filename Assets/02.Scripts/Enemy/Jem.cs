using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jem : MonoBehaviour
{
    public bool isRotate;
    public float flySpeed = 0.05f;

    private void OnEnable()
    {
       StartCoroutine(Fly());
    }
    IEnumerator Fly()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            float y = Mathf.Sin(Time.time * 5) * 0.05f;
            transform.position += new Vector3(0, y, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
