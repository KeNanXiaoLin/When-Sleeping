using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGManager : MonoBase<CGManager>
{

    public bool CGPlaying = false;

    /// <summary>
    /// 播放开头CG
    /// </summary>
    public void PlayKaiTouCG()
    {
        StartCoroutine(PlayKaiTouCGAnim(3f));
    }

    /// <summary>
    /// 播放时钟动画
    /// </summary>
    /// <param name="time">播放每张图片的时间间隔</param>
    /// <returns></returns>
    private IEnumerator PlayKaiTouCGAnim(float time)
    {
        CGPlaying = true;

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

        CGPlaying = false;

        //额外变量
        SceneLoadManager.Instance.LoadScene("GameScene3", E_SceneLoadType.None);
        MusicManager.Instance.PlayBKMusic("轻松小曲2");

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
        CGPlaying = true;

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
        CGPlaying = true;

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

        Application.Quit();

        CGPlaying = false;
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
    
    public void PlayerEnd()
    {
    
    }

}

