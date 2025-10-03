using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unuse
{

    public class LA_DropItem : MonoBehaviour
    {
        // SpriteRenderer 组件，用于渲染道具的图片
        private SpriteRenderer Render;
        // 玩家的 Transform 组件，用于计算道具与玩家的距离和方向
        private Transform player_Tran;

        [Header("道具被玩家吸附相关变量")]
        [Tooltip("道具吸附范围")] public float AttactRate = 5f;
        [Tooltip("道具吸附速度")] public float AttactSpeed = 5f;

        // 道具的 ScriptableObject 数据，包含道具的图片和其他属性
        public LA_Item ItemOS;

        // 初始化函数，在对象激活时调用，设置道具的图片和获取玩家的 Transform
        private void OnEnable()
        {
            Render = this.GetComponent<SpriteRenderer>();
            Render.sprite = ItemOS.ItemPicture;
            player_Tran = GameManager.instence.player_Trans;
        }

        // 物理更新函数，每帧调用，处理道具被玩家吸附的逻辑
        private void FixedUpdate()
        {
            if (player_Tran != null)
            {
                float distance = Vector3.Distance(transform.position, player_Tran.position);
                if (distance <= AttactRate)
                {
                    // 冲向玩家
                    Vector3.MoveTowards(transform.position, player_Tran.position, AttactSpeed * Time.deltaTime);
                    // 旋转朝向玩家
                    GameManager.RotateToDirection(player_Tran.position, transform.position, transform);
                }
            }
        }

        // 碰撞检测函数，当道具与玩家碰撞时，将道具添加到背包并禁用道具
        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (other.GetComponent<LA_Player>())
        //     {
        //         LA_Backpack.instence.AddItm_Backpack(ItemOS);
        //         // 触碰玩家后取消活跃状态
        //         gameObject.SetActive(false);
        //     }
        // }
    }

}