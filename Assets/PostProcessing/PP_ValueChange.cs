using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PP_ValueChange : MonoBehaviour
{
    //获取在Project中的PostProcessProfile文件
    [SerializeField] private PostProcessProfile postProfile;

    //要修改的效果被当作组件附着在PostProcessProfile上
    //这里定义一下需要修改数值的效果类，之后进行修改
    private ColorGrading PP_ColorGrading;

    //要修改的值
    public Color PP_ColorValue;
    public float PP_HueShiftChange;

    void Awake()
    {
        //获取文件中的效果类并赋值
        PP_ColorGrading = postProfile.GetSetting<ColorGrading>();

        //也可以动态添加/删除效果类
        //但是不推荐这么做，因为会导致效果类长期驻留在Profile文件中，且无法在Unity中动态修改
        // postProfile.AddSettings<Bloom>();

        // postProfile.RemoveSettings<ColorGrading>();
    }

    void Start()
    {
        //在源代码中，效果类的变量都为属性变量。
        PP_ColorGrading.colorFilter.value = PP_ColorValue;
        PP_ColorGrading.hueShift.value = PP_HueShiftChange;
    }

    void OnApplicationQuit()
    {
        //每次修改都是对文件属性值的长期驻留修改，游戏结束时不会自动恢复
        //所以必须手动恢复默认值，否则下次运行时会出现修改后的效果
        PP_ColorGrading.colorFilter.value = Color.white;
        PP_ColorGrading.hueShift.value = 0f;
    }

    //参考文献：https://blog.csdn.net/yigiwoliao/article/details/122322223
    //参考文献：https://docs.unity.cn/Packages/com.unity.postprocessing@3.2/api/UnityEngine.Rendering.PostProcessing.PostProcessProfile.html#UnityEngine_Rendering_PostProcessing_PostProcessProfile_GetSetting__1

}
