﻿using PROG2500_Final.Data;
using PROG2500_Final.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace PROG2500_Final.Pages
{
    public partial class MoviesPage : Page
    {
        private ImdbProjectContext _context;
        public ObservableCollection<MovieData> Movies { get; set; }

        public MoviesPage()
        {
            InitializeComponent();
            _context = new ImdbProjectContext();
            Movies = new ObservableCollection<MovieData>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMovies();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadMovies(SearchTextBox.Text);
        }

        private void LoadMovies(string searchTitle = "")
        {
            if (searchTitle != "")
            {
                MoviesListView.ItemsSource = _context.Titles
                    .Where(t => t.PrimaryTitle.Contains(searchTitle))
                    .Select(t => new MovieData
                    {
                        Title = t.PrimaryTitle,
                        Director = t.Names.Where(n => n.Principals.First(p => p.JobCategory == "director") != null).First().PrimaryName,
                        AverageRating = (double?)t.Rating.AverageRating
                    }).Take(100).ToList();
            } 
            else
            {
                MoviesListView.ItemsSource = _context.Titles
                    .Take(1000)
                    .Select(t => new MovieData 
                    {
                        Title = t.PrimaryTitle,
                        Director = t.Names.Where(n => n.Principals.First(p => p.JobCategory == "director") != null).First().PrimaryName,
                        AverageRating = (double?)t.Rating.AverageRating
                    }).ToList();
            }



            //var titles = _context.Titles.ToList();
            //if (!string.IsNullOrEmpty(searchTitle))
            //{
            //    titles = titles.Where(t => t.PrimaryTitle.Contains(searchTitle)).ToList();
            //}

            //foreach (var title in titles)
            //{
            //    var movieData = new MovieData
            //    {
            //        Title = title.PrimaryTitle
            //    };

            //    // Fetch average rating from Ratings table using titleId
            //    var rating = _context.Ratings.FirstOrDefault(r => r.TitleId == title.TitleId);
            //    if (rating != null)
            //    {
            //        movieData.AverageRating = (double?)rating.AverageRating;
            //    }

            //    // Fetch director's name from Principals table using titleId and job_category
            //    var principal = _context.Principals.FirstOrDefault(p => p.TitleId == title.TitleId && p.JobCategory == "director");
            //    if (principal != null)
            //    {
            //        var name = _context.Names.FirstOrDefault(n => n.NameId == principal.NameId && principal.JobCategory == "director");
            //        if (name != null)
            //        {
            //            movieData.Director = name.PrimaryName;
            //        }
            //    }

            //    Movies.Add(movieData);
            //    }
        }
    }

    public class MovieData
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public double? AverageRating { get; set; }
    }
}
