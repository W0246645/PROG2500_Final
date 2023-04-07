using Microsoft.EntityFrameworkCore;
using PROG2500_Final.Data;
using PROG2500_Final.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PROG2500_Final.Pages
{
    /// <summary>
    /// Interaction logic for SeriesPage.xaml
    /// </summary>
    public partial class SeriesPage : Page
    {
        private readonly ImdbProjectContext _context;
        public SeriesPage(ImdbProjectContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var search = _context.Episodes.GroupBy(e => e.ParentTitle).Select(g => g.First()).ToList().Where(t => t.ParentTitle.PrimaryTitle.ToLower().Contains(seriesSearch.Text.ToLower())).AsEnumerable()
                .Select(s => new
                {
                    Series = s.ParentTitle.PrimaryTitle,
                    SeriesRating = s.ParentTitle.Rating != null ? s.ParentTitle.Rating.AvgFormatted : "No rating",
                    Episodes = s.ParentTitle.EpisodeParentTitles.ToList()
                }).ToList();

            seriesListView.ItemsSource = search;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Titles.Load();
            _context.Episodes.Load();
            _context.Ratings.Load();

            var series = _context.Episodes.GroupBy(e => e.ParentTitle)
                .Select(g => g.First())
                .AsEnumerable()
                .Select(s => new
                {
                    Series = s.ParentTitle.PrimaryTitle,
                    SeriesRating = s.ParentTitle.Rating != null ? s.ParentTitle.Rating.AvgFormatted : "No rating",
                    Episodes = s.ParentTitle.EpisodeParentTitles.ToList()
                }).ToList();

            seriesListView.ItemsSource = series;
        }
    }
}
