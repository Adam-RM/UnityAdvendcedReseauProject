using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror {
    public class MicrobeStatController : StatController
    {
        [SerializeField] AudioSource DieSound;
        void Start()
        {
            SetStatValue();
        }

        void Update()
        {

        }

        protected override void Die()
        {
            AudioSource.PlayClipAtPoint(DieSound.clip, transform.position);
            base.Die();
        }

        public override void TakeDamage(float value)
        {
            base.TakeDamage(value);
            if (dying == false)
                GetComponent<EnemyController>().TakeDamage();
        }
    }
}
