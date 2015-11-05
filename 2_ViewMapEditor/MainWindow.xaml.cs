using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        Block singleBlock;
        RedoUndo undomodel = new RedoUndo();// model to call every undo action


        public MainWindow()
        {

            InitializeComponent();
            map = new Map(0, 0, mapCanvas);
            undomodel.currentMap = map;
            singleBlock = new Block();
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
                map.MapCanvas.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate
                {
                    map.RenderMap();
                });

            }


        }

        private void menuLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                map.CurrentMapPath = dialog.FileName;
                map.CurrentMap = new MapModel(map.CurrentMapPath);
                map.RenderMap();


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
                map.RenderMap();
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

            if (cmbBrush.SelectedIndex > -1)
            {
                if (queueCheckbox.IsChecked == true)
                {
                    map.QueueChecked = true;
                }
                else
                {
                    map.QueueChecked = false;
                }
                Point click = e.MouseDevice.GetPosition(mapCanvas);
                string t = (cmbBrush.SelectedItem as ComboBoxItem).Content.ToString();

                int x = (int)((click.X / map.BlockScale));
                int y = (int)((click.Y / map.BlockScale));

                if (DrawRectangleCheckbox.IsChecked == true)
                {
                    customRectangle rectangle = new customRectangle();
                    rectangle.X = x;
                    rectangle.Y = y;
                    try
                    {
                        rectangle.Width = Convert.ToInt32(newRectangleBreedte.Text);
                        rectangle.Height = Convert.ToInt32(newRectangleHoogte.Text);
                    }
                    catch (FormatException)
                    {

                        MessageBox.Show("Alleen nummers mogen ingevuld worden");
                        newRectangleBreedte.Text = newRectangleBreedte.placeHolderText;
                        
                        newRectangleHoogte.Text = newRectangleHoogte.placeHolderText;
                    }
                    catch (Exception excp)
                    {
                        MessageBox.Show(excp.ToString());
                    }

                    rectangle.TypeBlock = Convert.ToInt32(t);

                    map.DrawOnMap(rectangle, click);
                }
                else
                {
                    singleBlock.X = x;
                    singleBlock.Y = y;
                    singleBlock.TypeBlock = Convert.ToInt32(t);

                    map.DrawOnMap(singleBlock, click);
                }


            }
        }

        private void CreateRandomMap_Click(object sender, RoutedEventArgs e)
        {
            if (map.CreateNewMap)
            {

                try
                {
                    map.NewMapBreedte = Convert.ToInt32(newMapBreedte.Text);
                    map.NewMapHoogte = Convert.ToInt32(newMapHoogte.Text);
                }
                catch (FormatException)
                {

                    MessageBox.Show("Alleen nummers mogen ingevuld worden");
                    newMapBreedte.Text = newMapBreedte.placeHolderText;

                    newMapHoogte.Text = newMapHoogte.placeHolderText;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

            }

            int parseValue;
            if (!int.TryParse(minTypeTextBox.Text, out parseValue) && !int.TryParse(minTypeTextBox.Text, out parseValue))

            {
                //the input is not a number. This could be the helper text or a wrong input
                map.RenderRandomMap();
            }
            else
            {
                int type = Convert.ToInt32(minTypeTextBox.Text);
                int amount = Convert.ToInt32(minAmountTextBox.Text);
                map.RenderRandomMap(type, amount);
            }


        }

        private void mapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (cmbBrush.SelectedIndex > -1 && e.LeftButton == MouseButtonState.Pressed)
            {
                if (queueCheckbox.IsChecked == true)
                {
                    map.QueueChecked = true;
                }
                else
                {
                    map.QueueChecked = false;
                }
                Point click = e.MouseDevice.GetPosition(mapCanvas);
                string t = (cmbBrush.SelectedItem as ComboBoxItem).Content.ToString();

                int x = (int)((click.X / map.BlockScale));
                int y = (int)((click.Y / map.BlockScale));

                if (DrawRectangleCheckbox.IsChecked == true)
                {
                    customRectangle rectangle = new customRectangle();
                    rectangle.X = x;
                    rectangle.Y = y;
                    try
                    {
                        rectangle.Width = Convert.ToInt32(newRectangleBreedte.Text);
                        rectangle.Height = Convert.ToInt32(newRectangleHoogte.Text);
                    }
                    catch (FormatException)
                    {

                        MessageBox.Show("Alleen nummers mogen ingevuld worden");
                        newRectangleBreedte.Text = newRectangleBreedte.placeHolderText;

                        newRectangleHoogte.Text = newRectangleHoogte.placeHolderText;
                    }
                    catch (Exception excp)
                    {
                        MessageBox.Show(excp.ToString());
                    }

                    rectangle.TypeBlock = Convert.ToInt32(t);

                    map.DrawOnMap(rectangle, click);
                }
                else
                {
                    singleBlock.X = x;
                    singleBlock.Y = y;
                    singleBlock.TypeBlock = Convert.ToInt32(t);

                    map.DrawOnMap(singleBlock, click);
                }




            }
        }

        private void hideToolbox_Click(object sender, RoutedEventArgs e)
        {
            if (collapsed)
            {

                Toolbox.Visibility = Visibility.Visible;

                
                rowToolbox.Width = new GridLength(1, GridUnitType.Star);
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
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            map.RenderMap();
        }

        #endregion

        #region on change event
        private void slidesScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (map != null)
            {
                map.BlockScale = (int)e.NewValue;
                map.RenderMap();
            }

        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Queue tempQueue = new Queue(map);
            MessageBoxResult result = MessageBox.Show("Yes: Voer alle opgeslagen opdrachten uit \nNo: Alle opgeslagen opdrachten worden verwijderd \nCancel: Ga verder met het bewerken van uw wachtrij", "opgeslagen opdrachten uitvoeren", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                tempQueue.UnQueueAndDrawList();

            }
            if (result == MessageBoxResult.No)
            {
                tempQueue.ClearQueueList();
            }
            if (result == MessageBoxResult.Cancel)
            {
                queueCheckbox.IsChecked = true;
            }
        }

        private void TypeMapRadioButton_Checked(object sender, RoutedEventArgs e)
        {

            RadioButton ck = (RadioButton)sender;
            if (ck.Content.ToString() == "Use current map")
            {
                if (map != null)
                {
                    map.CreateNewMap = false;
                }

            }
            else if (ck.Content.ToString() == "Create new map")
            {
                map.CreateNewMap = true;






            }


        }









        #endregion



        

        
    }
}
