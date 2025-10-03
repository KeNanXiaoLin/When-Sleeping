using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unuse
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager instence;

        [Header("场景中的玩家对象")]
        public Transform player_Trans;

        [Header("用于生成道具实体的Prefab")]
        public GameObject DropItemPrefab;

        [Header("背包和制作系统的Canvas")]
        public GameObject BackpackUICanvas;
        public GameObject WorkBenchUICanvas;

        [Header("在靠近制作台时才会显示的配方")]
        public List<GameObject> Sp_recipe;


        void Awake()
        {
            if (instence != null) Destroy(instence);
            else instence = this;

            BackpackUICanvas.SetActive(false);
            WorkBenchUICanvas.SetActive(false);
        }

        public static void RotateToDirection(Vector3 _AimPositon, Vector3 _OrgialPositon, Transform _RotateObject, float rotationSpeed = 3f)
        {
            Vector2 direction = _AimPositon - _OrgialPositon;

            var radian = Mathf.Atan2(direction.y, direction.x);
            var angle = radian * (180 / Mathf.PI);
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
            _RotateObject.rotation = Quaternion.Slerp(_RotateObject.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        public void DisplaySpecial(bool _CanDisplay)
        {
            if (_CanDisplay)
            {
                foreach (GameObject i in Sp_recipe)
                {
                    i.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject i in Sp_recipe)
                {
                    i.SetActive(false);
                }
            }
        }
    }
}
