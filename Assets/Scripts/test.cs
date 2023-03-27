using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    float count = 0f;
    Vector3 startpoint;
    [SerializeField] Transform controlPoint;
    [SerializeField] Transform endPoint;
    void Start()
    {
        startpoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (count < 1.0f)
        {
            count += Time.deltaTime * 0.2f;

            Vector3 m1 = Vector3.Lerp(startpoint, controlPoint.position, count);
            Vector3 m2 = Vector3.Lerp(controlPoint.position, endPoint.position, count);
            transform.position = Vector3.Lerp(m1, m2, count);
        }
    }
}
