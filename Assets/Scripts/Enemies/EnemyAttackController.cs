using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class EnemyAttackController : NetworkBehaviour
    {
        protected bool attackStatus = false;
        protected float currentCooldown = 0f;
        [SerializeField] protected float Cooldown = 5f;
        [SerializeField] protected float AttackRadius = 5f;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (attackStatus == true)
            {
                currentCooldown += Time.deltaTime;
            }
        }

        public virtual bool TryToAttack(Transform target)
        {
            if (attackStatus == false)
            {
                Attack(target);
                return true;
            }
            return false;
        }

        public virtual bool isAttacking()
        {
            return (attackStatus);
        }

        public virtual float GetAttackRadius()
        {
            return AttackRadius;
        }
        protected virtual void Attack(Transform target)
        {
            attackStatus = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }
    }
}
