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
        }

        void Start()
        {
            EventListener.OnItemGot += GotItem;
            EventListener.OnCheckedItemGot += CheckedItemGot;

            ItemImage.sprite = item.ItemPicture;
        }

        private void GotItem()
        {
            if (Ischecked == true) return;

            LA_Backpack.Instance.AddItm_Backpack(item);
            LA_Backpack.Instance.ShowGotItemUI_Backpack(item);
        }

        private void CheckedItemGot()
        {
            Debug.Log("Checking");

            if (ItemImage == null)
            {
                ItemImage = this.GetComponent<SpriteRenderer>();
                ItemImage.color = new Color(0, 0, 0, 0);
            }
            Ischecked = true;
        }


    }


}