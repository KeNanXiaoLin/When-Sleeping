using System.Collections;
using System.Collections.Generic;
using GJ;
using UnityEngine;

namespace GJ
{

public class BugFix : MonoBehaviour
{
    public GameObject WinScene;
        public GameObject LoseScene;

        void Update()
        {
            if (BattleSceneManager.Instance.WinScene == null)
                BattleSceneManager.Instance.WinScene = WinScene;

            if (BattleSceneManager.Instance.LoseScene == null)
                BattleSceneManager.Instance.LoseScene = LoseScene;
            
        }
}

}