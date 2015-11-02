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
using System.Windows.Shapes;

namespace _2_ViewMapEditor
{
    /// <summary>
    /// Interaction logic for MapDimensions.xaml
    /// </summary>
    public partial class MapDimensions : Window
    {

        public MapDimensions()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hoogte = Convert.ToInt32(txbHoogte.Text);
                Breedte = Convert.ToInt32(txbBreedte.Text);
                this.Close();
            }
            catch (ArgumentOutOfRangeException excep)
            {
                MessageBox.Show(excep.Message);
            }
            catch (FormatException excep)
            {
                MessageBox.Show("vul alleen gehele getallen boven 0 in:\n"+excep.Message);
            }
            
            //TODO: controleer ingegeven waarden 
            
            
        }
        private int hoogte;
        
        public int Hoogte
        {
            get { return hoogte; }
            set {
                if(value >= 1)
                {
                    hoogte = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Hoogte moet groter zijn dan 1");
                }
               

            }
        }
        private int breedte;

        public int Breedte
        {
            get { return breedte; }
            set {

                if(value >= 1)
                {
                    breedte = value;
                }else
                {
                    throw new ArgumentOutOfRangeException("Breedte moet groter zijn dan 1");
                }
                

            }
        }

        
    }
}
