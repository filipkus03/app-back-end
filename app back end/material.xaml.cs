using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace app_back_end
{
    public partial class material : Window
    {
        private main_kierownik mainKierownikWindow;
        private ObservableCollection<Material> materials = new ObservableCollection<Material>();
        private DBConnection dbConnection = DBConnection.Instance;
        private SqlDataReader dataReader;
        private SqlConnection sqlConnection;

        public material(main_kierownik mainKierownikWindow)
        {
            InitializeComponent();
            this.mainKierownikWindow = mainKierownikWindow;
            MaterialsDataGrid.DataContext = this;
            LoadMaterialsFromDB();
        }

        public material()
        {
            InitializeComponent();
            MaterialsDataGrid.DataContext = this;
            LoadMaterialsFromDB();
        }

        private void LoadMaterialsFromDB()
        {
            try
            {
                using (var connection = dbConnection.Connect())
                {
                    string query = "SELECT Id_czesci, Nazwa, Marka, Miejsce, Stan, Wolne, Cena FROM Tbl_Material";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                materials.Add(new Material
                                {
                                    Id_czesci = dataReader.GetInt32(0),
                                    Nazwa = dataReader.GetString(1),
                                    Marka = dataReader.GetString(2),
                                    Miejsce = dataReader.GetString(3),
                                    Stan = dataReader.GetInt32(4),
                                    Wolne = dataReader.GetInt32(5),
                                    Cena = dataReader.GetDecimal(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania materiałów: {ex.Message}");
            }
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            if (mainKierownikWindow != null)
            {
                mainKierownikWindow.Visibility = Visibility.Visible;
            }
        }

        public ObservableCollection<Material> Materials
        {
            get { return materials; }
            set
            {
                materials = value;
                OnPropertyChanged(nameof(Materials));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
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
