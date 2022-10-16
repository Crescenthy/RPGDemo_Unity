using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestRequirement : MonoBehaviour
{
    private TextMeshProUGUI requireName;
    private TextMeshProUGUI progressNumber;

    private void Awake()
    {
        requireName = GetComponent<TextMeshProUGUI>();
        progressNumber = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetupRequirement(string name, int amount, int currentAmount)
    {
        requireName.text = name;
        progressNumber.text = currentAmount.ToString() + " / " + amount.ToString();
    }

    public void SetupRequirement(string name, bool isFinished)
    {
        if (isFinished)
        {
            requireName.text = name;
            progressNumber.text = "Íê³É";
            requireName.color = Color.gray;
            progressNumber.color = Color.gray;
        }
    }

}
