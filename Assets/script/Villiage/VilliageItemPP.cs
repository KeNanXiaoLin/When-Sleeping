using UnityEngine;

namespace GJ
{

    public class VilliageItemPP : MonoBehaviour
    {
        public LA_Item item;
        public SpriteRenderer ItemImage;
        private bool Ischecked = false;

        public bool OnlyAvaInDayTwo = false;

        void Awake()
        {
            ItemImage = GetComponent<SpriteRenderer>();
            ItemImage.sprite = item.ItemPicture;
        }

        void Start()
        {
            EventListener.OnItemGot += GotItem;

            //不符合条件判断
            if (OnlyAvaInDayTwo == true && SceneLoadManager.Instance.DayIndex == 0
            ||  OnlyAvaInDayTwo == false && SceneLoadManager.Instance.DayIndex == 1)
            {
                this.gameObject.SetActive(false);
            }
        }

        private void GotItem()
        {
        
                LA_Backpack.Instance.AddItm_Backpack(item);
                LA_Backpack.Instance.ShowGotItemUI_Backpack(item);

                MusicManager.Instance.PlaySound("发现证物的音效1",false,false);

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