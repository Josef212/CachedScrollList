using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class BaseCachedScrollRect : ScrollRect
{
    protected abstract float ItemSize { get; }

    protected abstract int MaxItems { get; }

    protected abstract Vector2 ItemPivot { get; }

    protected abstract Vector2 ResetNormalizedScrollPosition { get; }

    protected abstract Vector2 GetScrollNormalizedPosition(int index);

    protected abstract Vector3 GetItemResetLocalPosition(int index);

    // ===============================================================

    protected abstract void OnScroll(Vector2 v);

    protected abstract void UpdateContainerSize();

    protected abstract void CalcViewLimits();

    // ===============================================================

    public void BuildScrollList()
    {
        int maxItems = MaxItems;

        if (maxItems == m_maxItems - 1)
        {
            return;
        }

        m_maxItems = maxItems + 1;
        m_lastContainerPos = content.localPosition;

        for (int i = 0; i < m_maxItems + c_extraElements; ++i)
        {
            RectTransform itemRect = null;
            if (i >= m_items.Count)
            {
                itemRect = UnityEngine.Object.Instantiate(m_itemPrefab, content);
                itemRect.gameObject.name = itemRect.gameObject.name + " - " + i;
                itemRect.pivot = ItemPivot;
                m_items.Add(itemRect);
            }
            else
            {
                itemRect = m_items[i];
            }

            itemRect.gameObject.SetActive(false);
            itemRect.localPosition = GetItemResetLocalPosition(i);
        }

        if (m_items.Count > m_maxItems)
        {
            for (int i = m_maxItems; i < m_items.Count; ++i)
            {
                m_items[i].gameObject.SetActive(false);
            }
        }

        UpdateContainerSize();
        CalcViewLimits();
    }

    public void SetRows(int numItems, int offset = 0)
    {
        m_activeItems = 0;
        m_scrollItemsCount = numItems;

        for (int i = offset; i < m_maxItems; ++i)
        {
            if (i < numItems)
            {
                m_items[i].gameObject.SetActive(true);
                m_itemInitCallback?.Invoke(i, m_items[i].gameObject);
                m_activeItems++;
            }
            else
            {
                m_items[i].gameObject.SetActive(false);
            }
        }

        UpdateContainerSize();
        CalcViewLimits();
    }

    public void ClearRows()
    {
        m_activeItems = 0;
        m_scrollItemsCount = 0;
        m_scrollIndex = 0;
        normalizedPosition = ResetNormalizedScrollPosition;

        for (int i = 0; i < m_items.Count; ++i)
        {
            m_items[i].gameObject.SetActive(false);
            m_items[i].localPosition = GetItemResetLocalPosition(i);
        }
    }

    public void SetScrollPosition(int index)
    {
        if (index >= 0 && index < m_scrollItemsCount)
        {
            normalizedPosition = GetScrollNormalizedPosition(index);
        }
    }

    // ===============================================================

    protected int GetListIndex(int itemIndex)
    {
        return (int)Mathf.Repeat(itemIndex, m_items.Count);
    }

    protected RectTransform GetItem(int itemIndex)
    {
        return m_items[GetListIndex(itemIndex)];
    }

    // ===============================================================

    protected override void Awake()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        m_items = new List<RectTransform>();
        m_itemSize = ItemSize;

        onValueChanged.AddListener(OnScroll);

        BuildScrollList();
    }

#if UNITY_EDITOR
    protected abstract void ForcedOptions();

    protected override void Reset()
    {
        ForcedOptions();
    }

    protected override void OnValidate()
    {
        ForcedOptions();
    }
#endif

    [Header("Cached Scroll Rect")]
    [SerializeField] protected RectTransform m_itemPrefab = null;
    [SerializeField] protected CachedScrollInitItemEvent m_itemInitCallback = null;

    protected List<RectTransform> m_items = null;
    protected float m_topOrLeftLimit = 0f, m_botOrRightLimit = 0f;

    protected int m_scrollItemsCount = 0;
    protected int m_activeItems = 0;
    protected int m_maxItems = 0; // The amount of items that fit visible on the scroll rect plus one for margin
    protected int m_scrollIndex = 0;

    protected float m_itemSize = 0f;
    protected Vector3 m_lastContainerPos = Vector3.zero;

    protected const int c_extraElements = 4;

    [System.Serializable] public class CachedScrollInitItemEvent : UnityEvent<int, GameObject> { }

#if DEBUG_GUI_MODE && UNITY_EDITOR
    private static GUIStyle m_style = null;
    private static GUIStyle m_constStyle = null;
    private static GUIStyle m_backStyle = null;

    protected void OnGUI()
    {
        if (m_style == null) m_style = new GUIStyle() { fontSize = 20, normal = new GUIStyleState() { textColor = Color.white } };
        if (m_constStyle == null) m_constStyle = new GUIStyle() { fontSize = 20, normal = new GUIStyleState() { textColor = Color.yellow } };
        if (m_backStyle == null) m_backStyle = new GUIStyle() { border = new RectOffset(1, 1, 1, 1), normal = new GUIStyleState() { background = null } };

        GUILayout.BeginArea(new Rect(20.0f, 150.0f, 600.0f, 500.0f), m_backStyle);
        GUILayout.BeginVertical();

        GUILayout.Label("Current index: " + m_scrollIndex, m_style);
        GUILayout.Label("Current row: -" + GetItem(m_scrollIndex).gameObject.name + "-", m_style);
        GUILayout.Label("Limit - TopOrLeft: " + m_topOrLeftLimit, m_style);
        GUILayout.Label("Limit - BotOrRight: " + m_botOrRightLimit, m_style);
        GUILayout.Label("Last container pos: " + m_lastContainerPos, m_style);
        GUILayout.Label("Content local pos: " + content.localPosition, m_style);

        GUILayout.Label("=======================", m_style);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scroll items count: " + m_scrollItemsCount, m_constStyle);
        GUILayout.Label("Active rows: " + m_activeItems, m_constStyle);
        GUILayout.Label("Max. items: " + m_maxItems, m_constStyle);
        GUILayout.EndHorizontal();

        GUILayout.Label("Item size: " + m_itemSize, m_constStyle);

        GUILayout.Label("=======================", m_style);

        GUILayout.Label($"List count: -{m_items.Count}-", m_constStyle);
        GUILayout.Label($"Above -{m_scrollIndex - 1}- Scroll index -{m_scrollIndex}- Below -{m_scrollIndex + 1}-", m_style);
        GUILayout.Label($"Above -{GetListIndex(m_scrollIndex - 1)}- Scroll index -{GetListIndex(m_scrollIndex)}- Below -{GetListIndex(m_scrollIndex + 1)}-", m_style);
        GUILayout.Label($"Above -{GetItem(m_scrollIndex - 1).gameObject.name}\n- Scroll index -{GetItem(m_scrollIndex).gameObject.name}\n- Below -{GetItem(m_scrollIndex + 1).gameObject.name}-", m_style);

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
#endif
}
