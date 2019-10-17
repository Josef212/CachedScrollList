using UnityEngine;

[AddComponentMenu("UI/Vertical Cached Scroll Rect", 100)]
[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class VerticalCachedScrollRect : BaseCachedScrollRect
{
    protected override float ItemSize => m_itemPrefab.sizeDelta.y;

    protected override int MaxItems => Mathf.CeilToInt(((RectTransform)transform).rect.height / m_itemSize);

    protected override Vector2 ItemPivot => new Vector2(0, 1);

    protected override Vector2 ResetNormalizedScrollPosition => new Vector2(0, 1f);

    protected override Vector2 GetScrollNormalizedPosition(int index)
    {
        return new Vector2(0, 1f - (float)index / m_scrollItemsCount);
    }

    protected override Vector3 GetItemResetLocalPosition(int index)
    {
        return new Vector3(0, -index * m_itemSize);
    }

    protected override void OnScroll(Vector2 v)
    {
        if (m_scrollItemsCount <= m_maxItems - c_extraElements || m_maxItems == 0)
        {
            return;
        }

        Vector3 newPos = content.localPosition;
        float diff = newPos.y - m_lastContainerPos.y;

        if (diff > 0)
        {
            // Scroll towards top

            while (newPos.y > m_topOrLeftLimit && m_scrollIndex + m_maxItems < m_scrollItemsCount)
            {
                // Doing this at first since I need the "old" scrollIndex value. Could do it later and use minus one.
                RectTransform nextRow = GetItem(m_scrollIndex + m_activeItems);
                m_itemInitCallback?.Invoke(m_scrollIndex + m_activeItems, nextRow.gameObject);
                nextRow.gameObject.SetActive(true);

                m_scrollIndex++;

                RectTransform movingRow = GetItem(m_scrollIndex - 1);
                RectTransform prevRow = GetItem(m_scrollIndex - 2);

                movingRow.gameObject.SetActive(false);
                movingRow.localPosition = new Vector3(0, prevRow.localPosition.y - m_itemSize);

                CalcViewLimits();
            }
        }
        else
        {
            // Scroll towards bot

            while (newPos.y < m_botOrRightLimit && m_scrollIndex > 0)
            {
                m_scrollIndex--;

                RectTransform movingRow = GetItem(m_scrollIndex);
                RectTransform nextRow = GetItem(m_scrollIndex + 1);

                movingRow.localPosition = new Vector3(0, nextRow.localPosition.y + m_itemSize);
                m_itemInitCallback?.Invoke(m_scrollIndex, movingRow.gameObject);
                movingRow.gameObject.SetActive(true);

                RectTransform prevRow = GetItem(m_scrollIndex + m_activeItems);
                prevRow.gameObject.SetActive(false);

                CalcViewLimits();
            }
        }

        m_lastContainerPos = newPos;
    }

    protected override void UpdateContainerSize()
    {
        float totalSize = m_scrollItemsCount * m_itemSize;

        Vector2 containerSize = content.sizeDelta;
        containerSize.y = totalSize;
        content.sizeDelta = containerSize;
    }

    protected override void CalcViewLimits()
    {
        m_topOrLeftLimit = m_scrollIndex * m_itemSize + m_itemSize;
        m_botOrRightLimit = m_scrollIndex * m_itemSize;
    }

#if UNITY_EDITOR
    protected override void ForcedOptions()
    {
        horizontal = false;
        vertical = true;
        horizontalScrollbar = null;
    }
#endif
}
