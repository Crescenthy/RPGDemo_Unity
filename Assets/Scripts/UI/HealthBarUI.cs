using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    //属性
    public GameObject healthUIPrefab;
    private Transform healthUIbar;
    private Image healthSlider;
    public bool alwaysVisible;
    public float visibleTime;
    private float timeLife;

    //约束
    public Transform barPoint;
    private Transform cam;

    //数据
    CharacterStates currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStates>(); ;
        currentStats.UpdataHealthBarOnAttack += updataHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                healthUIbar = Instantiate(healthUIPrefab, canvas.transform).transform;

                healthSlider = healthUIbar.GetChild(0).GetComponent<Image>();

                healthUIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void updataHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(healthUIbar.gameObject);

        healthUIbar.gameObject.SetActive(true);
        timeLife = visibleTime;
        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    private void LateUpdate()
    {
        if (healthUIbar != null)
        {
            healthUIbar.position = barPoint.position;
            healthUIbar.forward = -cam.forward;

            if (timeLife <= 0 && !alwaysVisible)
                healthUIbar.gameObject.SetActive(false);
            else
                timeLife -= Time.deltaTime;
        }
    }
}
