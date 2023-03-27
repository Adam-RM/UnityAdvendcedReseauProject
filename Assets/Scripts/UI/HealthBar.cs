using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Gradient color;
    [SerializeField] Slider healthBar;
    [SerializeField] Image img;
    float value = 0;
    void Start()
    {
        value = healthBar.value;
        img.color = color.Evaluate(healthBar.value);
    }

    // Update is called once per frame
    void Update()
    {
        if (value != healthBar.value)
        {
            img.color = color.Evaluate(healthBar.value);
            value = healthBar.value;
        }
    }
}
