using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class ReplaceTMPFonts
{
    [MenuItem("替换字体资源", menuItem = "Tools/替换为指定字体资源")]
    private static void Function()
    {
        TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Fonts/PixelFonts SDF");
        var prefabGuids = AssetDatabase.FindAssets("t:Prefab"); // 查找所有预制体
        foreach (var guid in prefabGuids)
        {
            var prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null) continue;
            var allComs = prefab.GetComponentsInChildren<TMP_Text>();
            if (allComs == null || allComs.Length == 0) continue;
            foreach (var item in allComs)
            {
                item.font = fontAsset;
            }

            // var serializedObj = new SerializedObject(prefab);
            // var propertyIterator = serializedObj.GetIterator();
            // while (propertyIterator.NextVisible(true))
            // {
            //     if (propertyIterator.propertyType == SerializedPropertyType.ObjectReference &&
            //         propertyIterator.objectReferenceValue == oldAsset)
            //     {
            //         propertyIterator.objectReferenceValue = newAsset;
            //     }
            // }
            // serializedObj.ApplyModifiedProperties();
            AssetDatabase.SaveAssetIfDirty(prefab); // 保存预制体修改
        }
    }
}
