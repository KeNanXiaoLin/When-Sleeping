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

        [SerializeField] private bool OnlyAvaInDayTwo = false;
        [SerializeField] private bool Mom = false;


        public bool IsDialogued = false;

        void Start()
        {
            if (OnlyAvaInDayTwo == false && SceneLoadManager.Instance.DayIndex == 0
            || OnlyAvaInDayTwo == true && SceneLoadManager.Instance.DayIndex == 1)
            {
                EventListener.OnDialogueStart += StartDialogue;

                EventListener.OnDialogueEnd += LetPlayerGotItem;
                EventListener.OnDialogueEnd += EndDialogue;
            }
            else
                this.gameObject.SetActive(false);


            //Mom专用判断条件
            if (Mom == true && SceneLoadManager.Instance.LoadScene3Index < 2)
            {
                EventListener.OnDialogueStart -= StartDialogue;

                EventListener.OnDialogueEnd -= LetPlayerGotItem;
                EventListener.OnDialogueEnd -= EndDialogue;
                this.gameObject.SetActive(false);
            }
        }

        private void StartDialogue()
        {
            if (IsDialogued == false)
            {
                DialogSystem.Instance.TriggerStartDialog(data);
                NPCNotice = this.transform.GetChild(0).gameObject;
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

            if (Item != null)
            {
                LA_Backpack.Instance.AddItm_Backpack(Item);
                LA_Backpack.Instance.ShowGotItemUI_Backpack(Item);

                MusicManager.Instance.PlaySound("发现证物的音效1");
            }
        }

        private void EndDialogue()
        {
            IsDialogued = true;
        }
    }

}