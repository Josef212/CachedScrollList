using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;


public static class CreateCachedScrollRectEditor
{
    [MenuItem("GameObject/UI/Vertical Cached Scroll View", false, 2063)]
    public static void CreateVerticalScrollRect()
    {
        GameObject root = GetOrCreateCanvasGameObject();

        GameObject scrollGO = CreateUIObject("VerticalCachedScrollRect", root);
        scrollGO.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);

        VerticalCachedScrollRect scroll = scrollGO.AddComponent<VerticalCachedScrollRect>();
        scroll.enabled = true;
        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.horizontalScrollbar = null;

        AddSlicedImage(scrollGO, new Color(1f, 1f, 1f, 0.3921569f), GetBackgroundSprite());

        GameObject viewportGO = CreateVerticalScrollViewportGO(scrollGO);
        GameObject contentGO = CreateVerticalScrollContentGO(viewportGO);

        scroll.viewport = viewportGO.GetComponent<RectTransform>();
        scroll.content = contentGO.GetComponent<RectTransform>();

        scroll.verticalScrollbar = CreateVerticalScrollbar(scrollGO);
        scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;

        Selection.activeGameObject = scrollGO;
    }

    [MenuItem("GameObject/UI/Horizontal Cached Scroll View", false, 2064)]
    public static void CreaTeHorizontalScrollRect()
    {
        GameObject root = GetOrCreateCanvasGameObject();

        GameObject scrollGO = CreateUIObject("HorizontalCachedScrollRect", root);
        scrollGO.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);

        HorizontallCachedScrollRect scroll = scrollGO.AddComponent<HorizontallCachedScrollRect>();
        scroll.enabled = true;
        scroll.horizontal = true;
        scroll.vertical = false;
        scroll.verticalScrollbar = null;

        AddSlicedImage(scrollGO, new Color(1f, 1f, 1f, 0.3921569f), GetBackgroundSprite());

        GameObject viewportGO = CreateHorizontalScrollViewportGO(scrollGO);
        GameObject contentGO = CreateHorizontalScrollContentGO(viewportGO);

        scroll.viewport = viewportGO.GetComponent<RectTransform>();
        scroll.content = contentGO.GetComponent<RectTransform>();

        scroll.horizontalScrollbar = CreateHorizontalScrollbar(scrollGO);
        scroll.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;

        Selection.activeGameObject = scrollGO;
    }

    private static GameObject CreateVerticalScrollViewportGO(GameObject parent)
    {
        GameObject viewportGO = CreateUIObject("Viewport", parent);
        RectTransform viewportRect = viewportGO.GetComponent<RectTransform>();
        SetFullStretch(viewportRect);
        viewportRect.pivot = new Vector2(0, 1);
        viewportRect.offsetMax = new Vector2(-20, viewportRect.offsetMax.y);

        Mask mask = viewportGO.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        Image bg = viewportGO.AddComponent<Image>();
        bg.sprite = GetUIMaskSprite();
        bg.color = Color.white;
        bg.type = Image.Type.Sliced;
        bg.fillCenter = true;

        return viewportGO;
    }

    private static GameObject CreateHorizontalScrollViewportGO(GameObject parent)
    {
        GameObject viewportGO = CreateUIObject("Viewport", parent);
        RectTransform viewportRect = viewportGO.GetComponent<RectTransform>();
        SetFullStretch(viewportRect);
        viewportRect.pivot = new Vector2(0, 1);
        viewportRect.offsetMin = new Vector2(viewportRect.offsetMin.x, 20);

        Mask mask = viewportGO.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        Image bg = viewportGO.AddComponent<Image>();
        bg.sprite = GetUIMaskSprite();
        bg.color = Color.white;
        bg.type = Image.Type.Sliced;
        bg.fillCenter = true;

        return viewportGO;
    }

    private static GameObject CreateVerticalScrollContentGO(GameObject parent)
    {
        GameObject contentGO = CreateUIObject("Content", parent);
        RectTransform contentRect = contentGO.GetComponent<RectTransform>();
        SetFullStretch(contentRect);
        contentRect.pivot = new Vector2(0, 1);
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.localPosition = Vector3.zero;

        return contentGO;
    }

    private static GameObject CreateHorizontalScrollContentGO(GameObject parent)
    {
        GameObject contentGO = CreateUIObject("Content", parent);
        RectTransform contentRect = contentGO.GetComponent<RectTransform>();
        SetFullStretch(contentRect);
        contentRect.pivot = new Vector2(0, 1);
        contentRect.anchorMin = new Vector2(0, 0);
        contentRect.anchorMax = new Vector2(0, 1);
        contentRect.localPosition = Vector3.zero;

        return contentGO;
    }

    private static Scrollbar CreateVerticalScrollbar(GameObject parent)
    {
        GameObject scrollBarGO = CreateUIObject("Scrollbar Vertical", parent);

        RectTransform scrollBarRect = scrollBarGO.GetComponent<RectTransform>();
        scrollBarRect.anchorMin = new Vector2(1, 0);
        scrollBarRect.anchorMax = new Vector2(1, 1);
        scrollBarRect.pivot = new Vector2(1, 1);
        scrollBarRect.sizeDelta = new Vector2(20, 0);

        // ---

        GameObject slidingAreaGO = CreateUIObject("Sliding Area", scrollBarGO);
        SetFullStretch(slidingAreaGO.GetComponent<RectTransform>());

        // ---

        GameObject handleGO = CreateUIObject("Handle", slidingAreaGO);
        SetFullStretch(handleGO.GetComponent<RectTransform>());

        Image handleImage = AddSlicedImage(handleGO, Color.white, GetUISpriteSprite());

        // ---

        AddSlicedImage(scrollBarGO, Color.white, GetBackgroundSprite());

        Scrollbar ret = scrollBarGO.AddComponent<Scrollbar>();
        ret.targetGraphic = handleImage;
        ret.handleRect = handleGO.GetComponent<RectTransform>();
        ret.direction = Scrollbar.Direction.BottomToTop;
        ret.value = 0.1452992f;
        ret.size = 0.61f;

        return ret;
    }

    private static Scrollbar CreateHorizontalScrollbar(GameObject parent)
    {
        GameObject scrollBarGO = CreateUIObject("Scrollbar Horizontal", parent);

        RectTransform scrollbarRect = scrollBarGO.GetComponent<RectTransform>();
        scrollbarRect.pivot = new Vector2(0, 0);
        scrollbarRect.anchorMin = new Vector2(0, 0);
        scrollbarRect.anchorMax = new Vector2(1, 0);
        scrollbarRect.sizeDelta = new Vector2(0, 20);

        // ---

        GameObject slidingAreaGO = CreateUIObject("Sliding Area", scrollBarGO);
        SetFullStretch(slidingAreaGO.GetComponent<RectTransform>());

        // ---

        GameObject handleGO = CreateUIObject("Handle", slidingAreaGO);
        SetFullStretch(handleGO.GetComponent<RectTransform>());

        Image handleImage = AddSlicedImage(handleGO, Color.white, GetUISpriteSprite());

        // ---

        AddSlicedImage(scrollBarGO, Color.white, GetBackgroundSprite());

        Scrollbar ret = scrollBarGO.AddComponent<Scrollbar>();
        ret.targetGraphic = handleImage;
        ret.handleRect = handleGO.GetComponent<RectTransform>();
        ret.direction = Scrollbar.Direction.LeftToRight;
        ret.value = 0;
        ret.size = 1;

        return ret;
    }

    // Helpers ==========================================================

    private static void SetFullStretch(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    private static Image AddSlicedImage(GameObject go, Color color, Sprite sprite = null)
    {
        Image ret = go.AddComponent<Image>();
        ret.sprite = sprite;
        ret.color = color;
        ret.type = Image.Type.Sliced;
        ret.fillCenter = true;

        return ret;
    }

    private static GameObject GetOrCreateCanvasGameObject()
    {
        GameObject selectedGO = Selection.activeGameObject;

        Canvas canvas = selectedGO != null ? selectedGO.GetComponent<Canvas>() : null;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
        {
            return canvas.gameObject;
        }

        canvas = (Canvas)Object.FindObjectOfType(typeof(Canvas));
        if (canvas != null && canvas.gameObject.activeInHierarchy)
        {
            return canvas.gameObject;
        }

        return CreateCanvasGameObject();
    }

    private static GameObject CreateCanvasGameObject()
    {
        GameObject ret = new GameObject("Canvas");
        ret.layer = LayerMask.NameToLayer(s_uiLayer);

        Canvas canvas = ret.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        ret.AddComponent<CanvasScaler>();
        ret.AddComponent<GraphicRaycaster>();

        Undo.RegisterCreatedObjectUndo(ret, "Create " + ret.name);

        CreateEventSystemIfNeeded();

        return ret;
    }

    private static void CreateEventSystemIfNeeded(GameObject parent = null)
    {
        EventSystem evsys = Object.FindObjectOfType<EventSystem>();

        if (evsys == null)
        {
            GameObject evsysRoot = new GameObject("EventSystem");
            GameObjectUtility.SetParentAndAlign(evsysRoot, parent);
            evsys = evsysRoot.AddComponent<EventSystem>();
            evsysRoot.AddComponent<StandaloneInputModule>();

            Undo.RegisterCreatedObjectUndo(evsysRoot, "Create " + evsysRoot.name);
        }
    }

    private static GameObject CreateUIObject(string name, GameObject parent)
    {
        GameObject ret = new GameObject(name);
        ret.AddComponent<RectTransform>();
        GameObjectUtility.SetParentAndAlign(ret, parent);
        return ret;
    }

    // ==================================================================

    // Sprite getters ===================================================

    private static Sprite GetBackgroundSprite()
    {
        return AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
    }

    private static Sprite GetUISpriteSprite()
    {
        return AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
    }

    private static Sprite GetUIMaskSprite()
    {
        return AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
    }

    // ==================================================================

    private static string s_uiLayer = "UI";
}