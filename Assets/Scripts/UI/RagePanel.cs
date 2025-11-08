using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagePanel : UIPanelBase
{
    public CanvasGroup group;
    public override void HideMe()
    {
        
    }

    public override void ShowMe()
    {

    }

    void OnEnable()
    {
        EventCenter.Instance.AddEventListener<float>(E_EventType.E_RageAdd, UpdateRageVal);
    }

    void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<float>(E_EventType.E_RageAdd, UpdateRageVal);
    }

    public void UpdateRageVal(float target)
    {
        float endV = group.alpha + target;
        if (endV > 1)
            endV = 1;
        StartCoroutine(addToTarget(endV));
    }

    private IEnumerator addToTarget(float target)
    {
        float now = group.alpha;
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(now, target, t);
            yield return null;
        }
    }

    public void FLighting()
    {
        StartCoroutine(FLightingCoroutine());
    }
    
    private IEnumerator FLightingCoroutine()
    {
        float t = 0;
        int count = 0;
        while (t < 2)
        {
            t += Time.deltaTime;
            if (count % 2 == 0)
            {
                group.alpha = 0;
            }
            else
            {
                group.alpha = 1;
            }
            count += 1;
            yield return null;
        }
        group.alpha = 1;
    }
}
