using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUiController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Image healthFillImage;

    private void Start()
    {
        GameController.instance.UpdateCountdownTimerDel += UpdateCountdownTimer;
    }
    private void UpdateCountdownTimer(float time)
    {
        countdownText.text = time.ToString("F0");
    }

    private void UpdateHealthBar(float amount)
    {
        healthFillImage.fillAmount = amount;
    }
    
    
}