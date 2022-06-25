using System;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace ET
{
    public class GridScroll :  MonoBehaviour, IEnhancedScrollerDelegate
    {
        public EnhancedScroller scroller;
        public int number = 10;
        public int rowNum = 5;
        public CellView cellViewPrefab;
        public float cellSize = 40;
        
        public Action<ListCellView> onRenderCellView;
        public Action<int,CellView> onRenderSingleView;
        public Action<int,CellView> onRecyleSingleView;

        #region self Register

        public void JumpToDataIndex(int dataIndex, EnhancedScroller.TweenType tweenType, float time)
        {
            int cellIndex = (int)Math.Ceiling((double)(dataIndex / this.rowNum));
            scroller.JumpToDataIndex(cellIndex,0,0,false, tweenType, time);
        }
        public void JumpTCellIndex(int cellIndex, EnhancedScroller.TweenType tweenType, float time)
        {
            scroller.JumpToDataIndex(cellIndex,0,0,false, tweenType, time);
        }

        public void Refresh()
        {
            this.scroller.RefreshActiveCellViews();
        }

        public void Reload()
        {
            this.scroller.ReloadData();
        }

        public void Init()
        {
            this.onRenderCellView = null;
            this.onRecyleSingleView = null;
            this.onRecyleSingleView = null;
        }

        #endregion
        public void Awake()
        {
            this.Init();
            scroller.Delegate = this;
            scroller.cellViewWillRecycle = (listcell) =>
            {
                ListCellView listCellView = (ListCellView)listcell;
                // 回收单个物体
                for (int i = 0; i < listCellView.cellViews.Count; i++)
                {
                    int index = i * listCellView.dataIndex + i;
                    onRecyleSingleView?.Invoke(index, listCellView.cellViews[i]);
                }
            };
        }

        public void OnDestroy()
        {
            this.Init();
        }

        #region EnhancedScroll Register
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)number / (float)rowNum);
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return cellSize;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            ListCellView listCellView = scroller.GetCellView(cellViewPrefab) as ListCellView;

            listCellView.name = "ListCell " + (dataIndex * this.rowNum).ToString() + " to " + ((dataIndex * rowNum) + rowNum - 1).ToString();
            onRenderCellView?.Invoke(listCellView);
            // 渲染单个物体
            for (int i = 0; i < listCellView.cellViews.Count; i++)
            {
                int index = i * listCellView.dataIndex + i;
                onRenderSingleView?.Invoke(index, listCellView.cellViews[i]);
            }

            return listCellView;
        }
        #endregion
        
    }
}