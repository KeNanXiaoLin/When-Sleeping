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

        protected override void Awake()
        {
            base.Awake();

            WinScene.SetActive(false);
            LoseScene.SetActive(false);

            EventListener.OnGameWin += ShowWinUI;
            EventListener.OnGameLose += ShowLoseUI;
        }

        
        [HideInInspector] public GameObject Player;
        public List<GameObject> Enemy = new List<GameObject>();

        public GameObject WinScene;
        public GameObject LoseScene;

        public bool CheckInput = false;

        void FixedUpdate()
        {
            if (SceneManager.GetActiveScene().name == "BattleScene")
            {
                CheckingEnemy = true;
            }
            if (Enemy.Count <= 0 && CheckingEnemy == true)
            {
                EventListener.GameWin();
            }

        }

        void Update()
        {
            if (CheckInput == true && Input.GetMouseButtonDown(0))
            {
                EventListener.VilliageSceneChange("GameScene3");    
            }

        }

        private void ShowWinUI()
        {
            WinScene.SetActive(true);
            WinScene.GetComponentInChildren<Animator>().Play("ShowSlowly");

            CheckInput = true;
        }


        private void ShowLoseUI()
        {
            LoseScene.SetActive(true);
            LoseScene.GetComponentInChildren<Animator>().Play("ShowSlowly");
        }
            
    }

}