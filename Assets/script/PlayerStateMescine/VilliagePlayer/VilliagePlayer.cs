using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.Experimental;
using UnityEngine;

namespace GJ
{


    public class VilliagePlayer : MonoBehaviour
    {
        [SerializeField] private float MoveSpeed = 5;
        [SerializeField] private float NPCDetectRate = 10;

        private float Horizonal;
        private float vertual;
        private Collider2D[] colliders;

        private bool IsMoving = true;

        private VilliageNPC NPC;
        private VilliageItemPoint ItemPoint;
        private Animator anim;

        void Awake()
        {
            anim = this.GetComponent<Animator>();
        }

        void Start()
        {
            EventListener.OnDialogueEnd += OnDialogueEnd;
            EventListener.OnCheckedItemGot += OnDialogueEnd;
        }

        void Update()
        {
            //TODO 检测到NPC后，提示玩家按键操作
            colliders = Physics2D.OverlapCircleAll(this.transform.position, NPCDetectRate);
            DetectNPC();
            DetectItemPoint();

            if (IsMoving == true)
            {
                Horizonal = Input.GetAxisRaw("Horizontal");
                vertual = Input.GetAxisRaw("Vertical");

                this.transform.Translate(new Vector2(Horizonal, vertual) * MoveSpeed * Time.deltaTime);

                //设置动画
                anim.SetFloat("XMove", Horizonal);
                anim.SetFloat("YMove", vertual);

                if (Horizonal != 0 || vertual != 0)
                {
                    anim.SetBool("Moving", true);
                    anim.SetFloat("XIdle", Horizonal);
                    anim.SetFloat("YIdle", vertual);
                }
                else anim.SetBool("Moving", false);

            }

 

                if (Input.GetKeyDown(KeyCode.F))
            {
                    if (NPC != null)
                    {
                        IsMoving = false;
                        EventListener.DialogueStart();
                    }
                    else if (ItemPoint != null)
                    {
                        IsMoving = false;
                        EventListener.ItemGot();
                }
            }
            
        }

        private bool DetectNPC()
        {
            if (NPC != null && Vector2.Distance(this.transform.position, NPC.transform.position) < NPCDetectRate)
            {
                return true;
            }
            else
            {
                NPC = null;
            }

            foreach (var i in colliders)
            {
                if (i.GetComponent<VilliageNPC>() != null)
                {
                    NPC = i.GetComponent<VilliageNPC>();
                    return true;
                }
            }

            return false;
        }

        //检测场景中的道具实体单位
        private bool DetectItemPoint()
        {
            foreach (var i in colliders)
            {
                if (i.GetComponent<VilliageItemPoint>() != null)
                {
                    ItemPoint = i.GetComponent<VilliageItemPoint>();
                    return true;
                }
            }

            return false;

        }

        private void OnDialogueEnd() => IsMoving = true;

        public float GetMoveSpeed_VPlayer() => MoveSpeed;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(this.transform.position, NPCDetectRate);
        }

    }
}