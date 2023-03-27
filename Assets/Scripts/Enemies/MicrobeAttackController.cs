using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class MicrobeAttackController : EnemyAttackController
    {
        [SerializeField] float damage = 20f;
        [SerializeField] GameObject Projectile;
        [SerializeField] Transform Launcher;
        [SerializeField] Transform ControlPoint;
        [SerializeField] AudioSource AttackSound;

        void Start()
        {

        }
        void Update()
        {

            if (attackStatus == true)
                currentCooldown += Time.deltaTime;
            if (attackStatus == true && currentCooldown >= Cooldown)
            {
                currentCooldown = 0;
                attackStatus = false;
            }

        }
        [ClientRpc]
        void CreateProjetile(Transform target)
        {
            AttackSound.Play();
            GameObject projectile = Instantiate(Projectile, Launcher.position, Quaternion.identity);
            MicrobeProjectile pro = projectile.GetComponent<MicrobeProjectile>();
            pro.startpoint = Launcher.position;
            pro.controlPoint = ControlPoint.position;
            pro.endPoint = target.position;
            pro.damage = damage;
        }
        protected override void Attack(Transform target)
        {
            base.Attack(target);
            CreateProjetile(target);
        }
    }
}