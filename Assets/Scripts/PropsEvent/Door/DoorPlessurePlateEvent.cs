using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class DoorPlessurePlateEvent : NetworkBehaviour
    {
        [SerializeField] DoorEvent myDoor;
        [SerializeField] ParticleSystem Diamound;
        [SerializeField] ParticleSystem Star;
        [SerializeField] AudioSource Success;

        bool isUsed = false;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        [ClientRpc]
        void SetParticulesSystem()
        {
            Diamound.Stop();
            Star.gameObject.SetActive(true);
            Success.Play();
            myDoor.OpenDoor();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (isServer)
            {
                if (other.tag == "Player")
                {
                    if (isUsed == false)
                    {
                        isUsed = true;
                        myDoor.OpenDoor();
                        SetParticulesSystem();
                    }

                }
            }
        }
    }
}