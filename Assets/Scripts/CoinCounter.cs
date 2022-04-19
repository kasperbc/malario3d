using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    TMPro.TextMeshProUGUI textField;
    public int coinCount;
    public int displayCount;
    bool updatingDisplay;
    bool updateDisplayFast;
    public int tookDamageAt;
    
    void Start()
    {
        textField = GetComponent<TMPro.TextMeshProUGUI>();
        coinCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        textField.text = displayCount.ToString();
    }

    public void AddCoins(int amount, PlayerHealth collector)
    {
        coinCount += amount;

        if (!updatingDisplay)
            StartCoroutine(UpdateDisplay());

        if (tookDamageAt + 5 <= coinCount)
        {
            collector.RecoverHealth(1);
            tookDamageAt = coinCount;
        }
    }

    IEnumerator UpdateDisplay()
    {
        updatingDisplay = true;

        while (displayCount < coinCount)
        {
            displayCount++;

            if (!updateDisplayFast)
                yield return new WaitForSeconds(0.1f);
            else
                yield return new WaitForSeconds(0.05f);

            if (!updateDisplayFast && displayCount + 10 < coinCount)
            {
                updateDisplayFast = true;
            }
        }

        updateDisplayFast = false;
        updatingDisplay = false;
    }
}
