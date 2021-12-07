using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
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
using CefSharp;
using CefSharp.Wpf;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Newtonsoft.Json;


namespace Cinema
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int countp { get; set; }
        public string nameis { get; set; }
        public string Description { get; set; }
        public dynamic Data { get; set; }
        public dynamic SingleData { get; set; }
        List<Movie> movies = new List<Movie>();
        Dictionary<string, bool> seats = new Dictionary<string, bool>();
        private int i = 0;
        private string _youtubeVideo;
        ChromiumWebBrowser chrome;
        private readonly YouTubeService _youtubeService;
        private ApiConfig _config;
        HttpClient httpClient = new HttpClient();
        public RelayCommand MessageCommand { get; set; }
        public RelayCommand SendCommand { get; set; }

        public MainWindow()
        {
            var settings = new CefSettings();
            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            Cef.Initialize(settings, true, browserProcessHandler: null);
            InitializeComponent();
            DataContext = this;
            MessageCommand = new RelayCommand(Display);
            SendCommand = new RelayCommand(Send);
            PrepareApi();
            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _config.Token,
                ApplicationName = this.GetType().ToString()
            });
            SetFirst();
            deserializefunk();
        }
        private void deserializefunk()
        {
            char[] splitchar = { ':' };
            string[] nn;
            nn = movieNameLabel.Content.ToString().ToLower().Split(splitchar);
            nameis = nn[0];
            if (File.Exists(nameis + ".json"))
            {
                var movie = FileHelper.JsonDeserializeMovie(nameis);
                List<CheckBoxx> seats = new List<CheckBoxx>();
                int index = 0;
                foreach (var item in Canvas.Children)
                {
                    var st = item as CheckBox;
                    st.IsChecked = movie.Seats[index].IsChecked;
                    index++;
                }
            }
        }
        
        private void CheckOutclck(object sender, RoutedEventArgs e)
        {
            char[] splitchar = {':'};
            string[] nn;
            nn = movieNameLabel.Content.ToString().ToLower().Split(splitchar);
            nameis = nn[0];
            if (File.Exists(nameis + ".json"))
            {
                File.Delete(nameis + ".json");
                List<CheckBoxx> seats = new List<CheckBoxx>();
                int index = 0;
                foreach (var item in Canvas.Children)
                {
                    var chbox = item as CheckBox;
                    CheckBoxx seat = new CheckBoxx(chbox.Name, chbox.IsChecked, 10);
                    seats.Add(seat);
                }
                Movie movie2 = new Movie
                {
                    Name = nameis,
                    Seats = seats,
                    Minute = movieRuntimeLabel.Content.ToString(),
                    Country = movieCountryLabel.Content.ToString(),
                    Time = movieRuntimeLabel.Content.ToString(),
                    Hall = CinemaHallLabel.Text,
                    Type = MovieLabel1.Text,
                    Genre = movieDescriptionLabel.Content.ToString(),
                    Imdb = movieImdbLabel.Content.ToString(),
                    Price = PriceLabel.Content.ToString(),
                    Hdate = $"{CalendarLabel.Text} {TimeBox.Text}"
                };
                FileHelper.JsonSerializationMovie(movie2);
            PDF.CreatePDF(movie2,nameis);
            }
            else
            {
                #region WriteJson
                List<CheckBoxx> seats = new List<CheckBoxx>();
                foreach (var item in Canvas.Children)
                {
                    var chbox = item as CheckBox;
                    CheckBoxx seat = new CheckBoxx(chbox.Name, chbox.IsChecked, 10);
                    seats.Add(seat);
                }
                Movie movie = new Movie
                {
                    Name = nameis,
                    Seats = seats,
                    Minute = movieRuntimeLabel.Content.ToString(),
                    Country = movieCountryLabel.Content.ToString(),
                    Time = movieRuntimeLabel.Content.ToString(),
                    Hall = CinemaHallLabel.Text,
                    Type = MovieLabel1.Text,
                    Genre = movieDescriptionLabel.Content.ToString(),
                    Imdb = movieImdbLabel.Content.ToString(),
                    Price = PriceLabel.Content.ToString(),
                    Hdate = $"{CalendarLabel.Text} {TimeBox.Text}"
                };
                FileHelper.JsonSerializationMovie(movie);
            PDF.CreatePDF(movie,nameis);
            }
            #endregion
            if (!(PriceLabel.Content.ToString() == "0 AZN"))
            {
                MessageBox.Show("You bought ticket,Your bill is in the bin file of program ");
            }
            else
            {
                MessageBox.Show("You did not any bought ticket, Firstly choose sit  ");
            }
        }
        private void PrepareApi()
        {
            var str = string.Empty;
            var fileName = "ApiConfig.json";
            if (!File.Exists(fileName))
                return;
            using (var fs = File.OpenRead("ApiConfig.json"))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
                str = sr.ReadToEnd();
            _config = JsonConvert.DeserializeObject<ApiConfig>(str);
        }
        private async void SetFirst()
        {
            var name = "X-Men: Apocalypse";
            HttpResponseMessage response = new HttpResponseMessage();
            response = httpClient.GetAsync($@"http://www.omdbapi.com/?apikey=bdba957b&s={name}&plot=full").Result;
            var str = response.Content.ReadAsStringAsync().Result;
            Data = JsonConvert.DeserializeObject(str);
            response = httpClient.GetAsync($@"http://www.omdbapi.com/?apikey=bdba957b&t={Data.Search[0].Title}&plot=full").Result;
            str = response.Content.ReadAsStringAsync().Result;
            dynamic SingleData = JsonConvert.DeserializeObject(str);
            movies.Add(new Movie
            {
                Name = SingleData.Title,
                Genre = SingleData.Genre,
                Poster = SingleData.Poster,
                Country = SingleData.Country,
                Director = SingleData.Director,
                Writers = SingleData.Writer,
                MovieImage = SingleData.Poster,
                MovieImage2 = SingleData.Poster,
                Minute = SingleData.Runtime,
                Imdb = SingleData.imdbRating,
                Year = SingleData.Released,
                Plot = SingleData.Plot

            });
            foreach (var movie in movies)
            {
                movieNameLabel.Content = movie.Name;
                MovieImage2.Source = new BitmapImage(new Uri(movie.MovieImage2));
                string Genre = movie.Genre;
                string[] Genres = Genre.Split(',');
                movieDescriptionLabel.Content = Genres[0] + " |" + Genres[1] + " |" + Genres[2];
                movieCountryLabel.Content = movie.Country;
                movieRuntimeLabel.Content = movie.Minute;
                movieReleasedLabel.Content = movie.Year.ToString("dd MMMM yyyy");
                movieImdbLabel.Content = movie.Imdb + " /";
                PlotText.Text = movie.Plot;
                Writers.Content = movie.Writers;
                DirectorLabel.Content = movie.Director;
            }
            var keyword = String.Format("{0} {1} trailer", SingleData.Title, SingleData.Year);
            await Search2(keyword);
            LoadTrailer();
        }
        private void LoadTrailer2()
        {
            var windowAK = new Window();
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
            browser.Address = $@"https://www.youtube.com/embed/{_youtubeVideo}?autoplay=1";
            windowAK.Content = browser;
            ChromiumBrowser.StopCommand.Execute(true);
            windowAK.ShowDialog();
        }
        private async void  Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChromiumBrowser.Address = $@"https://www.youtube.com/embed/{_youtubeVideo}?autoplay=1&mute=1";
            var keyword = String.Format("{0} {1} trailer", movieNameLabel.Content, movieReleasedLabel.Content);
            await Search2(keyword);
            LoadTrailer2();
            ChromiumBrowser.Address = $@"https://www.youtube.com/embed/{_youtubeVideo}?autoplay=1";
        }
        private void LoadTrailer()
        {
            ChromiumBrowser.Address = $@"https://www.youtube.com/embed/{_youtubeVideo}?autoplay=1&controls=0";
        }
        private async Task Search2(string keyword)
        {
            var searchListRequest = _youtubeService.Search.List("snippet");
            searchListRequest.Q = keyword;
            searchListRequest.MaxResults = 1;
            var searchListResponse = await searchListRequest.ExecuteAsync();
            List<string> videos = new List<string>();
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(searchResult.Id.VideoId);
                        break;
                    default:
                        break;
                }
            }
            _youtubeVideo = videos[0];
        }
        private async void Search_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            PriceLabel.Content = 0;
            countp = 0;
            var name = MovieTextBox.Text;
            HttpResponseMessage response = new HttpResponseMessage();
            response = httpClient.GetAsync($@"http://www.omdbapi.com/?apikey=bdba957b&s={name}&plot=full").Result;
            var str = response.Content.ReadAsStringAsync().Result;
            Data = JsonConvert.DeserializeObject(str);
            response = httpClient.GetAsync($@"http://www.omdbapi.com/?apikey=bdba957b&t={Data.Search[0].Title}&plot=full").Result;
            str = response.Content.ReadAsStringAsync().Result;
            SingleData = JsonConvert.DeserializeObject(str);
            movies.Add(new Movie
            {
                Name = SingleData.Title,
                Genre = SingleData.Genre,
                Poster = SingleData.Poster,
                Country = SingleData.Country,
                Director = SingleData.Director,
                Writers = SingleData.Writer,
                MovieImage = SingleData.Poster,
                MovieImage2 = SingleData.Poster,
                Minute = SingleData.Runtime,
                Imdb = SingleData.imdbRating,
                Year = SingleData.Released,
                Plot = SingleData.Plot
            });
            foreach (var movie in movies)
            {
                movieNameLabel.Content = movie.Name;
                MovieImage2.Source = new BitmapImage(new Uri(movie.MovieImage2));
                string Genre = movie.Genre;
                string[] Genres = Genre.Split(',');
                movieDescriptionLabel.Content = Genres[0] + " |" + Genres[1] + " |" + Genres[2];
                movieCountryLabel.Content = movie.Country;
                movieRuntimeLabel.Content = movie.Minute;
                movieReleasedLabel.Content = movie.Year.ToString("dd MMMM yyyy");
                movieImdbLabel.Content = movie.Imdb + " /";
            }
            var keyword = String.Format("{0} {1} trailer", SingleData.Title, SingleData.Year);
            await Search2(keyword);
            LoadTrailer();
            char[] splitchar = { ':' };
            string[] nn;
            nn = movieNameLabel.Content.ToString().ToLower().Split(splitchar);
            nameis = nn[0];
            if (File.Exists(nameis + ".json"))
            {
                var movie = FileHelper.JsonDeserializeMovie(nameis);
                List<CheckBoxx> seats = new List<CheckBoxx>();
                int index = 0;
                foreach (var item in Canvas.Children)
                {
                    var st = item as CheckBox;
                    st.IsChecked = movie.Seats[index].IsChecked;
                    index++;
                }
            }
            else
            {
                int index = 0;
                foreach (var item in Canvas.Children)
                {
                    var st = item as CheckBox;
                    st.IsChecked = false;
                    index++;
                }
            }
        }
        private void Search_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MovieTextBox.Visibility = Visibility.Visible;
            BorderSearch.Visibility = Visibility.Visible;
        }
        public void Display(object text = null)
        {
        }
        public void Send(object text = null)
        {
            MessageBox.Show("Message was sent");
        }
        private void PriceSum(object sender, MouseEventArgs e)
        {
            PriceLabel.Content = $"{countp += 10} AZN";
        }
    }
}
