using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROG2500_Final.Data;
using PROG2500_Final.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace PROG2500_Final.Pages
{
    /// <summary>
    /// Interaction logic for DirectorsPage.xaml
    /// </summary>
    public partial class DirectorsPage : Page
    {
        private ImdbProjectContext _context = new ImdbProjectContext();
        public DirectorsPage()
        {
            InitializeComponent();
            _context.Names.Take(2000).Load();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = directorSearch.Text;

            var query =
                from Name in _context.Names.Take(2000)
                where Name.PrimaryName.Contains(searchTerm)
                select Name;

            var directorViewSource = (CollectionViewSource)FindResource("directorViewSource");
            directorViewSource.Source = new ObservableCollection<Name>(query.ToList());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            var query =
                (from Name in _context.Names
                 select Name)
                .Take(2000)
                .ToList();

            var directorViewSource = (CollectionViewSource)FindResource("directorViewSource");
            directorViewSource.Source = new ObservableCollection<Name>(query);


        }
    }
}
