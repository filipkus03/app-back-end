using app_back_end.admin;
using app_back_end.magazyn;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace app_back_end
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Text;

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Desktop\app back end\app back end\data\db_local.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Role FROM Tbl_Logins WHERE Login = @Login AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            string role = reader["Role"].ToString();

                            // Zalogowano pomyślnie, otwórz odpowiednie okno w zależności od roli
                            switch (role)
                            {
                                case "admin":
                                    Window AdminWindow = new main_admin(this);
                                    AdminWindow.Show();
                                    AdminWindow.Focus();
                                    break;
                                case "kierownik":
                                    Window KierownikWindow = new main_kierownik(this);
                                    KierownikWindow.Show();
                                    KierownikWindow.Focus();
                                    break;
                                case "magazyn":
                                    Window MagazynWindow = new main_magazyn(this);
                                    MagazynWindow.Show();
                                    MagazynWindow.Focus();
                                    break;
                                case "mechanik":
                                    
                                    Window MechanicWindow = new main_mechanic(this);
                                    MechanicWindow.Show();
                                    MechanicWindow.Focus();
                                    break;
                            }

                            // Ukryj okno logowania
                            Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            // Nieprawidłowe dane logowania
                            Wrong_password.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }
    
    }
}
