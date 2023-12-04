﻿using System;
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
        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            //DataContext = new GameViewModel(this.tbPlayername.Text);
            GameWindow gameWindow = new GameWindow();
            gameWindow.Show();
            Close();
        }
    }
}