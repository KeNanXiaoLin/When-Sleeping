using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGManager : SingletonAutoMono<CGManager>
{

    /// <summary>
    /// 播放开头CG
    /// </summary>
    public IEnumerator PlayKaiTouCG()
    {
        MusicManager.Instance.PlayBKMusic("阴森的小曲1");
        MusicManager.Instance.PlaySound("心跳声");
        yield return PlayKaiTouCGAnim(3f);
        MusicManager.Instance.ClearSound();
        MusicManager.Instance.PlayBKMusic("轻松小曲1");
    }

    /// <summary>
    /// 播放时钟动画
    /// </summary>
    /// <param name="time">播放每张图片的时间间隔</param>
    /// <returns></returns>
    private IEnumerator PlayKaiTouCGAnim(float time)
    {
        List<Sprite> clockSprites = new();
        clockSprites.Add(Resources.Load<Sprite>("Sprites/kaitouCG"));
        UIManager.Instance.ShowPanel<RealCGPanel>();
        RealCGPanel gPanel = UIManager.Instance.GetPanel<RealCGPanel>();
        //从后往前移除列表
        for (int i = clockSprites.Count - 1; i >= 0; i--)
        {
            gPanel.UpdateInfo(clockSprites[i]);
            clockSprites.Remove(clockSprites[i]);
            yield return new WaitForSeconds(time);
        }
        UIManager.Instance.HidePanel<RealCGPanel>();
    }

    /// <summary>
    /// 播放吃人CG
    /// </summary>
    public void PlayChiRenCG()
    {
        StartCoroutine(PlayChiRenCGAnim(3f));
    }

    private IEnumerator PlayChiRenCGAnim(float time)
    {
        List<Sprite> clockSprites = new();
        clockSprites.Add(Resources.Load<Sprite>("Sprites/chirenCG"));
        UIManager.Instance.ShowPanel<RealCGPanel>();
        RealCGPanel gPanel = UIManager.Instance.GetPanel<RealCGPanel>();
        //从后往前移除列表
        for (int i = clockSprites.Count - 1; i >= 0; i--)
        {
            gPanel.UpdateInfo(clockSprites[i]);
            clockSprites.Remove(clockSprites[i]);
            yield return new WaitForSeconds(time);
        }
        UIManager.Instance.HidePanel<RealCGPanel>();

        PlayJuqing();
    }

    /// <summary>
    /// 播放结束CG
    /// </summary>
    public void PlayEndCG()
    {
        StartCoroutine(PlayEndCGAnim(3f));
    }

    private IEnumerator PlayEndCGAnim(float time)
    {
        List<Sprite> clockSprites = new();
        for (int i = 3; i > 0; i--)
        {
            clockSprites.Add(Resources.Load<Sprite>("Sprites/endCG" + i));
        }
        UIManager.Instance.ShowPanel<RealCGPanel>();
        RealCGPanel gPanel = UIManager.Instance.GetPanel<RealCGPanel>();
        //从后往前移除列表
        for (int i = clockSprites.Count - 1; i >= 0; i--)
        {
            gPanel.UpdateInfo(clockSprites[i]);
            clockSprites.Remove(clockSprites[i]);
            yield return new WaitForSeconds(time);
        }
        UIManager.Instance.HidePanel<RealCGPanel>();
    }

    /// <summary>
    /// 播放中间剧情
    /// </summary>
    public void PlayJuqing()
    {
        StartCoroutine(PlayJuqingAnim());
    }

    private IEnumerator PlayJuqingAnim()
    {

        List<Sprite> clockSprites = new();
        for (int i = 6; i > 0; i--)
        {
            clockSprites.Add(Resources.Load<Sprite>("Sprites/d" + i));
        }
        UIManager.Instance.ShowPanel<RealCGPanel>();
        RealCGPanel gPanel = UIManager.Instance.GetPanel<RealCGPanel>();
        bool isPlayNext = true;
        //从后往前移除列表
        while (clockSprites.Count > 0)
        {
            //允许播放下一张
            if (isPlayNext)
            {
                gPanel.UpdateInfo(clockSprites[clockSprites.Count - 1]);
                clockSprites.Remove(clockSprites[clockSprites.Count - 1]);
                isPlayNext = false;
            }
            else
            {
                //玩家点击鼠标左键允许播放下一张
                if (Input.GetMouseButtonDown(0))
                {
                    isPlayNext = true;
                }
            }

            yield return null;
        }
        UIManager.Instance.HidePanel<RealCGPanel>();

        PlayEndCG();
    }

    public IEnumerator PlayClockAnim()
    {
        MusicManager.Instance.PlaySound("时钟的音效2");
        yield return PlayClockAnimCoroutine(0.3f);
        MusicManager.Instance.ClearSound();
        
    }
    /// <summary>
    /// 播放时钟动画
    /// </summary>
    /// <param name="time">播放每张图片的时间间隔</param>
    /// <returns></returns>
    private IEnumerator PlayClockAnimCoroutine(float time)
    {

        List<Sprite> clockSprites = new();
        for (int i = 5; i >= 1; i--)
        {
            clockSprites.Add(Resources.Load<Sprite>("Sprites/Clock" + i));
        }
        UIManager.Instance.ShowPanel<CGPanel>();
        CGPanel gPanel = UIManager.Instance.GetPanel<CGPanel>();
        //从后往前移除列表
        for (int i = clockSprites.Count - 1; i >= 0; i--)
        {
            gPanel.UpdateImage(clockSprites[i]);
            clockSprites.Remove(clockSprites[i]);
            yield return new WaitForSeconds(time);
        }
        UIManager.Instance.HidePanel<CGPanel>();
    }

}

