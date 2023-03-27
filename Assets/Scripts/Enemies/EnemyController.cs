using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mirror {
    public class EnemyController : NetworkBehaviour
    {
        [SerializeField] EnemyAnimatorController enemyAnimatorController;
        [SerializeField] EnemyAttackController enemyAttackController;
        [SerializeField] StatController statController;

        [SerializeField] List<Transform> pathPoint = new List<Transform>();
        [SerializeField] float detectionRadius;
        NavMeshAgent nav;
        int pathIndex = 0;
        bool targetFind = false;
        GameObject target = null;
        private bool isMovable = true;
        private bool isDying = false;
        private bool isHit = false;
        void Start()
        {
            if (isServer)
            {
                nav = GetComponent<NavMeshAgent>();
            }
        }

        void Update()
        {
            
            if (isServer)
            {
                if (statController.IsDying() == true)
                    Die();
                if (enemyAnimatorController.IsHit() == false && isHit)
                {
                    isHit = false;
                    nav.isStopped = false;
                }
                else if (isHit)
                    return;
                if (isMovable && isDying == false)
                    HandleMove();
                if (isDying == false)
                HandleAttack();
                if (nav.velocity != Vector3.zero)
                    enemyAnimatorController.SetRunningAnim(true);
            }
        }

        public void TakeDamage()
        {
            enemyAnimatorController.GetHit();
            isHit = true;
            nav.isStopped = true;
        }
        private void Die()
        {
            if (isDying == false)
            {
                enemyAnimatorController.Die();
                isDying = true;
                nav.isStopped = true;
            }
            else if (enemyAnimatorController.isDead() == true)
            {
                NetworkServer.Destroy(gameObject);
                Destroy(transform.gameObject);
            }
        }

        private void HandleAttack()
        {
            if (enemyAttackController.isAttacking())
                return;
            DetectEnemies();
            if (isMovable == false && enemyAttackController.isAttacking() == false)
            {
                isMovable = true;
                nav.isStopped = false;
            }
        }

        private void DetectEnemies()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyAttackController.GetAttackRadius());
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    foreach (Transform t in collider.transform)
                    {
                        if (t.tag == "Body" && t.gameObject.activeSelf)
                        {
                            if (enemyAttackController.TryToAttack(collider.transform))
                            {
                                enemyAnimatorController.SetAttackAnim();
                                isMovable = false;
                                nav.isStopped = true;
                            }
                        }
                    }

                    
                }
            }
        }
        private void HandleMove()
        {
            if (targetFind == false)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Player"))
                    {
                        foreach (Transform t in collider.transform)
                        {
                            if (t.tag == "Body" && t.gameObject.activeSelf)
                            {
                                targetFind = true;
                                target = collider.gameObject;
                                nav.SetDestination(collider.transform.position);
                            }
                        }
                    }
                }
                if (nav.remainingDistance < 1)
                {
                    pathIndex++;
                    if (pathIndex >= pathPoint.Count)
                        pathIndex = 0;
                    nav.SetDestination(pathPoint[pathIndex].position);
                }
                else
                    nav.SetDestination(pathPoint[pathIndex].position);
            }
            if (targetFind == true)
            {
                nav.SetDestination(target.transform.position);
                foreach (Transform t in target.transform)
                {
                    if (t.tag == "Body" && t.gameObject.activeSelf == false)
                    {
                        target = null;
                        targetFind = false;
                    }
                }
            }
        }
    }
}
