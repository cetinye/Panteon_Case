using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace StrategyGameDemo.UI
{
    public class InfiniteScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform content;
        [SerializeField] private List<GameObject> itemPrefabs = new List<GameObject>();
        [SerializeField] private int totalItems;
        [SerializeField] private int bufferCount = 2;

        private GridLayoutGroup gridLayout;
        private float cellHeight, rowHeight, viewportHeight;
        private int numRows, numItems;
        private bool isUpdating;

        private void Awake()
        {
            scrollRect.onValueChanged.AddListener(OnScrollChanged);
        }

        private void Start()
        {
            gridLayout = content.GetComponent<GridLayoutGroup>();
            cellHeight = gridLayout.cellSize.y;
            rowHeight = cellHeight + gridLayout.spacing.y;
            viewportHeight = scrollRect.viewport.rect.height;
            int visibleRows = Mathf.CeilToInt(viewportHeight / rowHeight);
            numRows = visibleRows + bufferCount * 2;
            numItems = numRows * 2;
            for (int i = 0; i < numItems; i++)
                Instantiate(itemPrefabs[i % itemPrefabs.Count], content);
            
            Initialize(28);
        }

        public void Initialize(int itemCount)
        {
            totalItems = itemCount;
            float totalRows = Mathf.Ceil(totalItems / 2f);
            float contentHeight = totalRows * rowHeight;
            content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);
        }

        private void OnScrollChanged(Vector2 normalizedPos)
        {
            if (isUpdating) return;
            isUpdating = true;
            float scrollOffset = content.anchoredPosition.y;
            int rowsMoved = Mathf.FloorToInt(scrollOffset / rowHeight);
            if (rowsMoved != 0)
            {
                for (int i = 0; i < Mathf.Abs(rowsMoved) * 2; i++)
                {
                    if (rowsMoved > 0)
                    {
                        content.GetChild(0).SetAsLastSibling();
                    }
                    else
                    {
                        Transform last = content.GetChild(content.childCount - 1);
                        last.SetSiblingIndex(0);
                    }
                }
                content.anchoredPosition -= new Vector2(0, rowsMoved * rowHeight);
            }
            isUpdating = false;
        }

        private void OnDestroy()
        {
            scrollRect.onValueChanged.RemoveListener(OnScrollChanged);
        }
    }
}