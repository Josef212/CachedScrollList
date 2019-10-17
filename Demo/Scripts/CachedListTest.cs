using UnityEngine;

public class CachedListTest : MonoBehaviour
{
    public void SetItems()
    {
        m_cachedList?.SetRows(m_itemsCount);
    }

    public void Clear()
    {
        m_cachedList?.ClearRows();
    }

    public void InitCallback(int index, GameObject item)
    {
        item?.GetComponent<CachedListTestItem>()?.Init(index, m_itemsCount);
    }

    [SerializeField] private BaseCachedScrollRect m_cachedList = null;

    [Space(10)]
    [Range(0, 1_000)] [SerializeField] private int m_itemsCount = 10;
}
