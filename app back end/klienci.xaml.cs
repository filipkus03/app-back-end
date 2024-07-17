using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace app_back_end
{
    public partial class klienci : Window
    {
        private main_kierownik mainKierownikWindow;
        private List<KlientInfo> klienciList;

        public klienci(main_kierownik mainKierownikWindow)
        {
            InitializeComponent();
            this.mainKierownikWindow = mainKierownikWindow;
            LoadKlienci();
        }

        public klienci()
        {
            InitializeComponent();
            LoadKlienci();
        }

        private void LoadKlienci()
        {
            klienciList = new List<KlientInfo>();

            using (SqlConnection connection = DBConnection.Instance.Connect())
            {
                string query = "SELECT Id_Zlecenia, Imie_Klienta, Nazwisko_Klienta, Data, Kwota, godzina_od, godzina_do, praca_wykonana FROM Tbl_Klient";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            klienciList.Add(new KlientInfo
                            {
                                Id_Zlecenia = reader.GetInt32(0),
                                Imie_Klienta = reader.GetString(1),
                                Nazwisko_Klienta = reader.GetString(2),
                                Data = reader.GetDateTime(3),
                                Kwota = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                                godzina_od = reader.GetTimeSpan(5),
                                godzina_do = reader.GetTimeSpan(6),
                                praca_wykonana = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                            });
                        }
                    }
                }
            }

            KlienciDataGrid.ItemsSource = klienciList;
        }

        private void DodajKlienta_Click(object sender, RoutedEventArgs e)
        {
            int idZlecenia;
            string imieKlienta = ImieKlientaTextBox.Text;
            string nazwiskoKlienta = NazwiskoKlientaTextBox.Text;
            DateTime data = DataPicker.SelectedDate ?? DateTime.Now;
            decimal kwota;
            TimeSpan godzina_od, godzina_do;
            string praca_wykonana = PracaWykonanaTextBox.Text;

            if (string.IsNullOrEmpty(imieKlienta) || string.IsNullOrEmpty(nazwiskoKlienta) || !int.TryParse(IdZleceniaTextBox.Text, out idZlecenia) ||
                !decimal.TryParse(KwotaTextBox.Text, out kwota) || !TryParseTimeSpan(GodzinaOdTextBox.Text, out godzina_od) ||
                !TryParseTimeSpan(GodzinaDoTextBox.Text, out godzina_do))
            {
                MessageBox.Show("Wprowadź poprawne dane.");
                return;
            }

            // Normalizacja TimeSpan do zakresu akceptowalnego przez SqlDbType.Time
            godzina_od = NormalizeTimeSpan(godzina_od);
            godzina_do = NormalizeTimeSpan(godzina_do);

            using (SqlConnection connection = DBConnection.Instance.Connect())
            {
                string query = "INSERT INTO Tbl_Klient (Id_Zlecenia, Imie_Klienta, Nazwisko_Klienta, Data, Kwota, godzina_od, godzina_do, praca_wykonana) " +
                               "VALUES (@Id_Zlecenia, @Imie_Klienta, @Nazwisko_Klienta, @Data, @Kwota, @godzina_od, @godzina_do, @praca_wykonana)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_Zlecenia", idZlecenia);
                    command.Parameters.AddWithValue("@Imie_Klienta", imieKlienta);
                    command.Parameters.AddWithValue("@Nazwisko_Klienta", nazwiskoKlienta);
                    command.Parameters.AddWithValue("@Data", data);
                    command.Parameters.AddWithValue("@Kwota", kwota);
                    command.Parameters.AddWithValue("@godzina_od", godzina_od);
                    command.Parameters.AddWithValue("@godzina_do", godzina_do);
                    command.Parameters.AddWithValue("@praca_wykonana", praca_wykonana);
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Klient został dodany.");

            // Odśwież dane
            LoadKlienci();
        }

        // Metoda pomocnicza do parsowania TimeSpan
        private bool TryParseTimeSpan(string input, out TimeSpan result)
        {
            if (TimeSpan.TryParse(input, out result))
            {
                return true;
            }

            if (int.TryParse(input, out int hours))
            {
                result = new TimeSpan(hours, 0, 0);
                return true;
            }

            result = default;
            return false;
        }

        // Normalizacja TimeSpan do zakresu 0-24 godziny
        private TimeSpan NormalizeTimeSpan(TimeSpan timeSpan)
        {
            while (timeSpan.TotalHours >= 24)
            {
                timeSpan = timeSpan.Subtract(TimeSpan.FromHours(24));
            }
            return timeSpan;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (mainKierownikWindow != null)
            {
                mainKierownikWindow.Visibility = Visibility.Visible;
            }
        }
    }

    public class KlientInfo
    {
        public int Id_Zlecenia { get; set; }
        public string Imie_Klienta { get; set; }
        public string Nazwisko_Klienta { get; set; }
        public DateTime Data { get; set; }
        public decimal Kwota { get; set; }
        public TimeSpan godzina_od { get; set; }
        public TimeSpan godzina_do { get; set; }
        public string praca_wykonana { get; set; }
    }
}
