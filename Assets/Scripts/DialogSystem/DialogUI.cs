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
            switch (data.type)
            {
                case E_DialogRoleType.MainRole:
                    mainRoleImage.sprite = Resources.Load<Sprite>("Sprites/MainRole");
                    mainRoleNameText.text = "Mike";
                    break;
                case E_DialogRoleType.Mom:
                    mainRoleImage.sprite = Resources.Load<Sprite>("Sprites/Mom");
                    mainRoleNameText.text = "Mom";
                    break;
                case E_DialogRoleType.Bob:
                    mainRoleImage.sprite = Resources.Load<Sprite>("Sprites/Bob");
                    mainRoleNameText.text = "Bob";
                    break;
            }
        }

        public void PlayNextDialog()
        {
            if (!isPlot)
            {
                DialogSystemMgr.Instance.PlayNextDialog();
            }
            else
            {
                DialogSystemMgr.Instance.PlayNextPlotDialog();
            }
        }

        public override void ShowMe()
        {

        }

        public override void HideMe()
        {

        }
    }
}
