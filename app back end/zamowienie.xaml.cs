using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace app_back_end
{
    public partial class zamowienie : Window
    {
        private main_kierownik mainKierownikWindow;
        private DBConnection dbConnection = DBConnection.Instance;

        public zamowienie(main_kierownik mainKierownikWindow)
        {
            InitializeComponent();
            this.mainKierownikWindow = mainKierownikWindow;
            LoadMaterials();
        }

        public zamowienie()
        {
            InitializeComponent();
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            List<MaterialInfo> materials = new List<MaterialInfo>();

            using (SqlConnection connection = dbConnection.Connect())
            {
                string query = "SELECT Id_czesci, Nazwa, Marka, Miejsce, Stan, Wolne, Cena FROM Tbl_Material";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            materials.Add(new MaterialInfo
                            {
                                Id_czesci = reader.GetInt32(0),
                                Nazwa = reader.GetString(1),
                                Marka = reader.GetString(2),
                                Miejsce = reader.GetString(3),
                                Stan = reader.GetInt32(4),
                                Wolne = reader.GetInt32(5),
                                Cena = reader.GetDecimal(6)
                            });
                        }
                    }
                }
            }

            MaterialDataGrid.ItemsSource = materials;
        }

        private void DodajMaterial_Click(object sender, RoutedEventArgs e)
        {
            int idCzesci;
            string nazwa = NazwaTextBox.Text;
            string marka = MarkaTextBox.Text;
            string miejsce = MiejsceTextBox.Text;
            int stan, wolne;
            decimal cena;

            if (string.IsNullOrEmpty(nazwa) || string.IsNullOrEmpty(marka) || string.IsNullOrEmpty(miejsce) ||
                !int.TryParse(IdCzesciTextBox.Text, out idCzesci) || !int.TryParse(StanTextBox.Text, out stan) ||
                !int.TryParse(WolneTextBox.Text, out wolne) || !decimal.TryParse(CenaTextBox.Text, out cena))
            {
                MessageBox.Show("Wprowadź poprawne dane.");
                return;
            }

            if (IsPartAlreadyAdded(idCzesci))
            {
                MessageBox.Show("Część o podanym ID już istnieje w bazie danych.");
                return;
            }

            using (SqlConnection connection = dbConnection.Connect())
            {
                string query = "INSERT INTO Tbl_Material (Id_czesci, Nazwa, Marka, Miejsce, Stan, Wolne, Cena) VALUES (@Id_czesci, @Nazwa, @Marka, @Miejsce, @Stan, @Wolne, @Cena)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_czesci", idCzesci);
                    command.Parameters.AddWithValue("@Nazwa", nazwa);
                    command.Parameters.AddWithValue("@Marka", marka);
                    command.Parameters.AddWithValue("@Miejsce", miejsce);
                    command.Parameters.AddWithValue("@Stan", stan);
                    command.Parameters.AddWithValue("@Wolne", wolne);
                    command.Parameters.AddWithValue("@Cena", cena);
                    command.ExecuteNonQuery();
                }
            }

            LoadMaterials(); // Odśwież dane
        }

        private bool IsPartAlreadyAdded(int idCzesci)
        {
            using (SqlConnection connection = dbConnection.Connect())
            {
                string query = "SELECT COUNT(*) FROM Tbl_Material WHERE Id_czesci = @Id_czesci";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_czesci", idCzesci);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (mainKierownikWindow != null)
            {
                mainKierownikWindow.Visibility = Visibility.Visible;
            }
        }
    }

    public class MaterialInfo
    {
        public int Id_czesci { get; set; }
        public string Nazwa { get; set; }
        public string Marka { get; set; }
        public string Miejsce { get; set; }
        public int Stan { get; set; }
        public int Wolne { get; set; }
        public decimal Cena { get; set; }
    }
}
