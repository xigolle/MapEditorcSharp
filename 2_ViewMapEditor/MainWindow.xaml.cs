using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2_ViewMapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MapModel currentMap;
        private string currentMapPath = "";
        private bool collapsed = false;
        private Stack<Undo> UndoHistory = new Stack<Undo>();
        private Stack<Undo> RedoHistory = new Stack<Undo>();

        public MainWindow()
        {
            InitializeComponent();
            currentMap =  new MapModel(0, 0);
            
            cmbBrush.SelectedIndex = 0;
         

        }

        #region IO stuff
        
        private void  menuNew_Click(object sender, RoutedEventArgs e)
        {

            
            
            if (!(currentMap.Breedte == 0 && currentMap.Hoogte == 0))
            {
                saveMapWarning();
                
            }
            else
            {
                MapDimensions askdims = new MapDimensions();
                askdims.ShowDialog();
                currentMap = new MapModel(askdims.Breedte, askdims.Hoogte);
                LoadMapOnView();
            }
            

        }

        private void menuLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                currentMapPath = dialog.FileName;
                currentMap = new MapModel(currentMapPath);

                LoadMapOnView();
            }

        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {

            saveMap();

        }


        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                currentMapPath = dialog.FileName;

                currentMap.SaveMap(currentMapPath);

            }
        }

        #endregion

        private void saveMapWarning()
        {
            MessageBoxResult result = MessageBox.Show("Er is een actieve map wilt u deze eerste opslaan?", "Opslaan huidige map", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                saveMap();
                MapDimensions askdims = new MapDimensions();
                askdims.ShowDialog();
                currentMap = new MapModel(askdims.Breedte, askdims.Hoogte);
                LoadMapOnView();
            }
            if (result == MessageBoxResult.No)
            {

                MapDimensions askdims = new MapDimensions();
                askdims.ShowDialog();
                currentMap = new MapModel(askdims.Breedte, askdims.Hoogte);
                LoadMapOnView();

            }
            if (result == MessageBoxResult.Cancel)
            {

            }
        }

        private void saveMap()
        {
            if (currentMapPath == "")
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    currentMapPath = dialog.FileName;

                }
            }
            if (currentMapPath != "")
            {
                currentMap.SaveMap(currentMapPath);
            }
                
        }
        
        private void menuClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Dit zal uw huidige kaart volledig reseten.Bent u zeker?", "Map resetten!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                currentMap.ClearMap();
                LoadMapOnView();
            }
        }

        private int blokscale = 15;

        private void LoadMapOnView()
        {
            if (currentMap != null)
            {
                mapCanvas.Children.Clear();
                mapCanvas.Width = (currentMap.Breedte * blokscale);
                mapCanvas.Height = (currentMap.Hoogte * blokscale);
                for (int i = 0; i < currentMap.Hoogte; i++)
                {
                    for (int j = 0; j < currentMap.Breedte; j++)
                    {
                        Rectangle blok = new Rectangle();
                        blok.Stroke = new SolidColorBrush(Colors.Black);
                        blok.StrokeThickness = 0.3;
                        blok.Width = blokscale;
                        blok.Height = blokscale;
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
                        blok.SetValue(Canvas.LeftProperty, (double)(blokscale * j));
                        blok.SetValue(Canvas.TopProperty, (double)(blokscale * i));

                        mapCanvas.Children.Add(blok);
                    }
                }
            }
        }

        private void mapCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Bereken blokcoordinataat:
            if (cmbBrush.SelectedIndex > -1)
            {
                Point click = e.MouseDevice.GetPosition(mapCanvas);
                int x = (int)((click.X / blokscale));
                int y = (int)((click.Y / blokscale));
                var t = (cmbBrush.SelectedItem as ComboBoxItem).Content.ToString();
                Undo newAction = new Undo() { X = x, Y = y, OriginalValue = (int)currentMap.GetElement(x, y) ,typeBlock = Convert.ToInt32(t) };
                UndoHistory.Push(newAction);
                currentMap.SetElement(x, y, Convert.ToInt32(t));
                LoadMapOnView();
            }
        }

        private void slidesScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            blokscale = (int)e.NewValue;
            LoadMapOnView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMapOnView();
        }

        private void menuView_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hideToolbox_Click(object sender, RoutedEventArgs e)
        {
            if (collapsed)
            {
                
                Toolbox.Visibility = Visibility.Visible;

                //TODO: code moet aangepast worden dat hij weer terug naar asterisk(*) moet gaan
                Toolbox.Width = 5;
                collapsed = false;
            }
            else
            {
                rowToolbox.Width = GridLength.Auto;
                Toolbox.Visibility = Visibility.Collapsed;
                collapsed = true;
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!(currentMap.Breedte == 0 && currentMap.Hoogte == 0))
            {

                MessageBoxResult result = MessageBox.Show("Er is een actieve map wilt u deze eerste opslaan?", "Opslaan huidige map", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    saveMap();
                    currentMapPath = "";
                }
                if (result == MessageBoxResult.No)
                {

                }
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                
            }
        }

        private void redoAction()
        {
            if (RedoHistory.Count > 0)
            {
                Undo redoAction = RedoHistory.Pop();
                UndoHistory.Push(redoAction);
                currentMap.SetElement(redoAction.X, redoAction.Y, redoAction.typeBlock);
                LoadMapOnView();
            }
           
        }

        private void undoAction()
        {
            if (UndoHistory.Count > 0)
            {
                Undo lastaction = UndoHistory.Pop();
                RedoHistory.Push(lastaction);
                currentMap.SetElement(lastaction.X, lastaction.Y, lastaction.OriginalValue);
                
                LoadMapOnView();
            }
        }

        

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)||e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Z:
                        undoAction();
                        break;
                    case Key.Y:
                        redoAction();
                        break;
                }
               
            }
        }

       

        private void menuUndo_Click(object sender, RoutedEventArgs e)
        {
            undoAction();
        }

        private void menuRedo_Click(object sender, RoutedEventArgs e)
        {
            redoAction();
        }
    }
}
