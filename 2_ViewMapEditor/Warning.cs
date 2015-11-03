using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2_ViewMapEditor
{
    class Warning
    {
        private string messageText = "default message text";
        private string messageTitle = "default title text";
        private MessageBoxButton messageButton = MessageBoxButton.OK;
        private MessageBoxImage messageImage = MessageBoxImage.None;
        private MessageBoxResult result;
        private Map currentMap;

        public Warning()
        {
            result = MessageBox.Show(messageText, messageTitle, messageButton, messageImage);
        }
        public Warning(string text)
        {
            messageText = text;
            result = MessageBox.Show(messageText, messageTitle, messageButton, messageImage);
        }
        public Warning(string text, string title)
        {
            messageText = text;
            messageTitle = title;
            result = MessageBox.Show(messageText, messageTitle, messageButton, messageImage);
        }
        public Warning(string text, string title, MessageBoxButton button, MessageBoxImage image)
        {
            messageTitle = title;
            messageText = text;
            messageImage = image;
            messageButton = button;
            result = MessageBox.Show(messageText, messageTitle, messageButton, messageImage);
        }
        public Map CurrentMap
        {
            get { return currentMap; }
            set { currentMap = value; }
        }

        public MessageBoxImage MessageImage
        {
            get { return messageImage; }
            set { messageImage = value; }
        }

        public MessageBoxButton MessageButton
        {
            get { return messageButton; }
            set { messageButton = value; }
        }


        public string MessageTitle
        {
            get { return messageTitle; }
            set { messageTitle = value; }
        }

        public string MessageText
        {
            get { return messageText; }
            set { messageText = value; }
        }

        

        public void SaveNewMapWarning()
        {
            if (result == MessageBoxResult.Yes)
            {
                currentMap.SaveMap();
                MapDimensions askdims = new MapDimensions();
                askdims.ShowDialog();
                currentMap.CurrentMap = new MapModel(askdims.Breedte, askdims.Hoogte);

                currentMap.DrawMap();
            }
            if (result == MessageBoxResult.No)
            {

                MapDimensions askdims = new MapDimensions();
                askdims.ShowDialog();
                currentMap.CurrentMap = new MapModel(askdims.Breedte, askdims.Hoogte);

                currentMap.DrawMap();

            }
            if (result == MessageBoxResult.Cancel)
            {

            }
        }

        public bool SaveCloseAppWarning()
        {
            if (result == MessageBoxResult.Yes)
            {
                currentMap.SaveMap();
                currentMap.CurrentMapPath = "";
                return false;
            }
            if (result == MessageBoxResult.No)
            {
                return false;
            }
            if (result == MessageBoxResult.Cancel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
