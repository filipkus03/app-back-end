using System;
using System.Data.SqlClient;

public class DBConnection
{
    private static DBConnection instance;
    private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Desktop\app back end\app back end\data\db_local.mdf;Integrated Security=True";

    // Prywatny konstruktor, aby zapobiec tworzeniu instancji z zewnątrz
    private DBConnection() { }

    // Publiczna właściwość dostępu do instancji Singleton
    public static DBConnection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DBConnection();
            }
            return instance;
        }
    }

    // Metoda do nawiązywania połączenia
    public SqlConnection Connect()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    // Metoda do zamykania połączenia
    public void Disconnect(SqlConnection connection)
    {
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
