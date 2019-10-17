using UnityEngine;
using UnityEngine.UI;

public class CachedListTestItem : MonoBehaviour
{
    public void Init(int index, int totalCount)
    {
        float c = (float)index / totalCount;
        m_backgroundImage.color = new Color(c, c, c);
    }

    [SerializeField] private Image m_backgroundImage = null;
}
