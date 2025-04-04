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
        
        private Queue<GameObject> objectPool = new Queue<GameObject>();
        private List<GameObject> activeItems = new List<GameObject>();
        private Dictionary<GameObject, int> itemIndices = new Dictionary<GameObject, int>();

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

            InitializePool();
            Initialize(28);
        }

        private void InitializePool()
        {
            for (int i = 0; i < numItems; i++)
            {
                GameObject item = Instantiate(itemPrefabs[i % itemPrefabs.Count], content);
                item.SetActive(false);
                objectPool.Enqueue(item);
                itemIndices[item] = i;
            }
        }

        public void Initialize(int itemCount)
        {
            totalItems = itemCount;
            float totalRows = Mathf.Ceil(totalItems / 2f);
            float contentHeight = totalRows * rowHeight;
            content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);

            ReturnAllToPool();
            for (int i = 0; i < Mathf.Min(numItems, totalItems); i++)
            {
                GameObject item = GetFromPool();
                itemIndices[item] = i;
                item.SetActive(true);
                activeItems.Add(item);
            }
        }

        private GameObject GetFromPool()
        {
            if (objectPool.Count > 0)
            {
                return objectPool.Dequeue();
            }
            
            GameObject newItem = Instantiate(itemPrefabs[activeItems.Count % itemPrefabs.Count], content);
            itemIndices[newItem] = activeItems.Count;
            return newItem;
        }

        private void ReturnToPool(GameObject item)
        {
            item.SetActive(false);
            objectPool.Enqueue(item);
            activeItems.Remove(item);
        }

        private void ReturnAllToPool()
        {
            foreach (GameObject item in activeItems.ToArray())
            {
                ReturnToPool(item);
            }
        }

        private void OnScrollChanged(Vector2 normalizedPos)
        {
            if (isUpdating) return;
            isUpdating = true;
            float scrollOffset = content.anchoredPosition.y;
            int rowsMoved = Mathf.FloorToInt(scrollOffset / rowHeight);

            if (rowsMoved != 0)
            {
                int itemsToMove = Mathf.Abs(rowsMoved) * 2;
                int direction = rowsMoved > 0 ? 1 : -1;
                int startIndex = direction > 0 ? 0 : activeItems.Count - 1;

                for (int i = 0; i < itemsToMove; i++)
                {
                    if (direction > 0)
                    {
                        if (activeItems.Count > 0)
                        {
                            GameObject itemToMove = activeItems[0];
                            int newIndex = itemIndices[itemToMove] + numItems;
                            if (newIndex < totalItems)
                            {
                                ReturnToPool(itemToMove);
                                GameObject newItem = GetFromPool();
                                newItem.SetActive(true);
                                activeItems.Add(newItem);
                                itemIndices[newItem] = newIndex;
                                newItem.transform.SetAsLastSibling();
                            }
                        }
                    }
                    else
                    {
                        if (activeItems.Count > 0)
                        {
                            GameObject itemToMove = activeItems[^1];
                            int newIndex = itemIndices[itemToMove] - numItems;
                            if (newIndex >= 0)
                            {
                                ReturnToPool(itemToMove);
                                GameObject newItem = GetFromPool();
                                newItem.SetActive(true);
                                activeItems.Insert(0, newItem);
                                itemIndices[newItem] = newIndex;
                                newItem.transform.SetAsFirstSibling();
                            }
                        }
                    }
                }
                content.anchoredPosition -= new Vector2(0, rowsMoved * rowHeight);
            }
            isUpdating = false;
        }

        private void OnDestroy()
        {
            scrollRect.onValueChanged.RemoveListener(OnScrollChanged);
            ReturnAllToPool();
        }
    }
}