using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace GJ
{
    public class VilliageNPC : MonoBehaviour
    {
        public LA_Item Item;
        public DialogData data;
        public GameObject NPCNotice;
        

        public bool IsDialogued = false;

        void Awake()
        {
            EventListener.OnDialogueStart += StartDialogue;

            EventListener.OnDialogueEnd += LetPlayerGotItem;
        }

        private void StartDialogue()
        {
            if (IsDialogued == false)
            {
                IsDialogued = true;
                DialogSystem.Instance.TriggerStartDialog(data);
                NPCNotice.SetActive(false);
            }
            else
            {
                EventListener.DialogueEnd();
            }
        }

        private void LetPlayerGotItem()
        {
            if (IsDialogued == true) return;

            LA_Backpack.instence.AddItm_Backpack(Item);
            LA_Backpack.instence.ShowGotItemUI_Backpack(Item);
        }
    }

}