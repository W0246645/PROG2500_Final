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
            var search = _context.Episodes
                .Include(e => e.ParentTitle)
                    .ThenInclude(t => t.Rating)
                .Include(e => e.ParentTitle.EpisodeParentTitles)
                    .ThenInclude(e => e.Title)
                .Where(e => e.ParentTitle.PrimaryTitle.ToLower().Contains(seriesSearch.Text.ToLower()))
                .OrderBy(e => e.ParentTitle.PrimaryTitle)
                .ThenBy(e => e.SeasonNumber)
                .ThenBy(e => e.EpisodeNumber)
                .GroupBy(e => e.ParentTitle)
                .Select(g => new
                {
                    Series = g.Key.PrimaryTitle,
                    SeriesRating = g.Key.Rating != null ? g.Key.Rating.AvgFormatted : "No rating",
                    Episodes = g.ToList()
                })
                .ToList();

            seriesListView.ItemsSource = search;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var series = _context.Episodes
                .Include(e => e.Title)
                .Include(e => e.ParentTitle)
                    .ThenInclude(t => t.Rating)
                .Include(e => e.ParentTitle.EpisodeParentTitles)
                    .ThenInclude(e => e.Title)
                .OrderBy(e => e.ParentTitle.PrimaryTitle)
                .ThenBy(e => e.SeasonNumber)
                .ThenBy(e => e.EpisodeNumber)
                .Take(1000)
                .GroupBy(e => e.ParentTitle)
                .Select(g => new
                {
                    Series = g.Key.PrimaryTitle,
                    SeriesRating = g.Key.Rating != null ? g.Key.Rating.AvgFormatted : "No rating",
                    Episodes = g.ToList()
                }).ToList();

            seriesListView.ItemsSource = series;
        }
    }
}
