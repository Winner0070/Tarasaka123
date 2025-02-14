using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using Tarasaka.Components;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Data.OleDb;
using System.Windows.Forms;
using static MaterialDesignThemes.Wpf.Theme;
using System.IO;
using MessageBox = System.Windows.MessageBox;
using PrintDialog = System.Windows.Controls.PrintDialog;
using Word = Microsoft.Office.Interop.Word;
using System.Collections;
using DocumentFormat.OpenXml.Office.Word;
using Azure;
using DataGrid = System.Windows.Controls.DataGrid;
using DocumentFormat.OpenXml.Bibliography;
using System.Windows.Media.Animation;

namespace Tarasaka
{
    /// <summary>
    /// Логика взаимодействия для Tarasaka_Main.xaml
    /// </summary>
    public partial class Tarasaka_Main : Window
    {
        Database db = new Database();
        DataTable dataTable = new DataTable();
        DataTable dataTable2 = new DataTable();
        DataTable dataTable3 = new DataTable();
        DataTable dataTable4 = new DataTable();
        DataTable dataTable5 = new DataTable();
        DataTable dataTable0 = new DataTable();
        SqlCommand command;
        SqlCommand command2;
        SqlCommand command3;
        SqlCommand command4;
        SqlCommand command5;
        SqlCommand command0;
        SqlDataAdapter dataAdapter;
        SqlDataAdapter dataAdapter2;
        SqlDataAdapter dataAdapter3;
        SqlDataAdapter dataAdapter4;
        SqlDataAdapter dataAdapter0;
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-1LANN3F\SQLEXPRESS;Initial Catalog=Tarasaka;Integrated Security=True;");
        SqlDataReader rdr;
        public Tarasaka_Main()
        {

            InitializeComponent();
            LoadData_Job();
            LoadData();
            LoadData_Dolshnosty();
            LoadData_Main_Otch();
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

        public void SetEmployeeInfo(string id, string familia, string name, string idLevel)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minwin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void LoadData()
        {

            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM IDSotrudnicy_Accounts", db.getConnection());
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataTable.Clear();
                dataAdapter.Fill(dataTable);
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                db.closeConnection();
            }
        }

        private void LoadData_Job()
        {

            try
            {
                db.openConnection();
                SqlCommand command1 = new SqlCommand("SELECT * FROM Job_Account", db.getConnection());
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
                db.closeConnection();
            }
        }

        private void LoadDataFree(string query, DataTable dataTable, DataGrid dataGrid)
        {
            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataTable.Clear();
                dataAdapter.Fill(dataTable);
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                db.closeConnection();
            }
        }

        private string GetReportFromDatabase(string reportId)
        {
            string reportContent = null;

                con.Open();
                string query = "SELECT Otchet FROM Otchet WHERE Name_Otchet = @Name_Otchet";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Name_Otchet", reportId);
                    var result = command.ExecuteScalar();
                    reportContent = result as string;
                    con.Close();
                }

