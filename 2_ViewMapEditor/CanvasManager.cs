using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2_ViewMapEditor
{
    class CanvasManager
    {
        #region private Variables

        
        private int blockScale;
        private Canvas mapCanvas;
        private MapModel mapModel;

        #endregion

        #region Public variables
        ActionManager am = new ActionManager();


        public int BlockScale
        {
            get { return blockScale; }
            set { blockScale = value; }
        }
        public MapModel Mapmodel
        {
            get { return mapModel; }
        }






        #endregion

        #region constructors

        public CanvasManager(MapModel CurrentMap,Canvas canvas, int Blockscale )
        {
            mapModel = CurrentMap;
            mapCanvas = canvas;
            BlockScale = Blockscale;
        }
        public CanvasManager(MapModel CurrentMap, Canvas canvas)
        {
            mapModel = CurrentMap;
            mapCanvas = canvas;
            
        }
        public CanvasManager(MapModel CurrentMap)
        {
            mapModel = CurrentMap;
        }

        #endregion

        #region public methods


        public void RedrawMap(MapModel newMap)
        {

            mapModel = newMap;
            LoadMapOnView();
        }

        public void RedrawMap()
        {
            LoadMapOnView();
        }

        public void DrawOnMap(MouseButtonEventArgs e,object sender,ComboBox cmbBrush)
        {

               
                Point click = e.MouseDevice.GetPosition(mapCanvas);
                int breedte = (int)((click.X / BlockScale));
                int hoogte = (int)((click.Y / BlockScale));
                string typeBrush = (cmbBrush.SelectedItem as ComboBoxItem).Content.ToString();
                Undo newAction = new Undo() { X = breedte, Y = hoogte, OriginalValue = (int)mapModel.GetElement(breedte, hoogte), typeBlock = Convert.ToInt32(typeBrush) };
            am.UndoHistory.Push(newAction);
                //UndoHistory.Push(newAction);
                mapModel.SetElement(breedte, hoogte, Convert.ToInt32(typeBrush));
                RedrawMap();
            
        }

        #endregion

        #region private methods
        
        private void LoadMapOnView()
        {
            if (mapModel != null)
            {
                mapCanvas.Children.Clear();
                mapCanvas.Width = (mapModel.Breedte * blockScale);
                mapCanvas.Height = (mapModel.Hoogte * blockScale);
                for (int i = 0; i < mapModel.Hoogte; i++)
                {
                    for (int j = 0; j < mapModel.Breedte; j++)
                    {
                        Rectangle blok = new Rectangle();
                        blok.Stroke = new SolidColorBrush(Colors.Black);
                        blok.StrokeThickness = 0.3;
                        blok.Width = blockScale;
                        blok.Height = blockScale;
                        switch (mapModel.GetElement(j, i))
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

    }
}
