using app_back_end.admin;
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
    /// Logika interakcji dla klasy main_kierownik.xaml
    /// </summary>
    public partial class main_kierownik : Window
    {
        private Window mainwindow;
        public main_kierownik(Window window = null)
        {
            mainwindow = window;
            InitializeComponent();
            Focus();
        }
        public main_kierownik()
        {
            InitializeComponent();
        }

       

        private void zamowienie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window Rezerwacja = new rezerwacja(this);
            Rezerwacja.Show();
            Rezerwacja.Focus();
            Visibility = Visibility.Hidden;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            // Przywracamy widoczność okna main_kierownik
            mainwindow.Visibility = Visibility.Visible;
        }
    }
}
