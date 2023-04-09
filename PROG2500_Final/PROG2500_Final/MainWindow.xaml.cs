using PROG2500_Final.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROG2500_Final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ImdbProjectContext _context = new ImdbProjectContext();
        public Pages.HomePage HomePage { get; set; }
        public Pages.MoviesPage MoviesPage { get; set; }
        public Pages.SeriesPage SeriesPage { get; set; }
        public Pages.DirectorsPage DirectorsPage { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            HomePage = new Pages.HomePage();
            MoviesPage = new Pages.MoviesPage();
            SeriesPage = new Pages.SeriesPage(_context);
            DirectorsPage = new Pages.DirectorsPage();

            mainFrame.NavigationService.Navigate(HomePage);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(HomePage);
        }

        private void MoviesButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(MoviesPage);
        }

        private void SeriesButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(SeriesPage);
        }

        private void DirectorsButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(DirectorsPage);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
