using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KNXL.DialogSystem
{
    public class DialogChooseUI : UIPanelBase
    {
        public Image imgMainRole;
        public TextMeshProUGUI tmpName;
        public Button btnOption1;
        public Button btnOption2;
        public Button btnOption3;
        public TextMeshProUGUI tmpOption1;
        public TextMeshProUGUI tmpOption2;
        public TextMeshProUGUI tmpOption3;

        private DialogData dialogData1;
        private DialogData dialogData2;
        private DialogData dialogData3;

        public override void HideMe()
        {

        }

        public override void ShowMe()
        {

        }
        /*
public void ShowDialog(List<DialogData> datas)
{
   if (datas == null || datas.Count == 0)
   {
       Debug.LogError("请检查配置文件，传入的数据不能为空");
       return;
   }
   switch (datas.Count)
   {
       case 1:
           DialogData data1 = datas[0];
           tmpOption1.text = data1.dialogText;
           dialogData1 = data1;
           btnOption1.onClick.AddListener(() =>
           {
               DialogSystemMgr.Instance.PlayNextDialog(dialogData1);
           });
           //禁用按钮2和3的显示
           btnOption2.gameObject.SetActive(false);
           tmpOption2.gameObject.SetActive(false);
           btnOption3.gameObject.SetActive(false);
           tmpOption3.gameObject.SetActive(false);
           break;
       case 2:
           data1 = datas[0];
           DialogData data2 = datas[1];
           tmpOption1.text = data1.dialogText;
           dialogData1 = data1;
           btnOption1.onClick.AddListener(() =>
           {
               DialogSystemMgr.Instance.PlayNextDialog(dialogData1);
           });
           dialogData2 = data2;
           tmpOption2.text = data2.dialogText;
           btnOption2.onClick.AddListener(() =>
           {
               DialogSystemMgr.Instance.PlayNextDialog(dialogData2);
           });
           //禁用按钮3的显示
           btnOption3.gameObject.SetActive(false);
           tmpOption3.gameObject.SetActive(false);
           break;
       case 3:
           data1 = datas[0];
           data2 = datas[1];
           DialogData data3 = datas[2];
           tmpOption1.text = data1.dialogText;
           dialogData1 = data1;
           btnOption1.onClick.AddListener(() =>
           {
               DialogSystemMgr.Instance.PlayNextDialog(dialogData1);
           });
           dialogData2 = data2;
           tmpOption2.text = data2.dialogText;
           btnOption2.onClick.AddListener(() =>
           {
               DialogSystemMgr.Instance.PlayNextDialog(dialogData2);
           });
           dialogData3 = data3;
           tmpOption3.text = data3.dialogText;
           btnOption3.onClick.AddListener(() =>
           {
               DialogSystemMgr.Instance.PlayNextDialog(dialogData3);
           });
           break;
   }

}*/

    }
}
