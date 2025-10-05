using System.Collections;
using System.Collections.Generic;
using GJ;
using UnityEngine;
using UnityEngine.UI;

namespace GJ
{

    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] private Player Player;
        [SerializeField] private Image PlayerHealthUI;

        void FixedUpdate()
        {
            PlayerHealthUI.fillAmount = Player.GetHealthPer_Player();
        }
    }

}