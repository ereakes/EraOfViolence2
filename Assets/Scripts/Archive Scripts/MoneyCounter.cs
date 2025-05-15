using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    public TMP_Text moneyText;
    private int score = 0;

    void Start()
    {
        score = 20;
    }

    public void IncreaseMoneyMelee()
    {
        score += 20;
        moneyText.text = score.ToString();
    }

}
