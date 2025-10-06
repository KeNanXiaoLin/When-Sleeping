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

        [SerializeField] private float ConformWaitingTime;
        private bool StartWaitingTime = false;
        private float CC_WaitingTime;


        void Awake()
        {
            ConformToBattleScene_Button.onClick.AddListener(ConformDrink);
            ReventPlayer_Button.onClick.AddListener(UnDrink);
        }

        void Update()
        {
            if (StartWaitingTime == true)
            {
                CC_WaitingTime -= Time.deltaTime;
            }

            if (CC_WaitingTime <= 0)
            {
                StartWaitingTime = false;
            }
        }

        public override void CheckIfSceneChangeChange()
        {
            foreach (Collider2D i in colliders)
            {
                if (i.GetComponent<VilliagePlayer>() != null)
                {
                    if (LA_Backpack.Instance.CheckItemNum_Backpack(_AimItem, 1) == true && StartWaitingTime == false)
                    {
                        ConformToBattleScene.SetActive(true);
                    }
                }
            }

        }

        private void ConformDrink()
        {
            SceneLoadManager.Instance.MilkDrinked = true;
            EventListener.BattleSceneChange(AimScene,E_SceneLoadType.None);
        }

        private void UnDrink()
        {
            SceneLoadManager.Instance.MilkDrinked = false;
            EventListener.BattleSceneChange(AimScene,E_SceneLoadType.None);
        }
}

}