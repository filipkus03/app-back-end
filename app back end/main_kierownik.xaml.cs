using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace app_back_end
{
    public partial class main_kierownik : Window
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Desktop\app back end\app back end\data\db_local.mdf;Integrated Security=True";
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
            LoadKlientData();
        }

        private void LoadKlientData()
        {
            DataTable klientData = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id_Zlecenia, Imie_Klienta, Nazwisko_Klienta, Data, Kwota, godzina_od, godzina_do, praca_wykonana FROM Tbl_Klient WHERE Data = @Data";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Data", DateTime.Today);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(klientData);
                    }
                }
            }

            dataGrid.ItemsSource = klientData.DefaultView;
        }

        private void zamowienie_Click(object sender, RoutedEventArgs e)
        {
            Window Zamowienie = new zamowienie(this);
            Zamowienie.Show();
            Zamowienie.Focus();
            Visibility = Visibility.Hidden;
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
            mainwindow.Visibility = Visibility.Visible;
        }

        private void pracownicy_Click(object sender, RoutedEventArgs e)
        {
            Window Pracownicy = new pracownicy(this);
            Pracownicy.Show();
            Pracownicy.Focus();
            Visibility = Visibility.Hidden;
        }

        private void klienci_Click(object sender, RoutedEventArgs e)
        {
            Window Klienci = new klienci(this);
            Klienci.Show();
            Klienci.Focus();
            Visibility = Visibility.Hidden;
        }

        private void material_Click(object sender, RoutedEventArgs e)
        {
            Window Material = new material(this);
            Material.Show();
            Material.Focus();
            Visibility = Visibility.Hidden;
        }
    }
}
