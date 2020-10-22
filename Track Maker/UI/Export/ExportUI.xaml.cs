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
using System.Windows.Shapes;

namespace Track_Maker
{

    public enum FormatType { Export, Import }
    /// <summary>
    /// Interaction logic for ExportUI.xaml
    /// </summary>
    public partial class ExportUI : Window
    {
        public IExportFormat ExpFormat { get; set; }
        public MainWindow MnWindow { get; set; }
        public FormatType Type { get; set; }
        public List<Storm> StormsToExport { get; set; }
        public ExportUI(FormatType FType, IExportFormat ExportFormat)
        {

            if (ExportFormat.AutoStart)
            {
                this.Visibility = Visibility.Hidden; //try to make it never appear if autostart is true. 
            }

            InitializeComponent();
            Export_Init(FType, ExportFormat);
   

        }

        private void Export_Init(FormatType FType, IExportFormat ExportFormat)
        {
            MnWindow = (MainWindow)Application.Current.MainWindow;
            ExpFormat = ExportFormat;
            StormsToExport = MnWindow.CurrentProject.SelectedBasin.GetFlatListOfStorms(); // feature pushed back to Dano, maybe even 3.0/"Aurora"
            Type = FType;

            if (!ExportFormat.DisplayQualityControl)
            {
                HideQualityControl();
            }

                //completely different in Dano
                //ExportFormat.GeneratePreview(ExportPlatform_Preview);
                //ExportPlatform_Preview.UpdateLayout(); 
        }

        // Set the header using the Export Platform. 
        private void Setup()
        {
            if (ExpFormat.AutoStart) // AutoStart for export-only no-preview formats. 
            {
                MnWindow.TickTimer.Stop();

                switch (Type)
                {
                    case FormatType.Import:
                        // Dano: rewrite

                        MnWindow.CurrentProject = ExpFormat.Import();
                        MnWindow.TickTimer.Start();
                        Close();
                        return; 
                    case FormatType.Export:
                        ExpFormat.Export(MnWindow.CurrentProject);
                        MnWindow.TickTimer.Start();
                        Close();
                        return; 
                }

                
                return;
            }

            // Set the "Export to..." text based on the ExportFormat's Name and the Type supplied to us in the constructor. 
            switch (Type)
            {
                case FormatType.Import:
                    ExportPlatform_ExportBtn.Content = "Import";
                    ExportPlatform.Text = $"Import from {ExpFormat.GetName()}";
                    Title = $"Import from {ExpFormat.GetName()}";
                    return;
                case FormatType.Export:
                    ExportPlatform.Text = $"Export to {ExpFormat.GetName()}";
                    Title = $"Export to {ExpFormat.GetName()}";
                    return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setup();
        }

        private void ExportPlatform_ExportBtn_Click(object sender, RoutedEventArgs e)
        {

            // Temporarily uncommented
            // Old code - dano move when we can do easy previews due to multitab
            // Stop the ticktimer while importing or exporting because we need to do this stuff.
            MnWindow.TickTimer.Stop();

            switch (Type)
            {
                case FormatType.Import:
                    MnWindow.CurrentProject = ExpFormat.Import();
                    MnWindow.TickTimer.Start();
                    Close();
                    return; 
                case FormatType.Export:
                    ExpFormat.Export(MnWindow.CurrentProject);
                    MnWindow.TickTimer.Start();
                    Close();
                    return;
            } 
            
            return;
        }

        private void HideQualityControl()
        {
            Height = 474;
            Width = 918;
            ExportPlatform.Margin = new Thickness(10, 89, 0, 0);
            ExportPlatform_PreviewBorder.Margin = new Thickness(10, 118, 0, 0);
            ExportPlatform_ExportBtn.Margin = new Thickness(794, 381, 0, 0);

            // Make the quality control uninteractable and invisible
            QualityControl.Height = 0;
            QualityControl.Width = 0;
            QualityControl.IsEnabled = false;

        }
    }
}
