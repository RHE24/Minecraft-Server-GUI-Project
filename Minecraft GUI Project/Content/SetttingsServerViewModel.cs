using System;
using FirstFloor.ModernUI.Presentation;

namespace Minecraft_GUI_Project.Content
{
    public class SetttingsServerViewModel : NotifyPropertyChanged
    {
        private String _diretory;

        public SetttingsServerViewModel()
        {
            Diretory = Properties.Settings.Default.WorkingDiretory;
        }

        public String Diretory
        {
            get { return _diretory; }
            set
            {
                OnPropertyChanged("Diretory");
                _diretory = value;
                Properties.Settings.Default.WorkingDiretory = value;
                Properties.Settings.Default.Save();
            } 
        }
    }
}
