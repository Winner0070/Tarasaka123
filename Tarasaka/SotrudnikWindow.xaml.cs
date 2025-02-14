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
using System.Windows.Shapes;
using System.Data;
using System.IO;
using DocumentFormat.OpenXml.Office2010.Excel;


namespace Tarasaka
{
    public partial class SotrudnikWindow : Window
    {


        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-1LANN3F\SQLEXPRESS;Initial Catalog=Tarasaka;Integrated Security=True;");
        DataTable dataTable5 = new DataTable();
        SqlCommand command;
        SqlDataAdapter dataAdapter4;
        DataTable dataTable2 = new DataTable();
        SqlCommand command1;
        SqlDataAdapter dataAdapter1;

        public SotrudnikWindow()
        {
            InitializeComponent();
            LoadData_Main_Otch();
            LoadData_Job();
        }

        public void IDKK(int id)
        {
            kk.Text = id.ToString();
        }

        public void SetEmployeeInfo1(string id, string familia, string name, string idLevel)
        {
            TextBlock_ID.Text = $"ID Сотрудника: {id}";
            TextBlock_Familia.Text = $"Фамилия: {familia}";
            TextBlock_Name.Text = $"Имя: {name}";
            TextBlock_IDLevel.Text = $"Уровень: {idLevel}";

            if (TextBlock_IDLevel.Text == "Уровень: 1")
            {
                TextBlock_IDLevel.Text = "Высокий уровень продуктивности";
            }
            if (TextBlock_IDLevel.Text == "Уровень: 2")
            {
                TextBlock_IDLevel.Text = "Средний уровень продуктивности";
            }
            if (TextBlock_IDLevel.Text == "Уровень: 3")
            {
                TextBlock_IDLevel.Text = "Низкий уровень продуктивности";
            }
            if (TextBlock_IDLevel.Text == "Уровень: 4")
            {
                TextBlock_IDLevel.Text = "Риск на увольнение";
            }
        }

        private void LoadData_Job()
        {

            try
            {
                con.Open();
                SqlCommand command1 = new SqlCommand("SELECT * FROM Job_Account", con);
                SqlDataAdapter dataAdapter1 = new SqlDataAdapter(command1);
                dataTable2.Clear();
                dataAdapter1.Fill(dataTable2);
                dataGrid_Job.ItemsSource = dataTable2.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void LoadData_Main_Otch()
        {

            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Otchet", con);
                SqlDataAdapter dataAdapter4 = new SqlDataAdapter(command);
                dataTable5.Clear();
                dataAdapter4.Fill(dataTable5);
                dataMain_Otchety.ItemsSource = dataTable5.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnmenu_Otchety_Click(object sender, RoutedEventArgs e)
        {
            Grid_Zadanya.Visibility = Visibility.Hidden;

            if (Grid_Othoty.Visibility == Visibility.Hidden)
            {
                Grid_Othoty.Visibility = Visibility.Visible;
            }
            else
            {
                Grid_Othoty.Visibility = Visibility.Hidden;
            }
        }

        private void btn_del_job_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            string title = ReportTitle.Text;
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Сначала сохраните отчет, чтобы его напечатать.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string fileName = $"./Отчеты/{title}.txt";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Файл не найден. Убедитесь, что отчет сохранен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {

                var textContent = File.ReadAllText(fileName);
                FlowDocument document = new FlowDocument(new Paragraph(new Run(textContent)));

                IDocumentPaginatorSource idocument = document;
                printDialog.PrintDocument(idocument.DocumentPaginator, "Печать отчета");
            }
        }

        public void AddOtchet_one(string Name_Otchet, string Otchet)
        {
            con.Open();
            command = new SqlCommand($"INSERT INTO Otchet(Name_Otchet, Otchet) VALUES(@Name_Otchet,@Otchet)", con);
            command.Parameters.AddWithValue("Name_Otchet", Name_Otchet);
            command.Parameters.AddWithValue("Otchet", Otchet);
            command.ExecuteNonQuery();
            con.Close();
        }

        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {
            string title = ReportTitle.Text;
            string content = ReportContent.Text;
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                string fileName = $"./Отчеты/{title}.txt";

                File.WriteAllText(fileName, content);
                MessageBox.Show("Отчет сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                AddOtchet_one(title, content);
                LoadData_Main_Otch();

                using (SqlConnection connection = con)
                {
                    string queryCheck = "SELECT COUNT(*) FROM Otchet WHERE Name_Otchet = @Title";
                    SqlCommand commandCheck = new SqlCommand(queryCheck, connection);
                    commandCheck.Parameters.AddWithValue("@Title", title);

                    connection.Open();
                    int exists = (int)commandCheck.ExecuteScalar();

                    if (exists > 0)
                    {
                        string queryUpdate = "UPDATE Otchet SET Otchet = @Content WHERE Name_Otchet = @Title";
                        SqlCommand commandUpdate = new SqlCommand(queryUpdate, connection);
                        commandUpdate.Parameters.AddWithValue("@Content", content);
                        commandUpdate.Parameters.AddWithValue("@Title", title);
                        commandUpdate.ExecuteNonQuery();

                        MessageBox.Show("Отчет обновлен в базе данных!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string queryInsert = "INSERT INTO Otchet (Name_Otchet, Otchet) VALUES (@Title, @Content)";
                        SqlCommand commandInsert = new SqlCommand(queryInsert, connection);
                        commandInsert.Parameters.AddWithValue("@Title", title);
                        commandInsert.Parameters.AddWithValue("@Content", content);
                        commandInsert.ExecuteNonQuery();

                        MessageBox.Show("Новый отчет сохранен в базе данных!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void btnmenu_Zadanya_Click(object sender, RoutedEventArgs e)
        {
            Grid_Othoty.Visibility = Visibility.Hidden;

            if (Grid_Zadanya.Visibility == Visibility.Hidden)
            {
                Grid_Zadanya.Visibility = Visibility.Visible;
            }
            else
            {
                Grid_Zadanya.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
        }

        private void minwin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void dataMain_Otchety_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UpdateSotrudnik(int idJob, int newIDsotr)
        {
            string query = "UPDATE [dbo].[Job_Account] SET [IDSotrudnicy] = @NewIDSotrudnicy WHERE [IDJob] = @IDJob";

            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@NewIDSotrudnicy", newIDsotr);
            command.Parameters.AddWithValue("@IDJob", idJob);

            try
            {
                con.Open();
                int rowsAffected = command.ExecuteNonQuery();
                MessageBox.Show(rowsAffected > 0 ? "Данные обновлены успешно." : "Сотрудник не найден.");
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void takeJob_Click(object sender, RoutedEventArgs e)
        {
            string idSotr = kk.Text;

            UpdateSotrudnik(int.Parse(textB5.Text), int.Parse(idSotr));
            if (textB5.Text != null)
            {
                textB5.Text = null;
            }
            LoadData_Job();
        }

        private void btnmenu_exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
