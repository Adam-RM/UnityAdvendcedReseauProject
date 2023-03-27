using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    public class StatController : NetworkBehaviour
    {
        [SerializeField] protected Slider healthBar;
        [SerializeField] protected float healthPoint;
        protected float currentHealthPoint;
        [HideInInspector] public bool dying = false;
        void Start()
        {
            SetStatValue();
        }

        // Update is called once per frame
        void Update()
        {
        }

        protected virtual void SetStatValue()
        {
            currentHealthPoint = healthPoint;
            healthBar.value = currentHealthPoint;
        }
        [ClientRpc]
        public virtual void TakeDamage(float value)
        {
            currentHealthPoint = currentHealthPoint - value;

            if (currentHealthPoint <= 0)
            {
                currentHealthPoint = 0;
                Die();
            }
            healthBar.value = currentHealthPoint / healthPoint;
            if (dying == false && GetComponent<PlayerController>() != null)
                GetComponent<PlayerController>().TakeDamage();
        }
        
        public virtual void CmdTakeDamage(float value)
        {
            if (isServer)
                TakeDamage(value);
        }
        public virtual float GetHealth()
        {
            return (healthPoint);
        }
        [ClientRpc]
        protected virtual void Heal(float value, NetworkIdentity target)
        {
            StatController stat = target.GetComponent<StatController>();
            stat.currentHealthPoint += value;
            if (stat.currentHealthPoint > stat.healthPoint)
                stat.currentHealthPoint = stat.healthPoint;
            stat.healthBar.value = stat.currentHealthPoint / stat.healthPoint;
        }

        [Command]
        public virtual void CmdHeal(float value, NetworkIdentity target)
        {
            Heal(value, target);
        }

        protected virtual void Die()
        {
            dying = true;
        }

        public virtual bool IsDying()
        {
            return dying;
        }
    }
}