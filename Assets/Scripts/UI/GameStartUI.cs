using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : MonoBehaviour
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnQuit;

    void Start()
    {
        btnStart.onClick.AddListener(() =>
        {
            btnStart.transform.DOShakePosition(1, 5);
        });
    }

}
