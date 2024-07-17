using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace app_back_end.admin
{
    public partial class main_admin : Window
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Desktop\app back end\app back end\data\db_local.mdf;Integrated Security=True;Connect Timeout=30";
        private Window mainWindow;

        public main_admin(Window mainWindow = null)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            LoadUserData();
        }

        private void LoadUserData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID, Name, Login, Role FROM [dbo].[Tbl_Logins]";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    UserDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void RefreshDataButton_Click(object sender, RoutedEventArgs e)
        {
            LoadUserData();
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;
            string role = RoleTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO [dbo].[Tbl_Logins] (Name, Login, Password, Role) VALUES (@Name, @Login, @Password, @Role)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Role", role);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User added successfully.");
                        LoadUserData(); // Refresh the data grid
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Please enter the login of the user to delete.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM [dbo].[Tbl_Logins] WHERE Login = @Login";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User deleted successfully.");
                            LoadUserData(); // Refresh the data grid
                        }
                        else
                        {
                            MessageBox.Show("User not found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.Visibility = Visibility.Visible;
            }
        }
    }
}
