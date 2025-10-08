using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public TextMeshProUGUI tmpCount;
    public Image image;

    public void UpdateInfo(Sprite sprite, int count)
    {
        image.sprite = sprite;
        tmpCount.text = count.ToString();
    }
}
