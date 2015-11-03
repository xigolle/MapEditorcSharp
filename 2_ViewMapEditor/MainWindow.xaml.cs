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

        //private MapModel currentMap;
        //private string currentMapPath = "";
        private bool collapsed = false;
        Map map;
        Undo undomodel = new Undo();// model to call every undo action


        public MainWindow()
        {

            InitializeComponent();
            map = new Map(0, 0, mapCanvas);
            undomodel.currentMap = map;
            //currentMap = new MapModel(0, 0);

            cmbBrush.SelectedIndex = 0;


        }

        #region Mouse Events

        private void menuNew_Click(object sender, RoutedEventArgs e)
        {
            if (map.MapExists)
            {
                Warning warning = new Warning("Er is een actieve map wilt u deze eerste opslaan?", "Opslaan huidige map", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                warning.CurrentMap = map;
                warning.SaveNewMapWarning();

            }
            else
            {
                MapDimensions askdims = new MapDimensions();
                askdims.ShowDialog();
                map.CurrentMap = new MapModel(askdims.Breedte, askdims.Hoogte);

                map.DrawMap();
            }


        }

        private void menuLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                map.CurrentMapPath = dialog.FileName;
                map.CurrentMap = new MapModel(map.CurrentMapPath);
                map.DrawMap();


            }

        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {

            map.SaveMap();

        }


        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                map.CurrentMapPath = dialog.FileName;

                map.CurrentMap.SaveMap(map.CurrentMapPath);

            }
        }

        private void menuClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Dit zal uw huidige kaart volledig reseten.Bent u zeker?", "Map resetten!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                map.CurrentMap.ClearMap();
                map.DrawMap();
            }
        }

        private void menuUndo_Click(object sender, RoutedEventArgs e)
        {
            undomodel.undoAction();
        }

        private void menuRedo_Click(object sender, RoutedEventArgs e)
        {
            undomodel.redoAction();
        }

        private void menuView_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mapCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Bereken blokcoordinataat:
            if (cmbBrush.SelectedIndex > -1)
            {
                Point click = e.MouseDevice.GetPosition(mapCanvas);
                int x = (int)((click.X / map.BlockScale));
                int y = (int)((click.Y / map.BlockScale));
                var t = (cmbBrush.SelectedItem as ComboBoxItem).Content.ToString();
                if (queueCheckbox.IsChecked == false)
                {
                    Undo newAction = new Undo() { X = x, Y = y, OriginalValue = (int)map.CurrentMap.GetElement(x, y), typeBlock = Convert.ToInt32(t) };
                    map.UndoHistory.Push(newAction);
                    map.CurrentMap.SetElement(x, y, Convert.ToInt32(t));
                    map.DrawMap();

                }
                else
                {
                    Queue tempQueue = new Queue(x, y, Convert.ToInt32(t), map);
                    tempQueue.QueueTask();
                }
            }
        }

        private void mapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (cmbBrush.SelectedIndex > -1 && e.LeftButton == MouseButtonState.Pressed)
            {
                string t = (cmbBrush.SelectedItem as ComboBoxItem).Content.ToString();
                Point click = e.MouseDevice.GetPosition(mapCanvas);
                int breedte = (int)((click.X / map.BlockScale));
                int hoogte = (int)((click.Y / map.BlockScale));
                if (map.CurrentMap.GetElement(breedte, hoogte) != Convert.ToInt32(t))
                {
                    map.CurrentMap.SetElement(breedte, hoogte, Convert.ToInt32(t));
                }
                Undo newAction = new Undo() { X = breedte, Y = hoogte, OriginalValue = (int)map.CurrentMap.GetElement(breedte, hoogte), typeBlock = Convert.ToInt32(t) };
                map.UndoHistory.Push(newAction);
                if ((int)click.X % map.BlockScale == 0 || (int)click.Y % map.BlockScale == 0)
                {


                    map.DrawMap();


                }
            }
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

        #endregion

        

        #region Keyboard events
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Z:
                        undomodel.undoAction();
                        break;
                    case Key.Y:
                        undomodel.redoAction();
                        break;
                }

            }
        }
        #endregion

        #region window event

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (map.MapExists)
            {
                Warning warning = new Warning("Er is een actieve map wilt u deze eerste opslaan?", "Opslaan huidige map", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                warning.CurrentMap = map;
                e.Cancel = warning.SaveCloseAppWarning();


            }
            else
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            map.DrawMap();
        }

        #endregion

        #region on change event
        private void slidesScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (map != null)
            {
                map.BlockScale = (int)e.NewValue;
                map.DrawMap();
            }

        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Queue tempQueue = new Queue(map);
            MessageBoxResult result = MessageBox.Show("Yes: Voer alle opgeslagen opdrachten uit /n No: Alle opgeslagen opdrachten worden verwijderd /n Cancel: Ga verder met het bewerken van uw wachtrij", "opgeslagen opdrachten uitvoeren", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                
                tempQueue.UnQueueAndDrawList();

            }
            if(result == MessageBoxResult.No)
            {
                tempQueue.ClearQueueList();
            }
            if (result == MessageBoxResult.Cancel)
            {
                queueCheckbox.IsChecked = true;
            }
        }
        #endregion










    }
}
