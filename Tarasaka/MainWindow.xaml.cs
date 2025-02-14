using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Tarasaka.Components;
using Microsoft.SqlServer.Server;
using static MaterialDesignThemes.Wpf.Theme;
using System.Data;
using System.Windows.Forms;
using System.Windows;
using MessageBox = System.Windows.MessageBox;
using Aspose.Words.Tables;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Tarasaka
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Database database = new Database();
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-1LANN3F\SQLEXPRESS;Initial Catalog=Tarasaka;Integrated Security=True;");
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginUser = textBox1.Text;
            var passwordUser = PasswordBoxx.Password;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string query = "SELECT IDSotrudnicy, FamiliaS, NameS, IDLevel, IDUserStat FROM dbo.IDSotrudnicy_Accounts WHERE Login = @Login AND Password = @Password";

            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@Login", loginUser);
            command.Parameters.AddWithValue("@Password", passwordUser);

            con.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int idSotrudnika = reader.GetInt32(0);
                string familia = reader.GetString(1);
                string name = reader.GetString(2);
                int idLevel = reader.GetInt32(3);
                int iduserstat = reader.GetInt32(4);

                if (iduserstat == 2)
                {
                    Tarasaka_Main form2 = new Tarasaka_Main();
                    form2.SetEmployeeInfo(idSotrudnika.ToString(), familia, name, idLevel.ToString());
                    con.Close();
                    this.Close();
                    form2.Show();
                }
                else
                {
                    SotrudnikWindow form3 = new SotrudnikWindow();
                    form3.SetEmployeeInfo1(idSotrudnika.ToString(), familia, name, idLevel.ToString());
                    form3.IDKK(idSotrudnika);
                    con.Close();
                    this.Close();
                    form3.Show();
                }
                
            }
            else
            {
                if ((loginUser == "") | (passwordUser == ""))
                {
                    MessageBox.Show("Пустое поле!", "Ошибка ввода данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    con.Close();
                }
            }
            reader.Close();
        }

        private void RegisterUser(string familiaS, string nameS, int idLevel, int idDolshnosty, string login, string password, string email, int idUserStat)
        {
            string query = "INSERT INTO [dbo].[IDSotrudnicy_Accounts] (FamiliaS, NameS, IDLevel, IDDolshnosty, Login, Password, Email, IDUserStat) " + "VALUES (@FamiliaS, @NameS, @IDLevel, @IDDolshnosty, @Login, @Password, @Email, @IDUserStat)";

            SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@FamiliaS", familiaS);
                command.Parameters.AddWithValue("@NameS", nameS);
                command.Parameters.AddWithValue("@IDLevel", idLevel);
                command.Parameters.AddWithValue("@IDDolshnosty", idDolshnosty);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@IDUserStat", idUserStat);

                try
                {

                    con.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected > 0 ? "Регистрация прошла успешно." : "Ошибка регистрации.");
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
        }

        private void Crest_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

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

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            GridReg.Visibility = Visibility.Visible;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.2))

            };

            GridReg.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            fadeInAnimation.Completed += (s, a) => GridReg.Visibility = Visibility.Visible;
            GridSingIn.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }

        private void Crest1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            string familiaS = txtFamiliaS.Text;
            string nameS = txtNameS.Text;
            int idLevel = 1;
            int idDolshnosty = 1;
            string login = txtLogin.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text;
            int idUserStat = 1;

            if ((familiaS == "") | (nameS == "") | (login == "") | (password == "") | (email == ""))
            {
                MessageBox.Show("Заполните все поля","Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                RegisterUser(familiaS, nameS, idLevel, idDolshnosty, login, password, email, idUserStat);
            }
        }

        private void btnReg_logout_Click(object sender, RoutedEventArgs e)
        {
            GridSingIn.Visibility = Visibility.Visible;

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };

            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };

            GridSingIn.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            fadeOutAnimation.Completed += (s, a) => GridReg.Visibility = Visibility.Hidden;
            GridReg.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }
    }
}
