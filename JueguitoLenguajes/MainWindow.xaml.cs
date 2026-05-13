using System.Windows;
using System.Windows.Controls;
using JueguitoLenguajes.ViewModel;

namespace JueguitoLenguajes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PwdJ1Box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is TicTacToe viewModel && sender is PasswordBox passwordBox)
            {
                viewModel.PwdJ1 = passwordBox.Password;
            }
        }

        private void PwdJ2Box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is TicTacToe viewModel && sender is PasswordBox passwordBox)
            {
                viewModel.PwdJ2 = passwordBox.Password;
            }
        }

        private void PwdRegistroBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is TicTacToe viewModel && sender is PasswordBox passwordBox)
            {
                viewModel.PwdRegistro = passwordBox.Password;
            }
        }
    }
}
