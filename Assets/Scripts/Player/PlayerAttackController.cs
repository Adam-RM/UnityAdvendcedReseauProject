using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class PlayerAttackController : NetworkBehaviour
    {
        [SerializeField] Transform Launcher;
        [SerializeField] GameObject Arrow;
        [SerializeField] float cooldown = 1;
        [SerializeField] float range = 100f;
        RaycastHit hit;
        float currentCooldown = 0;

        // Update is called once per frame
        void Update()
        {
            currentCooldown += Time.deltaTime;
        }
        [Command]
        void CmdShoot(Vector3 hitPoint, Vector3 pos, Vector3 rot)
        {
            GameObject arrow = Instantiate(Arrow, pos, Quaternion.Euler(rot));
            arrow.GetComponent<Arrow>().SetTarget(hitPoint);
            NetworkServer.Spawn(arrow);

        }

        public void Shoot()
        {
            if (currentCooldown > cooldown)
            {
                Vector2 ScreenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                Ray ray = Camera.main.ScreenPointToRay(ScreenCenter);
                if (Physics.Raycast(ray, out hit, range))
                {
                    CmdShoot(hit.point, Launcher.position, Launcher.rotation.eulerAngles);
                }
                currentCooldown = 0;
            }
        }
    }

    /*
             [Command]
        void CmdShoot()
        {
            GameObject arrow = Instantiate(Arrow, Launcher.position, Launcher.rotation);
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.VelocityChange);
            NetworkServer.Spawn(arrow);
        }

        public void Shoot()
        {
            if (currentCooldown > cooldown)
            {
                CmdShoot();
                currentCooldown = 0;
            }
        }*/
}