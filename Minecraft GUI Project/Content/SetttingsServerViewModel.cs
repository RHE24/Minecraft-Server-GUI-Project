using System;
using FirstFloor.ModernUI.Presentation;

namespace Minecraft_GUI_Project.Content
{
    public class SetttingsServerViewModel : NotifyPropertyChanged
    {
        private String _diretory;
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

        private String _jarParam;

        public String JarParam
        {
            get { return _jarParam; }
            set
            {
                OnPropertyChanged("JarParam");
                _jarParam = value;
                Properties.Settings.Default.JarParam = value;
                Properties.Settings.Default.Save();
            }
        }

        private String _javaParam;

        public String JavaParam
        {
            get { return _javaParam; }
            set
            {
                OnPropertyChanged("JavaParam");
                _javaParam = value;
                Properties.Settings.Default.JavaParam = value;
                Properties.Settings.Default.Save();
            }
        }

        public SetttingsServerViewModel()
        {
            Diretory = Properties.Settings.Default.WorkingDiretory;
            JarParam = Properties.Settings.Default.JarParam;
            JavaParam = Properties.Settings.Default.JavaParam;
        }

    }
}
