using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_ViewMapEditor
{
    class Queue
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int OriginalValue { get; set; }
        public Map currentMap;

        public Queue(Map map)
        {
            currentMap = map;
        }
        public Queue(int x,int y,int t, Map map)
        {
            X = x;
            Y = y;
            OriginalValue = t;
            currentMap = map;
        }
        public Queue(int x, int y,int t)
        {
            X = x;
            Y = y;
            OriginalValue = t;
        }

        public void QueueTask()
        {
            Queue q = new Queue(X,Y,OriginalValue);
            currentMap.QueueList.Enqueue(q);
        }
        public void ClearQueueList()
        {
            while (currentMap.QueueList.Count > 0)
            {
               currentMap.QueueList.Dequeue();
                
                
                
            }
        }
        public void UnQueueAndDrawList()
        {
            while (currentMap.QueueList.Count > 0)
            {
                Queue temp = currentMap.QueueList.Dequeue();
                currentMap.CurrentMap.SetElement(temp.X, temp.Y, temp.OriginalValue);

            }
            currentMap.DrawMap();
        }

    }
}
