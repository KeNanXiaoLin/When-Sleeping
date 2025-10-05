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

        public bool IsDialogued = false;

        void Awake()
        {
            EventListener.OnDialogueStart += StartDialogue;
        }

        public void PlayerGotItem()
        {
            LA_Backpack.instence.ShowGotItemUI_Backpack(Item);
        }

        private void StartDialogue()
        {
            if (IsDialogued == false)
            {
                IsDialogued = true;
                DialogSystem.Instance.TriggerStartDialog(data);
            }
            else
            {
                EventListener.DialogueEnd();
            }
        }
    }

}