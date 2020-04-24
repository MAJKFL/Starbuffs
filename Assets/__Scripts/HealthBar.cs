using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public float setMaxHealth
    {
        get
        {
            return slider.maxValue;
        }
        set
        {
            slider.maxValue = value;
        }
    }

    public float changeValue
    {
        get
        {
            return slider.value;
        }
        set
        {
            slider.value = value;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}
