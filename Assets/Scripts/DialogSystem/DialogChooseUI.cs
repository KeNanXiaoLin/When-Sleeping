using System;
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
        public Button btnOption4;
        public TextMeshProUGUI tmpOption1;
        public TextMeshProUGUI tmpOption2;
        public TextMeshProUGUI tmpOption3;
        public TextMeshProUGUI tmpOption4;


        public override void HideMe()
        {

        }

        public override void ShowMe()
        {

        }

        public void ShowDialog(DialogData data)
        {
            if (data == null)
            {
                Debug.LogError("请检查配置文件，传入的数据不能为空");
                return;
            }
            if (data.dialogType != E_DialogType.Choose)
            {
                Debug.LogError("不能处理不是选择对话的节点，请检查传入类型");
                return;
            }
            if (data.option1 == "")
            {
                Debug.LogError("请检查配置数据，不能配置空选择节点");
                return;
            }
            else if (data.option2 == "")
            {
                ShowOneOption(data);
            }
            else if (data.option3 == "")
            {
                ShowTwoOption(data);
            }
            else if (data.option4 == "")
            {
                ShowThreeOption(data);
            }
            else
            {
                ShowFourOption(data);
            }
        }

        private void ShowFourOption(DialogData data)
        {
            tmpOption1.text = data.option1;
            tmpOption2.text = data.option2;
            tmpOption3.text = data.option3;
            tmpOption4.text = data.option4;
        }

        private void ShowThreeOption(DialogData data)
        {
            HideOption(4);
            tmpOption1.text = data.option1;
            tmpOption2.text = data.option2;
            tmpOption3.text = data.option3;
        }

        private void ShowTwoOption(DialogData data)
        {
            HideOption(3);
            HideOption(4);
            tmpOption1.text = data.option1;
            tmpOption2.text = data.option2;
        }

        private void ShowOneOption(DialogData data)
        {
            HideOption(2);
            HideOption(3);
            HideOption(4);
            tmpOption1.text = data.option1;
        }

        private void HideOption(int id)
        {
            switch (id)
            {
                case 1:
                    btnOption1.gameObject.SetActive(false);
                    break;
                case 2:
                    btnOption2.gameObject.SetActive(false);
                    break;
                case 3:
                    btnOption3.gameObject.SetActive(false);
                    break;
                case 4:
                    btnOption4.gameObject.SetActive(false);
                    break;
            }
        }
    }

}
