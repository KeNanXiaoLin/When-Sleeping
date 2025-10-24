using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusData
{
    public int maxSan = 100;
    private int curSan;

    public PlayerStatusData()
    {
        curSan = maxSan;
    }

    public void ChangeSan(int value)
    {
        curSan += value;
        if (curSan > maxSan)
        {
            curSan = maxSan;
        }
        else if (curSan <= 0)
        {
            curSan = 0;
        }
        EventCenter.Instance.EventTrigger<int>(E_EventType.E_SanChange, curSan);
    }
}
