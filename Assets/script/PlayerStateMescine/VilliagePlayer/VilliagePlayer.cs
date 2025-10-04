using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{


    public class VilliagePlayer : MonoBehaviour
    {
        [SerializeField] private float MoveSpeed = 5;
        [SerializeField] private float NPCDetectRate = 10;

        private float Horizonal;
        private float vertual;
        private bool IsMoving = true;

        private VilliageNPC NPC;


        public PlayerStateMescine StateMachine { get; private set; }

        void Awake()
        {
            StateMachine = GetComponent<PlayerStateMescine>();
        }

        void Start()
        {
            EventListener.OnDialogueEnd += OnDialogueEnd;
        }

        void Update()
        {
            //TODO 检测到NPC后，提示玩家按键操作
            DetectNPC();

            if (IsMoving == true)
            {
                Horizonal = Input.GetAxisRaw("Horizontal");
                vertual = Input.GetAxisRaw("Vertical");
                this.transform.Translate(new Vector2(Horizonal, vertual) * MoveSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.F) && NPC != null)
            {
                IsMoving = false;
                EventListener.DialogueStart();
            }
        }

        private bool DetectNPC()
        {
            if (Vector2.Distance(this.transform.position, NPC.transform.position) < NPCDetectRate)
            {
                return true;
            }
            else
            {
                NPC = null;
            }

            var objCollider = Physics2D.OverlapCircleAll(this.transform.position, NPCDetectRate);

            foreach (var i in objCollider)
            {
                if (i.GetComponent<VilliageNPC>() != null)
                {
                    NPC = i.GetComponent<VilliageNPC>();
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