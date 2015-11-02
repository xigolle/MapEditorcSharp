using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_ViewMapEditor
{
    class ActionManager
    {
        
        CanvasManager cm;

        public ActionManager()
        {
            if(UndoHistory == null && RedoHistory == null)
            {
                UndoHistory = new Stack<Undo>();
                RedoHistory = new Stack<Undo>();
                
            }
            
            
        }

        #region properties

        private Stack<Undo> undoHistory;

        public Stack<Undo> UndoHistory
        {
            get { return undoHistory; }
            set { undoHistory = value; }
        }

        private Stack<Undo> redoHistory;

        public Stack<Undo> RedoHistory
        {
            get { return redoHistory; }
            set { redoHistory = value; }
        }

        


        #endregion

        public void redo()
        {

        }

        public void undo(MapModel currentMap)
        {
            if(cm == null)
            {
               
            }
            
            if (UndoHistory.Count > 0)
            {
                Undo lastaction = UndoHistory.Pop();
                RedoHistory.Push(lastaction);
                currentMap.SetElement(lastaction.X, lastaction.Y, lastaction.OriginalValue);

                cm.RedrawMap();
            }
        }

    }
}
