using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2_ViewMapEditor
{
    class Map
    {

        #region private property

        private MapModel currentMap;
        private int blockScale = 15;
        private string currentMapPath = "";
        private Canvas mapCanvas;
        private Stack<RedoUndo> undoHistory;
        private Stack<RedoUndo> redoHistory;
        private Queue<Queue> queueList;
        private bool mapExists;

        public bool MapExists
        {
            get {
                if (currentMap.Breedte == 0 && currentMap.Hoogte == 0)
                {
                    //there doesnt exist a valid map or it is default map
                    mapExists = false;
                    return mapExists;
                }
                else
                {
                    mapExists = true;
                    return mapExists;
                }
            }
            
        }

        #endregion

        #region public propery

        public Queue<Queue> QueueList
        {
            get { return queueList; }
            set { queueList = value; }
        }


        public Stack<RedoUndo> RedoHistory
        {
            get { return redoHistory; }
            set { redoHistory = value; }
        }


        public Stack<RedoUndo> UndoHistory
        {
            get { return undoHistory; }
            set { undoHistory = value; }
        }

        public Canvas MapCanvas
        {
            get { return mapCanvas; }
            set { mapCanvas = value; }
        }



        public string CurrentMapPath
        {
            get { return currentMapPath; }
            set { currentMapPath = value; }
        }


        public int BlockScale
        {
            get { return blockScale; }
            set { blockScale = value; }
        }

        public MapModel CurrentMap
        {
            get { return currentMap; }
            set { currentMap = value; }
        }
        #endregion

        #region Constructor

        public Map(int breedte, int hoogte,Canvas canvas)
        {
            CurrentMap = new MapModel(breedte, hoogte);
            MapCanvas = canvas;
            QueueList = new Queue<Queue>();
            UndoHistory = new Stack<RedoUndo>();
            RedoHistory = new Stack<RedoUndo>();

        }

        #endregion

        #region private Methods
        private void LoadMapOnView()
        {
            if (currentMap != null)
            {
                mapCanvas.Children.Clear();
                mapCanvas.Width = (currentMap.Breedte * blockScale);
                mapCanvas.Height = (currentMap.Hoogte * blockScale);
                for (int i = 0; i < currentMap.Hoogte; i++)
                {
                    for (int j = 0; j < currentMap.Breedte; j++)
                    {
                        Rectangle blok = new Rectangle();
                        blok.Stroke = new SolidColorBrush(Colors.Black);
                        blok.StrokeThickness = 0.3;
                        blok.Width = blockScale;
                        blok.Height = blockScale;
                        switch (currentMap.GetElement(j, i))
                        {
                            case 0:
                                blok.Fill = new SolidColorBrush(Colors.LightGray);
                                break;
                            case 1:
                                blok.Fill = new SolidColorBrush(Colors.Red);
                                break;
                            case 2:
                                blok.Fill = new SolidColorBrush(Colors.Green);
                                break;
                            case 3:
                                blok.Fill = new SolidColorBrush(Colors.Orange);
                                break;
                            case 4:
                                blok.Fill = new SolidColorBrush(Colors.Yellow);
                                break;
                            default:
                                blok.Fill = new SolidColorBrush(Colors.Black);
                                break;

                        }
                        blok.SetValue(Canvas.LeftProperty, (double)(blockScale * j));
                        blok.SetValue(Canvas.TopProperty, (double)(blockScale * i));

                        mapCanvas.Children.Add(blok);
                    }
                }
            }
        }
        #endregion

        #region public Methods 

        public void DrawMap()
        {
            LoadMapOnView();
        }

        public void SaveMap()
        {
            if (CurrentMapPath == "")
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    CurrentMapPath = dialog.FileName;

                }
            }
            if (CurrentMapPath != "")
            {
                CurrentMap.SaveMap(CurrentMapPath);
            }
        }
       

        #endregion

    }
}
