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

        void Start()
        {
            EventListener.OnDialogueStart += StartDialogue;

            EventListener.OnDialogueEnd += LetPlayerGotItem;
            EventListener.OnDialogueEnd += EndDialogue;
        }

        private void StartDialogue()
        {
            if (IsDialogued == false)
            {

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

            LA_Backpack.Instance.AddItm_Backpack(Item);
            LA_Backpack.Instance.ShowGotItemUI_Backpack(Item);
        }

        private void EndDialogue()
        {
            IsDialogued = true;
        }
    }

}