using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrobeProjectile : MonoBehaviour
{
    float count = 0f;
    public Vector3 startpoint;
    public Vector3 controlPoint;
    public Vector3 endPoint;
    public float damage;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (count < 1.0f)
        {
            count += Time.deltaTime * 0.8f;

            Vector3 m1 = Vector3.Lerp(startpoint, controlPoint, count);
            Vector3 m2 = Vector3.Lerp(controlPoint, endPoint, count);
            transform.position = Vector3.Lerp(m1, m2, count);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Mirror.PlayerStatController>().CmdTakeDamage(damage);
            Destroy(gameObject);
            
        }
        if (other.tag != "DetectionArea")
            Destroy(gameObject);
    }
}
