using System;
using System.Data.SqlClient;
using System.Windows;

namespace app_back_end
{
    public partial class rezerwacja : Window
    {
        private main_kierownik mainKierownikWindow;
        private DBConnection dbConnection = DBConnection.Instance;

        public rezerwacja(main_kierownik mainKierownikWindow)
        {
            InitializeComponent();
            this.mainKierownikWindow = mainKierownikWindow;
        }

        private void Zarezerwuj_Click(object sender, RoutedEventArgs e)
        {
            int idCzesci;
            int ilosc;
            int idZlecenia;

            if (int.TryParse(CzescIDTextBox.Text, out idCzesci) &&
                int.TryParse(IloscTextBox.Text, out ilosc) &&
                int.TryParse(ZlecenieIDTextBox.Text, out idZlecenia))
            {
                using (SqlConnection connection = dbConnection.Connect())
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        // Sprawdź dostępność części i aktualizuj tabelę Tbl_Material
                        string updateMaterialQuery = @"
                            UPDATE Tbl_Material
                            SET stan = stan - @ilosc, wolne = wolne - @ilosc
                            WHERE Id_czesci = @idCzesci AND wolne >= @ilosc";

                        using (SqlCommand updateMaterialCmd = new SqlCommand(updateMaterialQuery, connection, transaction))
                        {
                            updateMaterialCmd.Parameters.AddWithValue("@idCzesci", idCzesci);
                            updateMaterialCmd.Parameters.AddWithValue("@ilosc", ilosc);

                            int rowsAffected = updateMaterialCmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new InvalidOperationException("Niewystarczająca ilość części dostępna.");
                            }
                        }

                        // Dodaj wpis do Tbl_rezerwacja
                        string insertRezerwacjaQuery = @"
                            INSERT INTO Tbl_rezerwacja (Id, Id_czesci, ilosc, Id_Zlecenia)
                            VALUES ((SELECT ISNULL(MAX(Id), 0) + 1 FROM Tbl_rezerwacja), @idCzesci, @ilosc, @idZlecenia)";

                        using (SqlCommand insertRezerwacjaCmd = new SqlCommand(insertRezerwacjaQuery, connection, transaction))
                        {
                            insertRezerwacjaCmd.Parameters.AddWithValue("@idCzesci", idCzesci);
                            insertRezerwacjaCmd.Parameters.AddWithValue("@ilosc", ilosc);
                            insertRezerwacjaCmd.Parameters.AddWithValue("@idZlecenia", idZlecenia);

                            insertRezerwacjaCmd.ExecuteNonQuery();
                        }

                        // Oblicz sumę cen części dla danego zlecenia
                        string calculateTotalQuery = @"
                            SELECT SUM(m.Cena * r.ilosc) AS Total
                            FROM Tbl_rezerwacja r
                            INNER JOIN Tbl_Material m ON r.Id_czesci = m.Id_czesci
                            WHERE r.Id_Zlecenia = @idZlecenia";

                        decimal totalKwota = 0;

                        using (SqlCommand calculateTotalCmd = new SqlCommand(calculateTotalQuery, connection, transaction))
                        {
                            calculateTotalCmd.Parameters.AddWithValue("@idZlecenia", idZlecenia);

                            object result = calculateTotalCmd.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                totalKwota = Convert.ToDecimal(result);
                            }
                        }

                        // Aktualizuj pole Kwota w tabeli Tbl_Klient
                        string updateKwotaQuery = @"
                            UPDATE Tbl_Klient
                            SET Kwota = @totalKwota
                            WHERE Id_Zlecenia = @idZlecenia";

                        using (SqlCommand updateKwotaCmd = new SqlCommand(updateKwotaQuery, connection, transaction))
                        {
                            updateKwotaCmd.Parameters.AddWithValue("@totalKwota", totalKwota);
                            updateKwotaCmd.Parameters.AddWithValue("@idZlecenia", idZlecenia);

                            updateKwotaCmd.ExecuteNonQuery();
                        }

                        // Zatwierdź transakcję
                        transaction.Commit();
                        WynikTextBlock.Text = "Rezerwacja zakończona sukcesem.";
                    }
                    catch (Exception ex)
                    {
                        // Wycofaj transakcję w przypadku błędu
                        transaction.Rollback();
                        WynikTextBlock.Text = $"Błąd rezerwacji: {ex.Message}";
                    }
                    finally
                    {
                        dbConnection.Disconnect(connection); // Zamknij połączenie
                    }
                }
            }
            else
            {
                WynikTextBlock.Text = "Proszę wprowadzić poprawne dane.";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Przywracamy widoczność okna main_kierownik
            mainKierownikWindow.Visibility = Visibility.Visible;
        }
    }
}
