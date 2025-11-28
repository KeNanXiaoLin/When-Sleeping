using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MyVector3
{
    public float x;
    public float y;
    public float z;
    public MyVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public MyVector3(Vector3 vec3)
    {
        this.x = vec3.x;
        this.y = vec3.y;
        this.z = vec3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public void Set(Vector3 vec3)
    {
        x = vec3.x;
        y = vec3.y;
        z = vec3.z;
    }

    public static implicit operator Vector3(MyVector3 vec3)
    {
        return vec3.ToVector3();
    }

}

[System.Serializable]
public class GameData
{
    /// <summary>
    /// 玩家的位置
    /// </summary>
    public MyVector3 playerPos = new MyVector3(-5, 3, 0);
    /// <summary>
    /// 当前玩家所在的场景的名字
    /// </summary>
    public string curSceneName = Setting.GameScene3;
}
