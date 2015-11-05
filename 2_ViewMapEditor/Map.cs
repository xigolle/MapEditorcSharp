using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private bool queueChecked = false;
        private bool createNewMap = false;
        private int newMapBreedte;
        private int newMapHoogte;

        public int NewMapHoogte
        {
            get { return newMapHoogte; }
            set { newMapHoogte = value; }
        }

        public int NewMapBreedte
        {
            get { return newMapBreedte; }
            set { newMapBreedte = value; }
        }




        public bool CreateNewMap
        {
            get { return createNewMap; }
            set { createNewMap = value; }
        }




        public bool MapExists
        {
            get
            {
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


        public bool QueueChecked
        {
            get { return queueChecked; }
            set { queueChecked = value; }
        }

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

        public Map(int breedte, int hoogte, Canvas canvas)
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

        public void RenderMap()
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

        public void DrawOnMap(Block blok, Point click)
        {
            if (QueueChecked)
            {

                //checkbox is checked
                if (CurrentMap.GetElement(blok.X, blok.Y) != Convert.ToInt32(blok.TypeBlock))
                {

                    RedoUndo newAction = new RedoUndo(blok, this);
                    UndoHistory.Push(newAction);
                    Queue tempQueue = new Queue(blok.X, blok.Y, Convert.ToInt32(blok.TypeBlock), this);
                    tempQueue.QueueTask();

                }
            }
            else
            {
                if (CurrentMap.GetElement(blok.X, blok.Y) != Convert.ToInt32(blok.TypeBlock))
                {

                    RedoUndo newAction = new RedoUndo(blok, this);
                    UndoHistory.Push(newAction);
                    CurrentMap.SetElement(blok.X, blok.Y, Convert.ToInt32(blok.TypeBlock));
                    //if ((int)click.X % BlockScale == 0 || (int)click.Y % BlockScale == 0)
                    //{


                    RenderMap();


                    //}
                }

            }
        }
        public void DrawOnMap(customRectangle rectangle, Point click)
        {
            int defaultY = rectangle.Y;
            if (QueueChecked)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    for (int y = 0; y < rectangle.Height; y++)
                    {
                        RedoUndo newAction = new RedoUndo(rectangle, this);
                        undoHistory.Push(newAction);
                        Queue tempQueue = new Queue(rectangle.X, rectangle.Y, Convert.ToInt32(rectangle.TypeBlock), this);
                        tempQueue.QueueTask();
                        rectangle.Y += 1;
                    }
                    rectangle.Y = defaultY;
                    rectangle.X += 1;
                }

            }
            else
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    for (int y = 0; y < rectangle.Height; y++)
                    {
                        RedoUndo newAction = new RedoUndo(rectangle, this);
                        undoHistory.Push(newAction);
                        currentMap.SetElement(rectangle.X, rectangle.Y, Convert.ToInt32(rectangle.TypeBlock));
                        RenderMap();
                        rectangle.Y += 1;
                    }
                    rectangle.Y = defaultY;
                    rectangle.X += 1;

                }


            }
        }

        public void RenderRandomMap()
        {
            Random r = new Random();
            if (createNewMap)
            {

                currentMap = new MapModel(NewMapBreedte, NewMapHoogte);
            }
            int type = r.Next(0, 4);
            for (int x = 0; x < currentMap.Breedte; x++)
            {
                for (int y = 0; y < currentMap.Hoogte; y++)
                {

                    currentMap.SetElement(x, y, type);
                    type = r.Next(0, 4);
                }
            }

            this.RenderMap();


        }
        public void RenderRandomMap(int typeBlock, int amount)
        {
            Random r = new Random();
            if (createNewMap)
            {

                currentMap = new MapModel(NewMapBreedte, NewMapHoogte);
            }
            int type = r.Next(0, 4);
            if (amount > (currentMap.Breedte * currentMap.Hoogte))
            {
                amount = currentMap.Breedte * currentMap.Hoogte;
            }
            while (amount > 0)
            {
                for (int x = 0; x < currentMap.Breedte; x++)
                {
                    for (int y = 0; y < currentMap.Hoogte; y++)
                    {
                        if (type == typeBlock && currentMap.GetElement(x, y) != type)
                        {
                            int current = currentMap.GetElement(x, y);
                            currentMap.SetElement(x, y, type);
                            amount--;



                        }
                        else
                        {
                            if (currentMap.GetElement(x, y) != typeBlock)
                            {
                                currentMap.SetElement(x, y, type);
                            }

                        }

                        type = r.Next(0, 4);

                    }
                }
            }


            this.RenderMap();
        }
        #endregion

    }
}