            return reportContent;
        }
        private void UpdateSotrudnik(int idSotrudnika, int newIDLevel, int newIDDolshnosty)
        {
            string query = "UPDATE [dbo].[IDSotrudnicy_Accounts] SET [IDLevel] = @NewIDLevel, [IDDolshnosty] = @NewIDDolshnosty WHERE [IDSotrudnicy] = @IDSotrudnicy";

                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@NewIDLevel", newIDLevel);
                command.Parameters.AddWithValue("@NewIDDolshnosty", newIDDolshnosty);
                command.Parameters.AddWithValue("@IDSotrudnicy", idSotrudnika);

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


        public void Add_Dolshnosty(string Desription, string NameD)
        {
            con.Open();
            command3 = new SqlCommand($"INSERT INTO Dolshnosty(Desription, NameD) VALUES(@Desription, @NameD)", con);
            command3.Parameters.AddWithValue("Desription", Desription);
            command3.Parameters.AddWithValue("NameD", NameD);
            command3.ExecuteNonQuery();
            con.Close();
        }


        public void Add_Job(string DescriptionJob, int PriceJob, DateTime TimeJobStart, DateTime TimeJobEnd, int IDLevelJob, int IDSotrudnicy)
        {
            con.Open();
            command2 = new SqlCommand($"INSERT INTO Job_Account(DescriptionJob, PriceJob, TimeJobStart, TimeJobEnd, IDLevelJob, IDSotrudnicy) VALUES(@DescriptionJob, @PriceJob, @TimeJobStart, @TimeJobEnd, @IDLevelJob, @IDSotrudnicy)", con);
            command2.Parameters.AddWithValue("DescriptionJob", DescriptionJob);
            command2.Parameters.AddWithValue("PriceJob", PriceJob);
            command2.Parameters.AddWithValue("TimeJobStart", TimeJobStart);
            command2.Parameters.AddWithValue("TimeJobEnd", TimeJobEnd);
            command2.Parameters.AddWithValue("IDLevelJob", IDLevelJob);
            command2.Parameters.AddWithValue("IDSotrudnicy", IDSotrudnicy);
            command2.ExecuteNonQuery();
            con.Close();
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

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSotrudnik(int.Parse(textBIDSotr.Text), int.Parse(textB3.Text), int.Parse(textB4.Text));
            LoadData();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            var idsotrudnicy = deltxtb.Text;

            con.Open();
            command = new SqlCommand($"DELETE FROM IDSotrudnicy_Accounts WHERE IDSotrudnicy = {idsotrudnicy}", con);
            command.ExecuteNonQuery();
            con.Close();
            LoadData();
        }

        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

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

                using (SqlConnection connection = db.getConnection())
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
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void updateButton_Job_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addbtn_job_Click(object sender, RoutedEventArgs e)
        {
            Add_Job(textB5.Text, int.Parse(textB6.Text), DateTime.Parse(date_job.Text), DateTime.Parse(date_job_2.Text), int.Parse(textB9.Text), int.Parse(textB10.Text));
            LoadData_Job();
        }
        private void addbtn_dol_Click(object sender, RoutedEventArgs e)
        {
            Add_Dolshnosty(textB_dol_one.Text, textB_dol_two.Text);
            LoadData_Dolshnosty();
        }

        private void LoadData_Dolshnosty()
        {
            LoadDataFree("SELECT * FROM Dolshnosty", dataTable4, dataGrid_desc);

        }
        private void LoadData_Main_Otch()
        {

            LoadDataFree("SELECT * FROM Otchet", dataTable5, dataMain_Otchety);

        }

        private void btn_del_job_Click(object sender, RoutedEventArgs e)
        {
            var idjob = deltxtb_job.Text;

            con.Open();
            command2 = new SqlCommand($"DELETE FROM Job_Account WHERE IDJob = {idjob}", con);
            command2.ExecuteNonQuery();
            con.Close();
            LoadData_Job();
        }

        private void btn_del_dol(object sender, RoutedEventArgs e)
        {

        }

        private void btn_del_dolsh_Click(object sender, RoutedEventArgs e)
        {
            var iddol = deltxtb_dol.Text;

            con.Open();
            command3 = new SqlCommand($"DELETE FROM Dolshnosty WHERE IDDolshnisty = {iddol}", con);
            command3.ExecuteNonQuery();
            con.Close();
            LoadData_Dolshnosty();
        }

        private void btnmenu_Otchety_Click(object sender, RoutedEventArgs e)
        {
            Grid_Dolshnosty.Visibility = Visibility.Hidden;
            Grid_Zadanya.Visibility = Visibility.Hidden;
            Grid_Sotrudnicy.Visibility = Visibility.Hidden;


            if (Grid_Othoty.Visibility == Visibility.Hidden)
            {
                Grid_Othoty.Visibility = Visibility.Visible;
            }
            else
            {
                Grid_Othoty.Visibility = Visibility.Hidden;
            }
        }

        private void btnmenu_Dolshnosty_Click(object sender, RoutedEventArgs e)
        {
            Grid_Othoty.Visibility = Visibility.Hidden;
            Grid_Zadanya.Visibility = Visibility.Hidden;
            Grid_Sotrudnicy.Visibility = Visibility.Hidden;

            if (Grid_Dolshnosty.Visibility == Visibility.Hidden)
            {
                Grid_Dolshnosty.Visibility = Visibility.Visible;
            }
            else
            {
                Grid_Dolshnosty.Visibility = Visibility.Hidden;
            }
        }

        private void btnmenu_Zadanya_Click(object sender, RoutedEventArgs e)
        {

            Grid_Othoty.Visibility = Visibility.Hidden;
            Grid_Dolshnosty.Visibility = Visibility.Hidden;
            Grid_Sotrudnicy.Visibility = Visibility.Hidden;

            if (Grid_Zadanya.Visibility == Visibility.Hidden)
            {
                Grid_Zadanya.Visibility = Visibility.Visible;
            }
            else
            {
                Grid_Zadanya.Visibility = Visibility.Hidden;
            }
        }

        private void btnmenu_Sotrudnicy_Click(object sender, RoutedEventArgs e)
        {
            Grid_Othoty.Visibility = Visibility.Hidden;
            Grid_Dolshnosty.Visibility = Visibility.Hidden;
            Grid_Zadanya.Visibility = Visibility.Hidden;
            

            if (Grid_Sotrudnicy.Visibility == Visibility.Hidden)
            {
                Grid_Sotrudnicy.Visibility = Visibility.Visible;
            }
            else
            {
                Grid_Sotrudnicy.Visibility = Visibility.Hidden;
            }
        }

        private void dataMain_Otchety_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Открыть_отчет_Click(object sender, RoutedEventArgs e)
        {
            var reportId = ReportTitle.Text;

            try
            {
                    Grid_Othoty.Visibility = Visibility.Visible;
                    string reportContent = GetReportFromDatabase(reportId);
                    ReportContent.Text = reportContent ?? "Отчет не найден.";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btndelete_otchet_Click(object sender, RoutedEventArgs e)
        {
            string title = ReportTitle.Text;

            if (!string.IsNullOrEmpty(title))
            {
                string fileName = $"./Отчеты/{title}.txt";
                using (SqlConnection connection = db.getConnection())
                {
                    string queryDelete = "DELETE FROM Otchet WHERE Name_Otchet = @Title";
                    SqlCommand commandDelete = new SqlCommand(queryDelete, connection);
                    commandDelete.Parameters.AddWithValue("@Title", title);

                    connection.Open();
                    int rowsAffected = commandDelete.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }

                        MessageBox.Show("Отчет успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData_Main_Otch();
                    }
                    else
                    {
                        MessageBox.Show("Отчет не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название отчета для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Открыть_отчет1_Click(object sender, RoutedEventArgs e)
        {
            var reportId = ReportTitle1.Text;

            try
            {
                Grid_Othoty.Visibility = Visibility.Visible;
                string reportContent = GetReportFromDatabase(reportId);
                ReportContent.Text = reportContent ?? "Отчет не найден.";
                ReportTitle.Text = reportId;
                if (reportId != null)
                {
                    ReportTitle1.Text = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnmenu_exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
