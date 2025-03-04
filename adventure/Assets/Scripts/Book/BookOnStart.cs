using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Demo
{
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class BookOnStart : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public GameObject item;
        public int totalCount = -1;

        Stack<Transform> pool = new Stack<Transform>();
        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                return Instantiate(item);
            }
            Transform candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform transform, int idx)
        {
            List<PackageLocalItem> items = GameManager.Instance.GetSortPackageLocalData();
            PackagePanel uiParent = (PackagePanel)BookManager.Instance.GetPanel(UIConst.PackagePanel);
            transform.GetComponent<WordCell>().Refresh(items[idx], uiParent);
        }

        void Start()
        {
            RefillCells();
        }
        void OnEnable()
        {
            RefillCells();
        }
        void RefillCells()
        {
            var ls = GetComponent<LoopScrollRect>();
            ls.prefabSource = this;
            ls.dataSource = this;
            ls.totalCount = GameManager.Instance.GetSortPackageLocalData().Count;
            ls.RefillCells();
        }
        void RefreshCells()
        {
            var ls = GetComponent<LoopScrollRect>();
            ls.prefabSource = this;
            ls.dataSource = this;
            ls.totalCount = GameManager.Instance.GetSortPackageLocalData().Count;
            ls.RefreshCells();
        }
    }
}