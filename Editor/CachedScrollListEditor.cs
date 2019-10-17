using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(VerticalCachedScrollRect))]
public class VerticalCachedScrollRectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CachedScrollEditorHelper.CreateTemplateItemCheck((BaseCachedScrollRect)target, CachedScrollEditorHelper.CachedScrollType.Vertical);
    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(HorizontallCachedScrollRect))]
public class HorizontalCachedScrollRectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CachedScrollEditorHelper.CreateTemplateItemCheck((BaseCachedScrollRect)target, CachedScrollEditorHelper.CachedScrollType.Horizontal);
    }
}

internal static class CachedScrollEditorHelper
{
    internal enum CachedScrollType { Vertical, Horizontal };

    public static void CreateTemplateItemCheck(BaseCachedScrollRect target, CachedScrollType type)
    {
        if (GUILayout.Button("Create template item"))
        {
            GameObject item = new GameObject("TemplateItem");
            RectTransform itemRect = item.AddComponent<RectTransform>();
            GameObjectUtility.SetParentAndAlign(item, target.content.gameObject);

            itemRect.anchorMin = GetTemplateAnchorMin(type);
            itemRect.anchorMax = GetTemplateAnchorMax(type);
            itemRect.pivot = new Vector2(0, 1);

            itemRect.offsetMin = Vector2.zero;
            itemRect.offsetMax = Vector2.zero;

            itemRect.sizeDelta = GetTemplateSizeDelta(type);

            Selection.activeGameObject = item;
        }
    }

    private static Vector2 GetTemplateAnchorMin(CachedScrollType type)
    {
        return type == CachedScrollType.Vertical ? new Vector2(0, 1) : new Vector2(0, 0);
    }

    private static Vector2 GetTemplateAnchorMax(CachedScrollType type)
    {
        return type == CachedScrollType.Vertical ? new Vector2(1, 1) : new Vector2(0, 1);
    }

    private static Vector2 GetTemplateSizeDelta(CachedScrollType type)
    {
        return type == CachedScrollType.Vertical ? new Vector2(0, 100) : new Vector2(100, 0);
    }
}