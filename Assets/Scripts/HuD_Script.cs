using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuD_Script : MonoBehaviour
{
    public static HuD_Script instance { get; set; }

    public Image maskHealth;
    public Image maskStamina;

    public Image healthBar;
    public Image staminaBar;

    float healthSize;
    float staminaSize;

    void Awake() {
        instance = this;
    }
    void Start() {
        healthSize = maskHealth.rectTransform.rect.width;
        staminaSize = maskStamina.rectTransform.rect.width;
    }
    public void HealthValue(float value) {
        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthSize * value);
    }
    public void StaminaValue(float value) {
        staminaBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, staminaSize * value);
    }
}
