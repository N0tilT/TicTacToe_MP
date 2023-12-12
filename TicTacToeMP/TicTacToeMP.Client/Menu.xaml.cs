using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using TicTacToeMP.Client;
using TicTacToeMP.Core.Client.ViewModel;

namespace TicTacToeMP.Core.Client
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }
        private string ip;
        private string port;
        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPAddress.Parse(this.tbIP.Text); 
                GameWindow gameWindow = new GameWindow(this.tbPlayername.Text, this.tbIP.Text);
                gameWindow.Show();
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Неверно введён IP адрес!","Ошибка!",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            
        }
    }
}
