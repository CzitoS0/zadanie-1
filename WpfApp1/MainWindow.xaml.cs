
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TestRefleksu
{
    public partial class MainWindow : Window
    {
        private readonly Random los = new Random();
        private readonly Stopwatch stoper = new Stopwatch();

        private bool trwaTest = false;
        private bool moznaKliknac = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (trwaTest)
                return;

            trwaTest = true;
            moznaKliknac = false;

            SlabyObraz.Visibility = Visibility.Collapsed;
            StartButton.IsEnabled = false;

            Wynik.Text = "Twoj czas: --- ms";
            Instrukcja.Text = "Poczekaj na zielony kolor";
            Pole.Content = "CZEKAJ";
            Pole.Background = Brushes.Red;

            int opoznienie = los.Next(2000, 5001);
            await Task.Delay(opoznienie);

            if (!trwaTest)
                return;

            Pole.Background = Brushes.Green;
            Pole.Content = "KLIKNIJ!";
            Instrukcja.Text = "TERAZ!";

            stoper.Restart();
            moznaKliknac = true;
        }

        private void Pole_Click(object sender, RoutedEventArgs e)
        {
            if (moznaKliknac)
            {
                stoper.Stop();
                moznaKliknac = false;
                trwaTest = false;

                long czas = stoper.ElapsedMilliseconds;
                Wynik.Text = "Twoj czas: " + czas + " ms";

                SlabyObraz.Visibility = czas > 400
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                Instrukcja.Text = "Synku jestes kiepski";
                Pole.Content = "GOTOWY?";
                Pole.Background = Brushes.Gray;
                StartButton.IsEnabled = true;
            }
            else if (trwaTest)
            {
                trwaTest = false;
                moznaKliknac = false;

                Wynik.Text = "Zbyt predko!";
                Instrukcja.Text = "Sprobuj jeszcze raz.";
                Pole.Content = "GOTOWY?";
                Pole.Background = Brushes.Gray;
                StartButton.IsEnabled = true;

                SlabyObraz.Visibility = Visibility.Collapsed;
            }
        }

        private void Galeria_Click(object sender, RoutedEventArgs e)
        {
            Galeria okno = new Galeria();
            okno.Owner = this;

            Hide();
            okno.ShowDialog();
            Show();
        }


        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window okno = new Window
                {
                    Title = "CZOPEK XD",
                    WindowStyle = WindowStyle.None,
                    WindowState = WindowState.Maximized,
                    Background = Brushes.Black,
                    Topmost = true
                };

                var obraz = new System.Windows.Controls.Image
                {
                    Stretch = Stretch.Uniform,
                    Source = new System.Windows.Media.Imaging.BitmapImage(
                        new Uri("pack://application:,,,/czoperk.png",
                                UriKind.Absolute))
                };

                okno.Content = obraz;

                okno.KeyDown += (s, args) =>
                {
                    if (args.Key == System.Windows.Input.Key.Escape)
                        okno.Close();
                };

                okno.MouseLeftButtonDown += (s, args) => okno.Close();

                okno.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Nie można wczytać zdjęcia czoperk.png.\n\n" + ex.Message,
                    "Błąd zdjęcia",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


    }
}


