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

namespace app_back_end
{
    /// <summary>
    /// Logika interakcji dla klasy main_mechanic.xaml
    /// </summary>
    public partial class main_mechanic : Window
    {
        private Window mainwindow;
        public main_mechanic(Window window = null)
        {
            mainwindow = window;
            InitializeComponent();
            Focus();
        }
        public main_mechanic()
        {
            InitializeComponent();
        }
    }
}
