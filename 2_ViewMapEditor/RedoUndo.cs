using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_ViewMapEditor
{
    class RedoUndo
    {
        #region public property

        public int X { get; set; }
        public int Y { get; set; }
        public int OriginalValue { get; set; }
        public int typeBlock { get; set; }
        public Map currentMap { get; set; }

        #endregion

        #region public methods

        public void redoAction()
        {
            if (currentMap.RedoHistory.Count > 0)
            {
                RedoUndo redoAction = currentMap.RedoHistory.Pop();
                currentMap.UndoHistory.Push(redoAction);
                currentMap.CurrentMap.SetElement(redoAction.X, redoAction.Y, redoAction.typeBlock);
                currentMap.DrawMap();
            }
        }

        public void undoAction()
        {
            if (currentMap.UndoHistory.Count > 0)
            {
                RedoUndo lastaction = currentMap.UndoHistory.Pop();
                currentMap.RedoHistory.Push(lastaction);
                currentMap.CurrentMap.SetElement(lastaction.X, lastaction.Y, lastaction.OriginalValue);

                currentMap.DrawMap();
            }
        }

        
        #endregion

    }
}
