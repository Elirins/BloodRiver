using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
   
    public Slider slider;
    public Gradient gradient;
    public Image Fill;
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        Fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(float health)
    {
        slider.value = health;
        Fill.color = gradient.Evaluate(slider.normalizedValue);
    }
   
}
//¹ÒÔÚÑªÌõUIÉÏ

