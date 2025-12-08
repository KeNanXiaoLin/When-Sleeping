using System.Collections;
using System.Collections.Generic;
using KNXL.DialogSystem;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelUI : UIPanelBase
{
    public override void HideMe()
    {
        MusicManager.Instance.SaveMusicData();
    }

    public override void ShowMe()
    {
        //读取MusicManager中的音量信息对UI进行赋值
        GetControl<Slider>("MusicSli").value = MusicManager.Instance.musicData.bkMusicValue;
        GetControl<Slider>("SoundSli").value = MusicManager.Instance.musicData.soundValue;
        GetControl<Toggle>("MusicTog").isOn = !MusicManager.Instance.musicData.bkMusicMute;
        GetControl<Toggle>("SoundTog").isOn = !MusicManager.Instance.musicData.soundMute;
        GetControl<Toggle>("DialogTog").isOn = DialogSystemMgr.Instance.isOpenAnim;

    }

    protected override void SliderValueChange(string sliderName, float value)
    {
        Debug.Log("滑动被调用");
        switch (sliderName)
        {
            case "MusicSli":
                MusicManager.Instance.ChangeBKMusicValue(value);
                break;
            case "SoundSli":
                MusicManager.Instance.ChangeSoundValue(value);
                break;
        }
    }

    protected override void ToggleValueChange(string sliderName, bool value)
    {
        Debug.Log("Toggle被调用");
        switch (sliderName)
        {
            case "MusicTog":
                MusicManager.Instance.ChangeBKMusicMute(!value);
                break;
            case "SoundTog":
                MusicManager.Instance.ChangeSoundMute(!value);
                break;
            case "DialogTog":
                DialogSystemMgr.Instance.isOpenAnim = value;
                break;
        }
    }

    protected override void ClickBtn(string btnName)
    {
        Debug.Log("按钮被点击" + btnName);
        switch (btnName)
        {
            case "Close":
                UIManager.Instance.HidePanel<SettingPanelUI>();
                break;
        }
    }
}
