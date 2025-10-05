using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class BattleSceneManager : MonoBehaviour
    {
        public static BattleSceneManager instence;

        private void Awake()
        {
            if (instence == null) instence = this;
            else Destroy(instence);

            WinScene.SetActive(false);
        }

        [HideInInspector] public GameObject Player;
        [HideInInspector] public List<GameObject> Enemy = new List<GameObject>();

        public GameObject WinScene;

        void FixedUpdate()
        {
            if (Enemy.Count <= 0)
            {
                WinScene.SetActive(true);
            }
        }
    }

}