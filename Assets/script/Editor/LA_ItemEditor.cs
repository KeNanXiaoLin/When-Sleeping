using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LA_Item))]
public class LA_ItemEditor : Editor
{

    private SerializedProperty ItemIDProp;
    private SerializedProperty ItemNameProp;
    private SerializedProperty itemTypeProp;
    private SerializedProperty ItemPictureProp;
    private SerializedProperty ItemIntoProp;
    private SerializedProperty repiteItemProp;
    private SerializedProperty repiteItemNumProp;
    private SerializedProperty AttackRateProp;
    private SerializedProperty DefenceRateProp;
    private SerializedProperty HealthUpRateProp;
    private void OnEnable()
    {
        ItemIDProp = serializedObject.FindProperty("itemID");
        ItemNameProp = serializedObject.FindProperty("ItemName");
        itemTypeProp = serializedObject.FindProperty("itemType");
        ItemPictureProp = serializedObject.FindProperty("ItemPicture");
        ItemIntoProp = serializedObject.FindProperty("ItemInto");
        repiteItemProp = serializedObject.FindProperty("repiteItem");
        repiteItemNumProp = serializedObject.FindProperty("repiteItemNum");
        AttackRateProp = serializedObject.FindProperty("AttackRate");
        DefenceRateProp = serializedObject.FindProperty("DefenceRate");
        HealthUpRateProp = serializedObject.FindProperty("HealthUpRate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(ItemIDProp);
        EditorGUILayout.PropertyField(ItemNameProp);
        EditorGUILayout.PropertyField(itemTypeProp);
        EditorGUILayout.PropertyField(ItemPictureProp);
        EditorGUILayout.PropertyField(ItemIntoProp);
        EditorGUILayout.PropertyField(repiteItemProp);
        EditorGUILayout.PropertyField(repiteItemNumProp);

        LA_Item.ItemType type = (LA_Item.ItemType)itemTypeProp.enumValueIndex;
        switch (type)
        {
            case LA_Item.ItemType.Weapon:
                EditorGUILayout.PropertyField(AttackRateProp, new GUIContent("Attack Rate"));
                break;
            case LA_Item.ItemType.armor:
                EditorGUILayout.PropertyField(DefenceRateProp, new GUIContent("Defence Rate"));
                break;
            case LA_Item.ItemType.medcine:
                EditorGUILayout.PropertyField(HealthUpRateProp, new GUIContent("Health Rate"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
