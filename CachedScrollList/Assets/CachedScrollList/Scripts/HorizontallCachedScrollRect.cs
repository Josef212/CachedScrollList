using UnityEngine;

[AddComponentMenu("UI/Horizontal Cached Scroll Rect", 101)]
[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class HorizontallCachedScrollRect : BaseCachedScrollRect
{
    protected override float ItemSize => m_itemPrefab.sizeDelta.x;

    protected override int MaxItems => Mathf.CeilToInt(((RectTransform)transform).rect.width / m_itemSize);

    protected override Vector2 ItemPivot => new Vector2(0, 1);

    protected override Vector2 ResetNormalizedScrollPosition => new Vector2(0, 0);

    protected override Vector2 GetScrollNormalizedPosition(int index)
    {
        return new Vector2((float)index / m_scrollItemsCount, 0f);
    }

    protected override Vector3 GetItemResetLocalPosition(int index)
    {
        return new Vector3(index * m_itemSize, 0);
    }

    protected override void OnScroll(Vector2 v)
    {
        if (m_scrollItemsCount <= m_maxItems - c_extraElements || m_maxItems == 0)
        {
            return;
        }

        Vector3 newPos = content.localPosition;
        float diff = newPos.x - m_lastContainerPos.x;

        if (diff < 0)
        {
            // Scroll towards left

            while (newPos.x < m_topOrLeftLimit && m_scrollIndex + m_maxItems < m_scrollItemsCount)
            {
                RectTransform nextRow = GetItem(m_scrollIndex + m_activeItems);
                m_itemInitCallback?.Invoke(m_scrollIndex + m_activeItems, nextRow.gameObject);
                nextRow.gameObject.SetActive(true);

                m_scrollIndex++;

                RectTransform movingCol = GetItem(m_scrollIndex - 1);
                RectTransform prevCol = GetItem(m_scrollIndex - 2);

                movingCol.gameObject.SetActive(false);
                movingCol.localPosition = new Vector3(prevCol.localPosition.x + m_itemSize, 0);

                CalcViewLimits();
            }
        }
        else
        {
            // Scroll towards right

            while (newPos.x > m_botOrRightLimit && m_scrollIndex > 0)
            {
                m_scrollIndex--;

                RectTransform movingCol = GetItem(m_scrollIndex);
                RectTransform nextCol = GetItem(m_scrollIndex + 1);

                movingCol.localPosition = new Vector3(nextCol.localPosition.x - m_itemSize, 0);
                m_itemInitCallback?.Invoke(m_scrollIndex, movingCol.gameObject);
                movingCol.gameObject.SetActive(true);

                RectTransform prevCol = GetItem(m_scrollIndex + m_activeItems);
                prevCol.gameObject.SetActive(false);

                CalcViewLimits();
            }
        }

        m_lastContainerPos = newPos;
    }

    protected override void UpdateContainerSize()
    {
        float totalSize = m_scrollItemsCount * m_itemSize;

        Vector2 containerSize = content.sizeDelta;
        containerSize.x = totalSize;
        content.sizeDelta = containerSize;
    }

    protected override void CalcViewLimits()
    {
        m_topOrLeftLimit = -m_scrollIndex * m_itemSize - m_itemSize;
        m_botOrRightLimit = -m_scrollIndex * m_itemSize;
    }

#if UNITY_EDITOR
    protected override void ForcedOptions()
    {
        horizontal = true;
        vertical = false;
        verticalScrollbar = null;
    }
#endif
}
