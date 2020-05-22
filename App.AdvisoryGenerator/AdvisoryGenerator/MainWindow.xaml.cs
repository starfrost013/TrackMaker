using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Dano.AdvisoryGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdvMainWindow : Window
    {
        public List<Warning> WarningList = new List<Warning>();
        public enum WarningType { TSWatch = 0, TSWarning, HWatch, HWarning }

        public int Mode;

        public List<Forecast> ForecastList; // we need to move this to the advisory class if we want multiple-storms
        public AdvMainWindow()
        {
            InitializeComponent();
            ForecastList = new List<Forecast>();
            Mode = 0;
        }

        public class Advisory
        {
            public string Name {get; set;}
            public string Headline { get; set; }
            public double PositionNS { get; set; }
            public double PositionWE { get; set; }
            public string NearestArea { get; set; }
            public int NearestAreaMeasure { get; set; }
            public string NearestAreaDistance { get; set; }
            public int AdvisoryNumber { get; set; }
            public int Intensity { get; set; }
            public int Gusts { get; set; }
            public int Pressure { get; set; }
            public int ForwardSpeed { get; set; }
            public DateTime AdvisoryDateTime { get; set; }
            public List<Warning> WarningList { get; set; }

        }

        private void SaveToTextFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog SFD = SaveDialog();

            if (SFD == null)
            {
                return;
            }

            Save(SFD.FileName, Mode);

        }

        public SaveFileDialog SaveDialog()
        {
            SaveFileDialog SFD = new SaveFileDialog();
            if (Mode == 0)
            {
                SFD.Title = "Save advisory to text file.";
            }
            else if (Mode == 1)
            {
                SFD.Title = "Save advisory to text file (Wikitext format)";
            }
            else if (Mode == 2)
            {
                SFD.Title = "Save advisory to text file (MCC format)";
            }
            else
            {
                MessageBox.Show("THAT'S A BUG", "UH OH THAT'S BAD",MessageBoxButton.OK,MessageBoxImage.Error);
                Environment.Exit(696969);
            }
            SFD.Filter = "Text documents (.txt)|*.txt";
            SFD.ShowDialog();

            if (SFD.FileName == "") // oh did we not actually want to save?
            {
                return null;
            }
            return SFD;
        }

        public void Save(string path, int mode)
        {
            Advisory advisory = new Advisory();
            try
            {
                
                advisory.Name = StormName.Text;
                advisory.Headline = Headline.Text;
                advisory.PositionNS = Convert.ToDouble(LocationNS.Text);
                advisory.PositionWE = Convert.ToDouble(LocationWE.Text);
                advisory.NearestArea = NearestArea.Text;
                advisory.NearestAreaMeasure = MeasureBox.SelectedIndex;
                advisory.NearestAreaDistance = AwayFrom.Text;
                advisory.AdvisoryNumber = Convert.ToInt32(AdvisoryNum.Text);
                advisory.Intensity = Convert.ToInt32(Intensity.Text);
                advisory.Gusts = Convert.ToInt32(GustsBox.Text);
                advisory.Pressure = Convert.ToInt32(Pressure.Text);
                advisory.ForwardSpeed = Convert.ToInt32(Speed.Text);
                advisory.AdvisoryDateTime = (DateTime)Date.SelectedDate;

                if (Convert.ToInt32(Hours.Text) < 0)
                {
                    MessageBox.Show("An invalid input was entered", "Advisory Generator", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                advisory.AdvisoryDateTime = advisory.AdvisoryDateTime.AddHours(Convert.ToInt32(Hours.Text)); // add the hours
                advisory.AdvisoryDateTime = advisory.AdvisoryDateTime.AddMinutes(Convert.ToInt32(Minutes.Text)); // add the minutes.
                
            }
            catch (FormatException)
            {
                MessageBox.Show("An invalid input was entered", "Advisory Generator", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<string> thePath = new List<string>();
            Random RNG = new Random();
            if (mode == 0)
            {
                thePath.Add($"000\nWTNT34 KNHC {RoundNearest(RNG.Next(99999, 200001), 10)}\nTCPAT4\n");
                thePath.Add("BULLETIN");

                if (advisory.Intensity < 39)
                {
                    thePath.Add($"Tropical Depression {advisory.Name.ToUpper()} Advisory Number {advisory.AdvisoryNumber.ToString()}");
                }
                else if (advisory.Intensity > 39 & advisory.Intensity < 74)
                {
                    thePath.Add($"Tropical Storm {advisory.Name.ToUpper()} Advisory Number {advisory.AdvisoryNumber.ToString()}");
                }
                else
                {
                    thePath.Add($"Hurricane {advisory.Name.ToUpper()} Advisory Number {advisory.AdvisoryNumber.ToString()}");
                }
                thePath.Add($"{advisory.Headline}\n\n");
                thePath.Add($"SUMMARY OF {advisory.AdvisoryDateTime.ToString()} UTC...INFORMATION"); // aaaaa
                thePath.Add($"------------------------");
                thePath.Add($"LOCATION...{advisory.PositionNS.ToString()}{LocationNSBox.Text} {advisory.PositionWE.ToString()}{LocationWEBox.Text}...APPROXIMATELY {NearestArea.Text} {MeasureBox.Text} AWAY FROM {AwayFrom.Text}"); //yes you can get the text of a combobox
                thePath.Add($"MAXIMUM SUSTAINED WINDS...{advisory.Intensity.ToString()} MPH...{(RoundNearest(advisory.Intensity * 1.60934, 5)).ToString()} KM/H");
                thePath.Add($"PRESENT MOVEMENT...{advisory.ForwardSpeed.ToString()} MPH {(RoundNearest(advisory.ForwardSpeed * 1.60934, 5).ToString())} KM/H");
                thePath.Add($"MINIMUM CENTRAL PRESSURE...{advisory.Pressure.ToString()} MB...{(RoundNearest(advisory.Pressure * 0.0295301, 0.01)).ToString()} INCHES\n");

                advisory.WarningList = WarningList;

                if (advisory.WarningList.Count > 0)
                {
                    thePath.Add("WATCHES AND WARNINGS");
                    thePath.Add("--------------------\n");
                    foreach (Warning warning in advisory.WarningList)
                    {
                        if (warning.Text == "none") // kek
                        {
                            thePath.Add($"A {warning.Type.ToUpper()} HAS BEEN ISSUED BY THE {warning.IssuingAgency.ToUpper()} FROM {warning.AreaFrom.ToUpper()} TO {warning.AreaTo.ToUpper()}.");
                        }
                        else
                        {
                            thePath.Add($"A {warning.Type.ToUpper()} HAS BEEN ISSUED BY THE {warning.IssuingAgency.ToUpper()} FROM {warning.AreaFrom.ToUpper()} TO {warning.AreaTo.ToUpper()} - {warning.Text.ToUpper()}.");
                        }

                    }
                }


            }
            else if (mode == 1)
            {
                string colour; // temporary.
                thePath.Add("{{Infobox advisory");
                if (advisory.Intensity < 39)
                {
                    colour = "5ebaff";
                    thePath.Add($"| TCName = [[File:Tropical Depression symbol Layten.png|12px]] <span style=\"color: #5ebaff;\"> {advisory.Name} </span>");
                }
                else if (advisory.Intensity > 39 & advisory.Intensity < 74)
                {
                    colour = "00faf4";
                    thePath.Add($"| TCName = [[File:Tropical Storm symbol Layten.png|12px]] <span style=\"color: #00faf4;\"> {advisory.Name} </span>");
                }
                else if (advisory.Intensity > 74 & advisory.Intensity < 96)
                {
                    colour = "ffffcc";
                    thePath.Add($"| TCName = [[File:Category 1 symbol Layten.png|12px]] <span style=\"color: #ffffcc;\"> {advisory.Name} </span>");
                }
                else if (advisory.Intensity > 96 & advisory.Intensity < 111)
                {
                    colour = "ffe775";
                    thePath.Add($"| TCName = [[File:Category 2 symbol Layten.png|12px]] <span style=\"color: #ffe775;\"> {advisory.Name} </span>");
                }
                else if (advisory.Intensity > 111 & advisory.Intensity < 130)
                {
                    colour = "ffc140";
                    thePath.Add($"| TCName = [[File:Category 3 symbol Layten.png|12px]] <span style=\"color: #ffc140;\"> {advisory.Name} </span>");
                }
                else if (advisory.Intensity > 130 & advisory.Intensity < 157)
                {
                    colour = "ff8f20";
                    thePath.Add($"| TCName = [[File:Category 4 symbol Layten.png|12px]] <span style=\"color: #ff8f20;\"> {advisory.Name} </span>");
                }
                else
                {
                    colour = "ff6060";
                    thePath.Add($"| TCName = [[File:Category 5 symbol Layten.png|12px]] <span style=\"color: #ff6060;\"> {advisory.Name} </span>");
                }

                thePath.Add($"| Advisory = {advisory.Headline}");
                thePath.Add($"| Time = {advisory.AdvisoryDateTime} (Advisory #{advisory.AdvisoryNumber})");
                thePath.Add($"| Movement = {advisory.ForwardSpeed} mph");
                thePath.Add($"| Pressure = {advisory.Pressure} mbar");
                thePath.Add($"| Winds = {advisory.Intensity} mph (gusting to {advisory.Gusts} mph)");
                thePath.Add($"| Location = {advisory.PositionNS}{LocationNSBox.Text} {advisory.PositionWE}{LocationWEBox.Text}");
                thePath.Add($"| Colour = {colour}");
                thePath.Add("}}");
            }
            else if (mode == 2)
            {
                if (advisory.Intensity < 39)
                {
                    thePath.Add($"MEDISTORM {advisory.Name}");
                }
                else if (advisory.Intensity > 39 & advisory.Intensity < 57)
                {
                    thePath.Add($"SEVERE MEDISTORM {advisory.Name}");
                }
                else if (advisory.Intensity > 57 & advisory.Intensity < 74)
                {
                    thePath.Add($"MEDICANE {advisory.Name}");
                }
                else if (advisory.Intensity > 74 & advisory.Intensity < 96)
                {
                    thePath.Add($"MAJOR MEDICANE {advisory.Name}");
                }
                else
                {
                    thePath.Add($"CATASTROPHIC MEDICANE {advisory.Name}");
                }
                thePath.Add($"Advisory number {advisory.AdvisoryNumber} | {advisory.AdvisoryDateTime}\n");
                thePath.Add(advisory.Headline);
                thePath.Add($"Maximum sustained winds...{advisory.Intensity} / {RoundNearest(advisory.Intensity * 1.60934, 5)} km/h.");
                thePath.Add($"Maximum wind gusts...{advisory.Gusts} / {RoundNearest(advisory.Gusts * 1.60934, 5)} km/h");
                thePath.Add($"Minimum pressure...{advisory.Pressure} mbar");
                thePath.Add($"Current location...{advisory.PositionNS}{LocationNSBox.Text} {advisory.PositionWE}{LocationWEBox.Text}\n");
            }

            if (mode != 1)
            {
                if (ForecastList.Count > 0)
                {
                    thePath.Add("\n");
                    if (mode == 0)
                    {
                        thePath.Add("FORECAST POSITIONS AND MAX WINDS\n"); // yeah
                    }
                    else
                    {
                        thePath.Add("Forecast...");
                    }
                    int hours = 6; // temp
                    int count = 0; // eww


                    //todo: we can make this code better. 
                    foreach (Forecast forecast in ForecastList)
                    {
                        if (forecast.Intensity >= 25)
                        {
                            if (mode < 2)
                            {
                                thePath.Add($"{hours * count}H  {forecast.DateTime.ToString()} {forecast.Position} {RoundNearest(forecast.Intensity / 1.15078, 5)} KT {forecast.Intensity} MPH");
                            }
                            else
                            {
                                thePath.Add($"{hours * count}h...{forecast.Position} {forecast.Intensity} mph / {RoundNearest(forecast.Intensity * 1.60934, 5)} km/h.");
                            }
                        }
                        else
                        {
                            if (mode < 2)
                            {
                                thePath.Add($"{hours * count} DISSIPATED");
                            }
                            else
                            {
                                thePath.Add($"{hours * count}h...{forecast.Position} {forecast.Intensity} mph / {RoundNearest(forecast.Intensity * 1.60934, 5)} km/h...dissipated.");
                            }
                        }
                        count++;
                    }
                }
            }
            Clipboard.SetText(string.Join("",thePath));
            MessageBox.Show("File saved. In addition, the information has been copied to the clipboard.", "Advisory Generator", MessageBoxButton.OK, MessageBoxImage.Information);
            File.WriteAllLines(path, thePath);
        }

        private void FileMenu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void WarningsMenu_AddWarnings_Click(object sender, RoutedEventArgs e)
        {
            WarningManager wm = new WarningManager(this);
            wm.Show();
        }

        private void SaveToWikiText_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog OFD = new SaveFileDialog();
            OFD.Title = "Save advisory to text file (HHW wikitext advisory format)";
            OFD.Filter = "Text documents (.txt)|*.txt";
            OFD.ShowDialog();

            if (OFD.FileName == "") // oh did we not actually want to save?
            {
                return;
            }
            Save(OFD.FileName, 1);
        }

        private void WarningsMenu_ForecastManager_Click(object sender, RoutedEventArgs e)
        {
            if (Date.SelectedDate == null || Hours.Text == "" || Minutes.Text == "")
            {
                MessageBox.Show("You must input a date and time to use the Forecast Manager.", "Forecast Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ForecastManager ForecastManager = new ForecastManager(this);
            ForecastManager.ShowDialog();

        }

        private void TypeMenu_NHC_Click(object sender, RoutedEventArgs e)
        {
            if (TypeMenu_NHC.IsChecked == true)
            {
                TypeMenu_Wikitext.IsChecked = false;
                TypeMenu_MCC.IsChecked = false;
                Mode = 0;
            }
        }

        private void TypeMenu_Wikitext_Click(object sender, RoutedEventArgs e)
        {
            if (TypeMenu_Wikitext.IsChecked == true)
            {
                TypeMenu_NHC.IsChecked = false;
                TypeMenu_MCC.IsChecked = false;
                Mode = 1;
            }
        }

        private void TypeMenu_MCC_Click(object sender, RoutedEventArgs e)
        {
            if (TypeMenu_MCC.IsChecked == true)
            {
                TypeMenu_NHC.IsChecked = false;
                TypeMenu_Wikitext.IsChecked = false;
                Mode = 2;
            }
        }

        private void HelpMenu_About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow AboutWindow = new AboutWindow();
            AboutWindow.Owner = this; 
            AboutWindow.Show();
            return;
        }

        private void HelpMenu_ViewHelp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Readme.txt");
            e.Handled = true;
        }
    }
}
