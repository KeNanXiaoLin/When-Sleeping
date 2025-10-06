using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace GJ
{

    public class BattleSceneManager : MonoBase<BattleSceneManager>
    {
        private bool CheckingEnemy = false;
        private float checkTime = 2f;
        private float CC_CheckTime;
        private bool canCheck = false;


        protected override void Awake()
        {
            base.Awake();

            WinScene.SetActive(false);
            LoseScene.SetActive(false);

            EventListener.OnGameWin += ShowWinUI;
            EventListener.OnGameLose += ShowLoseUI;

            CC_CheckTime = checkTime;
        }


        [HideInInspector] public GameObject Player;
        public List<GameObject> Enemy = new List<GameObject>();

        public GameObject WinScene;
        public GameObject LoseScene;

        public bool CheckInput = false;

        public bool CheckInputFalse { get; private set; }

        void LateUpdate()
        {

            if (SceneLoadManager.Instance.CurrentScene == "BattleScene" && CheckingEnemy == false && Enemy.Count > 0)
            {
                CheckingEnemy = true;
            }

            if (CheckingEnemy == true && Enemy.Count <= 0)
            {
                EventListener.GameWin();
                CheckingEnemy = false;
            }


        }

        void Update()
        {

            //胜利后切换场景
            if (canCheck == true)
            {
                CC_CheckTime -= Time.deltaTime;
            }
            if (CC_CheckTime <= 0)
            {
                CheckInput = true;
                canCheck = false;
            }

            if (SceneLoadManager.Instance.CurrentScene == "BattleScene")
            {

                if (CheckInput == true && Input.GetMouseButtonDown(0))
                {
                    if (WinScene.activeSelf == true)
                    {
                        EventListener.VilliageSceneChange("GameScene3");

                        CheckInput = false;
                        WinScene.SetActive(false);
                    }

                    else if (LoseScene.activeSelf == true)
                    {
                        if (SceneLoadManager.Instance.MilkDrinked == false)
                            Application.Quit();
                        else
                        {
                            EventListener.ReloadScene("BattleScene");
                        }


                        CheckInput = false;
                        LoseScene.SetActive(false);
                    }
                }
            }
            if (SceneLoadManager.Instance.CurrentScene == "BattleScene 1")
            {
                if (CheckInput == true && Input.GetMouseButtonDown(0))
                {
                    if (LoseScene.activeSelf == true)
                    {
                        //启动CG

                        LoseScene.SetActive(false);
                    }
                }
            }
           

        }

        private void ShowWinUI()
        {
            WinScene.SetActive(true);
            WinScene.GetComponentInChildren<Animator>().Play("ShowSlowly");

            canCheck = true;
        }


        private void ShowLoseUI()
        {
            LoseScene.SetActive(true);
            LoseScene.GetComponentInChildren<Animator>().Play("ShowSlowly");

            canCheck = true;
        }

    }

}