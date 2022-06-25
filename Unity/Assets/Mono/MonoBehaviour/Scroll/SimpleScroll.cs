using System;
using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class SimpleScroll : MonoBehaviour, IEnhancedScrollerDelegate
    {
        [HideInInspector]
        public Action<EnhancedScrollerCellView> onRecyleSingleView = default;
        [HideInInspector]
        public Action<CellView> onRenderSingleView;
        
        public EnhancedScroller scroller;
        public int number = 10;
        public CellView cellViewPrefab;
        public float itemSize = 40;

        #region selfFunc

        public void JumpToDataIndex(int dataIndex, EnhancedScroller.TweenType tweenType, float time)
        {
            scroller.JumpToDataIndex(dataIndex,0,0,false, tweenType, time);
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
            this.onRecyleSingleView = null;
            this.onRenderSingleView = null;
        }
        #endregion

        #region Unity Function
        public void Start()
        {
            this.scroller.Delegate = this;
            this.scroller.cellViewWillRecycle = (cellView) =>
            {
                this.onRecyleSingleView?.Invoke(cellView);
            };
        }
        #endregion

        #region EnhancedScrollRegister
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return this.number;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return this.itemSize;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            CellView cellView = scroller.GetCellView(cellViewPrefab) as CellView;
            cellView.name = "Cell " + dataIndex.ToString();
            this.onRenderSingleView?.Invoke(cellView);
            return cellView;
        }
        #endregion
    }
}