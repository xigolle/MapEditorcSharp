using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace _2_ViewMapEditor
{
    class CustomTextbox:TextBox
    {
        public string placeHolderText { get; set; }
        public CustomTextbox()
        {
            
            this.Loaded += CustomTextbox_Loaded;

        }

        private void CustomTextbox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Foreground = new SolidColorBrush(Colors.LightGray);
            this.Text = placeHolderText;
            
            this.GotFocus += CustomTextbox_GotFocus;
            this.LostFocus += CustomTextbox_LostFocus;
        }

        private void CustomTextbox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = placeHolderText;
                tb.Foreground = new SolidColorBrush(Colors.LightGray);
            }

            
        }
        public void clearHelperText()
        {
            if(this.Text == placeHolderText)
            this.Text = "";

        }

        private void CustomTextbox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == placeHolderText)
            {
                tb.Text = "";
                tb.Foreground = new SolidColorBrush(Colors.Black);
            }

           
        }
    }
}
