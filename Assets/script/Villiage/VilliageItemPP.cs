using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class VilliageItemPP : MonoBehaviour
    {
        public LA_Item item;
        public SpriteRenderer ItemImage;
        private bool Ischecked = false;

        void Awake()
        {
            ItemImage = GetComponent<SpriteRenderer>();
            ItemImage.sprite = item.ItemPicture;
        }

        void Start()
        {
            EventListener.OnItemGot += GotItem;
            EventListener.OnCheckedItemGot += CheckedItemGot;
        }

        private void GotItem()
        {
        
                LA_Backpack.Instance.AddItm_Backpack(item);
                LA_Backpack.Instance.ShowGotItemUI_Backpack(item);

                MusicManager.Instance.PlaySound("发现证物的音效1");

                EventListener.OnCheckedItemGot += CheckedItemGot;
        }

        private void CheckedItemGot()
        {
            EventListener.OnCheckedItemGot -= CheckedItemGot;

            this.gameObject.SetActive(false);
        }

        void OnDisable()
        {
            EventListener.OnItemGot -= GotItem;
        }

    }


}