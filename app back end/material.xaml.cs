using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace app_back_end
{
    
    public partial class material : Window
    {
        private main_kierownik mainKierownikWindow;
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Desktop\app back end\app back end\data\db_local.mdf;Integrated Security=True";

        public material(main_kierownik mainKierownikWindow)
        {
            InitializeComponent();
            this.mainKierownikWindow = mainKierownikWindow;
            LoadMaterials();
        }

        public material()
        {
            InitializeComponent();
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            List<Material> materials = new List<Material>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id_czesci, Nazwa, Marka, Miejsce, Stan, Wolne, Cena FROM Tbl_Material";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            materials.Add(new Material
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

            MaterialsDataGrid.ItemsSource = materials;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Przywracamy widoczność okna main_kierownik
            if (mainKierownikWindow != null)
            {
                mainKierownikWindow.Visibility = Visibility.Visible;
            }
        }
    }

    public class Material
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
