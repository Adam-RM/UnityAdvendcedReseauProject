using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror {
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] NetworkAnimator networkAnimator;
        private bool attacking = false;
        [HideInInspector]public bool dying = false;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (attacking == true && animator.GetCurrentAnimatorStateInfo(0).IsName("BowAttack") == false)
            {
                StopAttacking();
            }
        }

        public void SetDirection(Vector3 direction)
        {
            networkAnimator.animator.SetFloat("xDirection", direction.x);
            networkAnimator.animator.SetFloat("yDirection", direction.z);
        }
        public void SetRunningAnim(bool status)
        {
            animator.SetBool("IsRunning", status);
            networkAnimator.animator.SetBool("IsRunning", status);
        }

        public void SetIdleAnim(bool status)
        {

        }
        public void SetAttackAnim()
        {
            animator.SetTrigger("Attack");
            networkAnimator.SetTrigger("Attack");
            attacking = true;
        }

        public void InterruptAttacking()
        {
            StopAttacking();
        }
        private void StopAttacking()
        {
            attacking = false;
        }

        public bool isAttacking()
        {
            return (attacking);
        }
        public void Jump()
        {
            networkAnimator.SetTrigger("Jump");
        }
        public bool isThrowArrow()
        {
            if(attacking == false)
                return false;
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.75f && animator.GetCurrentAnimatorStateInfo(0).IsName("BowAttack"))
                return true;
            return false;
        }

        public void GetHit()
        {
            animator.SetTrigger("GetHit");
            networkAnimator.SetTrigger("GetHit");
        }

        public bool isHit()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
                return true;
            return false;
        }

        public void Die()
        {
            networkAnimator.SetTrigger("Die");
            dying = true;

        }
        public bool isDead()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.95f && dying == true)
                return true;
            return false;
        }

        public void SetDash()
        {
            networkAnimator.SetTrigger("Dash");
        }

        public bool isDashing()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
                return true;
            return false;
        }
    }
}
