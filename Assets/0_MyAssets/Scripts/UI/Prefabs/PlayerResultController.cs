using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResultController : MonoBehaviour
{
    [SerializeField] Text rankText;
    [SerializeField] Text nameText;
    [SerializeField] Text eatenCountText;
    [SerializeField] Image rankBG;
    [SerializeField] Image infoBG;
    [SerializeField] Color myColor;
    string[] ordinals = new string[] { "st", "nd", "rd" };
    RectTransform rectTransform;
    Color defaultColor;
    public void OnStart(float posY)
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, posY);
        defaultColor = rankBG.color;
    }

    public void ShowParam(int rank, string name, int eatenCount, int playerIndex)
    {
        string ordinal = (rank > ordinals.Length) ? "th" : ordinals[rank - 1];
        rankText.text = rank + ordinal;
        nameText.text = name;
        eatenCountText.text = "★ " + eatenCount;
        if (playerIndex == 0)
        {
            rankBG.color = myColor;
            infoBG.color = myColor;
            GoogleAnalyticsManager.i.LogEvent("スコア_" + eatenCount);
        }
        else
        {
            rankBG.color = defaultColor;
            infoBG.color = defaultColor;
        }

    }
}
