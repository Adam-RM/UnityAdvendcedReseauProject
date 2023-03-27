using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] NetworkAnimator networkAnimator;

        private bool attacking = false;
        private bool dying = false;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (attacking == true && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
            {
                StopAttacking();
            }
        }
        public void SetRunningAnim(bool status)
        {
            //animator.SetBool("IsRunning", status);
            networkAnimator.animator.SetBool("IsRunning", status);
        }
        public void SetAttackAnim()
        {
            //animator.SetTrigger("Attack");
            networkAnimator.SetTrigger("Attack");
            attacking = true;
        }
        private void StopAttacking()
        {
            attacking = false;
        }

        public bool isAttacking()
        {
            return (attacking);
        }

        public void GetHit()
        {
            networkAnimator.SetTrigger("GetHit");
        }

        public bool IsHit()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit");
        }

        public void Die()
        {
            animator.SetTrigger("Die");
            networkAnimator.SetTrigger("Die");
            dying = true;

        }
        public bool isDead()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.95f && dying == true)
                return true;
            return false;
        }
    }
}
