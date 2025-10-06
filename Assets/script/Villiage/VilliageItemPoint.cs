using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VilliageItemPoint : MonoBehaviour
{
    [SerializeField] private LA_Item item;
    private SpriteRenderer ItemImage;
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
        ItemImage.color = new Color(0, 0, 0, 0);
        Ischecked = true;
    }

    IEnumerator InActiveForSomeTime()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }
}
