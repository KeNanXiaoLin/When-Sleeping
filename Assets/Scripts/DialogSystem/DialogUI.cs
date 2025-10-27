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

        void Start()
        {
            // nextButton.onClick.AddListener(() =>
            // {
            //     DialogSystemMgr.Instance.PlayNextDialog();
            // });
        }


        public void ShowDialog(DialogData data)
        {
            dialogText.text = data.dialogText;
            mainRoleImage.sprite = Resources.Load<Sprite>(data.headIconRes);
            mainRoleNameText.text = data.dialogName;
        }

        public void PlayNextDialog()
        {
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
