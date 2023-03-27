using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror {
    public class SlimeAttackController : EnemyAttackController
    {
        [SerializeField] ParticleSystem fireSpell;
        [SerializeField] float damage = 20f;
        [SerializeField] AudioSource AttackSound;
        private List<Transform> targetList = new List<Transform>();
        void Start()
        {

        }
        void Update()
        {

                if (attackStatus == true)
                    currentCooldown += Time.deltaTime;
                if (fireSpell.gameObject.activeSelf == false && attackStatus == true && currentCooldown >= Cooldown)
                {
                    currentCooldown = 0;
                    attackStatus = false;
                    targetList.Clear();
                }
                else if (fireSpell.gameObject.activeSelf)
                    DetectEnemies();

        }

        private void DetectEnemies()
        {
            List<Transform> newEnemies = new List<Transform>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, AttackRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    bool isNew = true;
                    foreach (Transform target in targetList)
                    {
                        if (collider.transform == target)
                            isNew = false;
                    }
                    if (isNew == true)
                    {
                        targetList.Add(collider.transform);
                        if (isServer)
                            collider.transform.GetComponent<PlayerStatController>().CmdTakeDamage(damage);
                    }
                }
            }
        }

        [ClientRpc]
        void RpcShowAttackPS()
        {
            fireSpell.gameObject.SetActive(true);
            fireSpell.Play();
            AttackSound.Play();
        }

        protected override void Attack(Transform target)
        {
            base.Attack(target);
            targetList.Add(target);
            RpcShowAttackPS();
            
            target.GetComponent<PlayerStatController>().CmdTakeDamage(damage);
        }
    }
}
