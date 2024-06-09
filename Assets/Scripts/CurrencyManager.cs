using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    public int currencyAmount;
    public Text currencyText; // UI Text to display currency

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // Keep the currency manager persistent across scenes
    }

    void Start()
    {
        UpdateCurrencyDisplay();
    }

    public void AddCurrency(int amount)
    {
        currencyAmount += amount;
        UpdateCurrencyDisplay();
    }

    public void SpendCurrency(int amount)
    {
        if (currencyAmount >= amount)
        {
            currencyAmount -= amount;
            UpdateCurrencyDisplay();
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }

    private void UpdateCurrencyDisplay()
    {
        if (currencyText != null)
            currencyText.text = "Currency: " + currencyAmount.ToString();
    }
}
