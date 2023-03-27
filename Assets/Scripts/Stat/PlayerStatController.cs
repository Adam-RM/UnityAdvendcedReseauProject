using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Mirror
{
    [SerializeField]
    public class PlayerStatController : StatController
    {
        // Start is called before the first frame update
        //[SerializeField] TextMeshProUGUI nameText = null;
        void Start()
        {
            if (isOwned)
                healthBar = GameObject.FindGameObjectWithTag("HUD").GetComponentInChildren<Slider>();
            else
            {
                foreach (Transform t in transform)
                {
                    if (t.tag == "PlayerUI")
                        t.gameObject.SetActive(true);
                }
            }
            SetStatValue();

        }
        void LateUpdate()
        {
            //if (nameText.isActiveAndEnabled == true && nameText.text == "")
            //{
            //    nameText.text = GetComponent<PlayerNetwork>().myName;
            //}
        }
        //protected override void SetStatValue()
        //{
        //    base.SetStatValue();
        //}
        public override void TakeDamage(float value)
        {
            base.TakeDamage(value);

        }

        protected override void Die()
        {
            base.Die();
        }

    }
}
