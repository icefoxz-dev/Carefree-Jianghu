using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveGridLayout : IAspectRatioUpdater
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private GridLayoutConfig gridLayoutConfig;

    [Button]
    public override void UpdateAspectRatio() => AdjustLayout();
    private void AdjustLayout()
    {
        var rect = (RectTransform)gridLayoutGroup.transform;
        var width = rect.rect.width;
        var height = rect.rect.height;

        int rows, columns;

        // 计算活跃的子物件数量
        var activeChildCount = gridLayoutGroup.transform.Cast<Transform>().Count(child => child.gameObject);

        if (width > height)
        {
            // 横屏模式
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = gridLayoutConfig.rowsInLandscape;

            rows = gridLayoutGroup.constraintCount;
            columns = Mathf.CeilToInt((float)activeChildCount / rows);
        }
        else
        {
            // 竖屏模式
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = gridLayoutConfig.columnsInPortrait;

            columns = gridLayoutGroup.constraintCount;
            rows = Mathf.CeilToInt((float)activeChildCount / columns);
        }

        // 计算 Cell Size
        var cellWidth =
            (width - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right -
             (columns - 1) * gridLayoutGroup.spacing.x) / columns;
        var cellHeight =
            (height - gridLayoutGroup.padding.top - gridLayoutGroup.padding.bottom -
             (rows - 1) * gridLayoutGroup.spacing.y) / rows;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }

    [Serializable]
    private class GridLayoutConfig
    {
        public int columnsInPortrait = 2;
        public int rowsInLandscape = 2;
    }
}
