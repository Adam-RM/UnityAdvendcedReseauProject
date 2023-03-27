using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class Arrow : NetworkBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] float Speed;
        [SerializeField] Transform HitPoint;
        [SerializeField] float TimeAlive;
        [SerializeField] AudioSource HitSound;
        private float currentTimeAlive = 0f;
        Vector3 target = Vector3.zero;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

                float step = Speed * Time.deltaTime;
                currentTimeAlive += Time.deltaTime;
                if (currentTimeAlive >= TimeAlive)
                {
                    NetworkTransform.Destroy(gameObject);

                    GameObject.Destroy(transform.gameObject);
                }
                if (target != Vector3.zero)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, step);
                }
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }*/
        public void SetTarget(Vector3 currentTarget)
        {
            target = currentTarget;
        }
        private void OnCollisionEnter(Collision collision)
        {

                if (collision.gameObject.layer != LayerMask.NameToLayer("Player") && collision.gameObject.layer != LayerMask.NameToLayer("DetectionArea"))
                {
                    Rigidbody rigidbody = GetComponent<Rigidbody>();
                    rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    rigidbody.isKinematic = true;
                    target = Vector3.zero;
                    HitPoint.position = collision.contacts[0].point;
                    if (collision.gameObject.tag == "Enemy")
                    {
                        collision.gameObject.GetComponent<Mirror.StatController>().CmdTakeDamage(50);
                        AudioSource.PlayClipAtPoint(HitSound.clip, transform.position);
                        NetworkTransform.Destroy(gameObject);
                        GameObject.Destroy(transform.gameObject);

                    }
                }
        }
    }
}
