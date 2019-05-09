using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Xml;

namespace GetWeatherWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebClient webClient = new WebClient();

            XmlDocument xmlDocument = new XmlDocument();

            try
            {

                xmlDocument.LoadXml(webClient.DownloadString("https://api.openweathermap.org/data/2.5/forecast?q="+ cityNameTextBox.Text + "&mode=xml&units=metric&APPID=9472a067887fe996e01e48bb94e8efee"));
                XmlElement rootElement = xmlDocument.DocumentElement;
                XmlNode elementForecast;
                int i = 0;
                elementForecast = rootElement.GetElementsByTagName("forecast")[0];

                foreach (XmlElement element in elementForecast.ChildNodes)
                {
              
                    if (element.Attributes["from"].Value.Contains("12:00:00"))
                    {
                        i++;
                        switch (i){
                            case 1: SetInfo(element, Date1, Image1, Temperature1, Humiwind1); break;
                            case 2: SetInfo(element, Date2, Image2, Temperature2, Humiwind2); break;
                            case 3: SetInfo(element, Date3, Image3, Temperature3, Humiwind3); break;
                            case 4: SetInfo(element, Date4, Image4, Temperature4, Humiwind4); break;
                            case 5: SetInfo(element, Date5, Image5, Temperature5, Humiwind5); break;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

        }
        private void SetInfo(XmlElement element, TextBlock date, Image image, TextBlock temperature, TextBlock humiwind)
        {
            string forecast = element.Attributes["from"].Value;
            int year = int.Parse(forecast.Substring(0, 4));
            int month = int.Parse(forecast.Substring(5, 2));
            int day = int.Parse(forecast.Substring(8, 2));
            DateTime dateWork = new DateTime(year, month, day);

            string dayCyr = "";
            switch (dateWork.DayOfWeek.ToString())
            {
                case "Monday": dayCyr = "понедельник"; break;
                case "Tuesday": dayCyr = "вторник"; break;
                case "Wednesday": dayCyr = "среда"; break;
                case "Thursday": dayCyr = "четверг"; break;
                case "Friday": dayCyr = "пятница"; break;
                case "Saturday": dayCyr = "суббота"; break;
                case "Sunday": dayCyr = "воскресенье"; break;
            }
            date.Text = dateWork.ToShortDateString() + ", " + dayCyr;

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri("http://openweathermap.org/img/w/" + element.GetElementsByTagName("symbol")[0].Attributes["var"].Value + ".png", UriKind.Absolute);
            bmp.EndInit();
            image.Source = bmp;

            string symbol = element.GetElementsByTagName("symbol")[0].Attributes["name"].Value;
            image.ToolTip = symbol.Substring(0, 1).ToUpper() + symbol.Substring(1).ToLower();
            
            temperature.Text = "температура: " + element.GetElementsByTagName("temperature")[0].Attributes["value"].Value + " град. Цельсия";
            humiwind.Text = "влажность: " + element.GetElementsByTagName("humidity")[0].Attributes["value"].Value + "%, "
                            + "ветер: " + element.GetElementsByTagName("windDirection")[0].Attributes["name"].Value;

        }
    }
}
