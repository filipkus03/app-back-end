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

namespace app_back_end.admin
{
    /// <summary>
    /// Logika interakcji dla klasy main_admin.xaml
    /// </summary>
    public partial class main_admin : Window
    {
        private Window mainwindow;
        public main_admin(Window window = null)
        {
            mainwindow = window;
            InitializeComponent();
            Focus();
        }
        public main_admin()
        {
            InitializeComponent();
        }

        

       
    }
}
