using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    private TextMeshProUGUI levelText;
    private Image healthSlider;
    private Image expSlider;

    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider= transform.GetChild(1).GetChild(0).GetComponent<Image>();

    }

    private void Update()
    {
        levelText.text = "Level  " + GameManager.Instance.playerStats.characterDate.currentLevel.ToString("00");
        UpdataHealth();
        UpdataExp();
    }

    private void UpdataHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    private void UpdataExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.characterDate.currentExp / GameManager.Instance.playerStats.characterDate.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}
