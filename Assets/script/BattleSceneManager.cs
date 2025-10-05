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
        }

        public GameObject Player;
        public List<GameObject> Enemy = new List<GameObject>();
    }

}