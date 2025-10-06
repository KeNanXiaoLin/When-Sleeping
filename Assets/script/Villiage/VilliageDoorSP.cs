using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GJ
{

    public class VilliageDoorSP : VilliageDoor
    {
        public LA_Item _AimItem;
        public GameObject ConformToBattleScene;
        public Button ConformToBattleScene_Button;
        public Button ReventPlayer_Button;




        void Awake()
        {
            ConformToBattleScene_Button.onClick.AddListener(ConformDrink);
            ReventPlayer_Button.onClick.AddListener(UnDrink);
        }

        void Start()
        {
            if (SceneLoadManager.Instance.DayIndex == 0)
            {
                AimScene = "BattleScene";
            }
            else
            {
                AimScene = "BattleScene 1";
            }
        }

        void Update()
        {
            
        }

        public override void CheckIfSceneChangeChange()
        {
            foreach (Collider2D i in colliders)
            {
                if (i.GetComponent<VilliagePlayer>() != null)
                {
                    if (LA_Backpack.Instance.CheckItemNum_Backpack(_AimItem, 1) == true)
                    {
                        ConformToBattleScene.SetActive(true);
                        
                    }
                }
            }
        }

        private void ConformDrink()
        {
            SceneLoadManager.Instance.MilkDrinked = true;
            LA_Backpack.Instance.DeceleItem_Backpack(_AimItem, 1);
            EventListener.BattleSceneChange(AimScene);
        }

        private void UnDrink()
        {
            SceneLoadManager.Instance.MilkDrinked = false;
            LA_Backpack.Instance.DeceleItem_Backpack(_AimItem, 1);
            EventListener.BattleSceneChange(AimScene);
        }
}

}