using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 音乐音效管理器
/// </summary>
public class MusicManager:SingletonAutoMono<MusicManager>
{
    //背景音乐播放组件
    private AudioSource bkMusic = null;

    //管理正在播放的音效
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效是否在播放
    private bool soundIsPlay = true;
    public MusicData musicData;

    private void Awake()
    {
        string path = Application.persistentDataPath + "/MusicData.json";
        //如果文件不存在 则创建一个新文件
        if(!File.Exists(path))
        {
            musicData = new MusicData();
        }
        else
        {
            //从文件中读取数据
            musicData = JsonMgr.Instance.LoadDataFromFilePath<MusicData>(path);
        }
        
    }

    void Update()
    {
        if(soundList != null && soundList.Count > 0)
        {
            for (int i = soundList.Count-1; i >= 0; i--)
            {
                if(!soundList[i].isPlaying)
                {
                    StopSound(soundList[i]);
                }
            }
        }
    }


    //播放背景音乐
    public void PlayBKMusic(string name)
    {
        //动态创建播放背景音乐的组件 并且 不会过场景移除 
        //保证背景音乐在过场景时也能播放
        if (bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BKMusic";
            GameObject.DontDestroyOnLoad(obj);
            bkMusic = obj.AddComponent<AudioSource>();
        }
        AudioClip clip = Resources.Load<AudioClip>("music/" + name);
        if (clip != null)
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = musicData.bkMusicValue;
            bkMusic.mute = musicData.bkMusicMute;
            bkMusic.Play();
        }
        else
        {
            Debug.LogWarning("加载资源失败，请检查加载资源路径是否存在问题" + "Music/" + name);
        }
    }

    //停止背景音乐
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }

    //暂停背景音乐
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    //设置背景音乐大小
    public void ChangeBKMusicValue(float v)
    {
        musicData.bkMusicValue = v;
        if (bkMusic == null)
            return;
        bkMusic.volume = musicData.bkMusicValue;
    }

    /// <summary>
    /// 调整背景音乐是否静音
    /// </summary>
    /// <param name="mute"></param>
    public void ChangeBKMusicMute(bool mute)
    {
        musicData.bkMusicMute = mute;
        if (bkMusic == null)
            return;
        bkMusic.mute = musicData.bkMusicMute;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名字</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="isSync">是否同步加载</param>
    /// <param name="callBack">加载结束后的回调</param>
    public void PlaySound(string name, bool isLoop = false, bool isSync = false, UnityAction<AudioSource> callBack = null)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/" + name);
        if (clip != null)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = this.transform;
            AudioSource source = obj.AddComponent<AudioSource>();
            //加入音效列表，方便管理
            soundList.Add(source);
            //播放音效
            source.clip = clip;
            source.loop = isLoop;
            source.volume = musicData.soundValue;
            source.Play();
        }
        else
        {
            Debug.LogWarning("加载资源失败，请检查加载资源路径是否存在问题" + "Sound/" + name);
        }
    }

    /// <summary>
    /// 停止播放音效
    /// </summary>
    /// <param name="source">音效组件对象</param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            //停止播放
            source.Stop();
            //从容器中移除
            soundList.Remove(source);
            //不用了 清空切片 避免占用
            source.clip = null;
            //销毁物体，避免占用内存
            Destroy(source.gameObject);
        }
    }

    /// <summary>
    /// 改变音效大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundValue(float v)
    {
        musicData.soundValue = v;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = musicData.soundValue;
        }
    }
    
    /// <summary>
    /// 调整音效是否禁音
    /// </summary>
    /// <param name="mute"></param>
    public void ChangeSoundMute(bool mute)
    {
        musicData.soundMute = mute;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].mute = musicData.soundMute;
        }
    }

    /// <summary>
    /// 继续播放或者暂停所有音效
    /// </summary>
    /// <param name="isPlay">是否是继续播放 true为播放 false为暂停</param>
    public void PlayOrPauseSound(bool isPlay)
    {
        if (isPlay)
        {
            soundIsPlay = true;
            for (int i = 0; i < soundList.Count; i++)
                soundList[i].Play();
        }
        else
        {
            soundIsPlay = false;
            for (int i = 0; i < soundList.Count; i++)
                soundList[i].Pause();
        }
    }

    /// <summary>
    /// 清空音效相关记录 过场景时在清空缓存池之前去调用它
    /// </summary>
    public void ClearSound()
    {
        for (int i = soundList.Count - 1; i >= 0; i--)
        {
            soundList[i].Stop();
            soundList[i].clip = null;
        }
    }

    /// <summary>
    /// 存储音乐音效数据到本地
    /// </summary>
    internal void SaveMusicData()
    {
        string path = Application.persistentDataPath + "/MusicData.json";
        JsonMgr.Instance.SaveDataToFilePath(musicData, path);
    }
}
