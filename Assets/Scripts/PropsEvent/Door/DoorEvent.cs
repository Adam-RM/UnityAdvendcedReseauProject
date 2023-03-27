using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvent : MonoBehaviour
{
    bool doorIsOpening = false;
    [SerializeField] float speed = 20f;
    [SerializeField] GameObject LeftDoor;
    [SerializeField] GameObject RightDoor;
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (doorIsOpening == false)
            return;
        LeftDoor.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        RightDoor.transform.Rotate(Vector3.back * Time.deltaTime * speed);
        Debug.Log(LeftDoor.transform.rotation.z * 100);
        Debug.Log(RightDoor.transform.rotation.z * 100);

        if (LeftDoor.transform.rotation.z * 100 >= 45 && RightDoor.transform.rotation.z * 100 <= -45)
            doorIsOpening = false;
    }

    
    public void OpenDoor()
    {
        doorIsOpening = true;
    }
}
