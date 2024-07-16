using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using Zephyr.Helpers;
using Zephyr.Core;
using Zephyr.ViewModels;

namespace Zephyr.View
{
    public partial class MainWindow : Window
    {
        private NavigationHelpers navHelper;
        private RandomGeneration generator;
        private MainViewModel viewModel;
        private Validators validators;

        private JsonGetters jsonGetters;
        private JsonSetters jsonSetters;

        private IBrowser _browser;
        private bool hasStarted = false;

        private const int DefaultBet = 5;
        private const int DefaultMines = 3;
        private const int DefaultTiles = 4;
        private const int MinValue = 1;
        private const int MaxValue = 24;

        private int mines = DefaultMines;
        private int tiles = DefaultTiles;
        private int bet = DefaultBet;

        public MainWindow()
        {
            InitializeComponent();
            InitializeAnimation();

            validators = new Validators();
            viewModel = new MainViewModel();
            generator = new RandomGeneration();
            navHelper = new NavigationHelpers();

            jsonSetters = new JsonSetters();
            jsonGetters = new JsonGetters();

        }

        private void InitializeAnimation()
        {
            this.Width = 0;
            this.Show();

            double targetWidth = 800;
            TimeSpan duration = TimeSpan.FromSeconds(0.3);

            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 0,
                To = targetWidth,
                Duration = new Duration(duration),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            widthAnimation.Completed += (s, e) => this.Width = targetWidth;

            this.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            double targetWidth = 0;
            TimeSpan duration = TimeSpan.FromSeconds(0.5);

            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = this.ActualWidth,
                To = targetWidth,
                Duration = new Duration(duration),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            widthAnimation.Completed += (s, e) =>
            {
                this.Width = 0;
                this.Close();
            };

            this.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string auth = jsonGetters.getAuthFromJson();
            if (AuthEntryBox.Password.Length > 1 && AuthEntryBox.Password != auth)
            {
                jsonSetters.setAuthToJson(AuthEntryBox.Password);
            }

            if (!await validators.ValidateBoxes(MinesEntryBox, TilesEntryBox, BetEntryBox)) return;

            if (!hasStarted)
            {
                Button startButton = sender as Button;
                if (startButton != null)
                {
                    if (startButton.Style == (Style)FindResource("StartButtonStyle"))
                    {
                        hasStarted = true;
                        startButton.Style = (Style)FindResource("CashOutButtonStyle");
                        startButton.IsEnabled = false;
                    }
                    else
                    {
                        hasStarted = false;
                        startButton.Style = (Style)FindResource("StartButtonStyle");
                        startButton.IsEnabled = true;
                    }
                }
            }
        }

        private void TopBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void PredictButton_Click(object sender, RoutedEventArgs e)
        {
            if (!hasStarted) return;

            Debug.WriteLine("run play button");

            int numberOfTiles;
            try
            {
                numberOfTiles = int.Parse(TilesEntryBox.Text);
            }
            catch
            {
                numberOfTiles = tiles;
            }

            int totalGridCells = MinesGrid.Children.OfType<Button>().Count();
            Debug.WriteLine(numberOfTiles);
            Debug.WriteLine(totalGridCells);

            List<int> randomPositions = generator.GenerateRandomPositions(numberOfTiles, totalGridCells);

            string currentPath = Directory.GetCurrentDirectory();
            string MainPath = Path.Combine(currentPath, @"..\..\..");
            string settingsPath = Path.Combine(MainPath, @"Images\");

            string defaultImagePath = Path.Combine(settingsPath, "default.png");
            string goodImagePath = Path.Combine(settingsPath, "good.png");

            int currentPositionIndex = 0;
            foreach (Button button in MinesGrid.Children.OfType<Button>())
            {
                Image iconImage = navHelper.FindChild<Image>(button, "IconImage");
                if (iconImage != null)
                {
                    if (iconImage.Source != null && iconImage.Source.ToString().Contains("good.png")) 
                    {
                        Debug.WriteLine($"{currentPositionIndex} is good");
                        iconImage.Source = new BitmapImage(new Uri(defaultImagePath, UriKind.RelativeOrAbsolute));
                    }

                    foreach (int spot in randomPositions)
                    {
                        if (currentPositionIndex == spot)
                        {
                            iconImage.Source = new BitmapImage(new Uri(goodImagePath, UriKind.RelativeOrAbsolute));
                        }
                    }
                }
                currentPositionIndex++;
            }
        }
    }
}
