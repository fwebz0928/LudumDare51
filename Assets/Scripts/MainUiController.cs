using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUiController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Slider healthFillImage;

    private void Start()
    {
        GameController.instance.UpdateCountdownTimerDel += UpdateCountdownTimer;

        // healthFillImage.maxValue = PlayerController.Instance.maxhealth;
        healthFillImage.value = PlayerController.Instance.GetHealthAmount();
        PlayerController.Instance.UpdateHealthBarDel += UpdateHealthBar;
    }
    private void UpdateCountdownTimer(float time)
    {
        countdownText.text = time.ToString("F0");
    }

    private void UpdateHealthBar(float amount)
    {
        healthFillImage.value = amount;
    }
}