using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GJ
{

    public class VilliageNPC : MonoBehaviour
    {
        public DialogData data;
        public LA_Item Item;
        
        public void PlayerGotItem()
        {
            LA_Backpack.instence.ShowGotItemUI_Backpack(Item);
        }
    }

}