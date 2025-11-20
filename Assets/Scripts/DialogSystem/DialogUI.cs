using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KNXL.DialogSystem
{

    public class DialogUI : UIPanelBase
    {
        // 主角色信息
        public Image mainRoleImage;
        public TextMeshProUGUI mainRoleNameText;
        // 对话文本
        public TextMeshProUGUI dialogText;
        // public Button nextButton;
        private bool isPlot = false;

        public bool IsPlot { get => isPlot; set => isPlot = value; }
        private StringBuilder sb;
        //是否处于播放对话动画的状态
        private bool isPlayAnim = false;

        void Start()
        {
            // nextButton.onClick.AddListener(() =>
            // {
            //     DialogSystemMgr.Instance.PlayNextDialog();
            // });
        }


        public void ShowDialog(DialogData data)
        {
            isPlayAnim = false;
            dialogText.text = data.dialogText;
            mainRoleImage.sprite = Resources.Load<Sprite>(data.headIconRes);
            mainRoleNameText.text = data.dialogName;
        }

        public IEnumerator DialogPlayAnimCoroutine(DialogData data, WaitForSeconds intervalTime)
        {
            isPlayAnim = true;
            mainRoleImage.sprite = Resources.Load<Sprite>(data.headIconRes);
            mainRoleNameText.text = data.dialogName;
            sb = new StringBuilder(data.dialogText);
            int count = 0;
            int len = sb.Length;
            string showText = "";
            while (count < len)
            {
                showText += sb[count];
                dialogText.text = showText;
                count++;
                yield return intervalTime;
            }
            isPlayAnim = false;
        }

        /// <summary>
        /// 播放下一句对话，只有在不处于播放对话动画的时候，才可以播放下一句对话
        /// </summary>
        public void PlayNextDialog()
        {
            if (!isPlayAnim)
                DialogSystemMgr.Instance.PlayNextDialog();
        }

        public override void ShowMe()
        {

        }

        public override void HideMe()
        {

        }
    }
}
