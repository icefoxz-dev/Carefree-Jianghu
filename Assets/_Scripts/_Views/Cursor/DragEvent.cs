using UnityEngine;
using UnityEngine.EventSystems;

namespace _Views.Cursor
{
    public class DragHelper
    {
        public enum DragEvent
        {
            Begin,
            Dragging,
            End
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        //public static Direction? TryGetDirection(PointerEventData p, float minDistance = 50)
        //{
        //    var delta = p.delta;
        //    var x = Mathf.Abs(delta.x);
        //    var y = Mathf.Abs(delta.y);
        //    if (x < minDistance && y < minDistance)
        //        return null;
        //    return GetDirection(p);
        //}        
        //public static Direction GetDirection(PointerEventData p)
        //{
        //    var delta = p.delta;
        //    var x = Mathf.Abs(delta.x);
        //    var y = Mathf.Abs(delta.y);
        //    if (x > y)
        //        return delta.x > 0 ? Direction.Right : Direction.Left;
        //    return delta.y > 0 ? Direction.Up : Direction.Down;
        //}
    }

}