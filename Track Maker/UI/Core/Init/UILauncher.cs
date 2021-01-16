using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMaker.Core;

namespace Track_Maker
{
    public partial class MainWindow
    {
        private bool InitExportUI<T>(FormatType FType) where T : new()
        {
            T TInstance = new T();

            Type Typ = TInstance.GetType();

            if (!typeof(IExportFormat).IsAssignableFrom(Typ) && !typeof(IImageExportFormat).IsAssignableFrom(Typ))
            {
                Error.Throw("Error", "Cannot pass non-IExportFormats to ExportUI!", ErrorSeverity.FatalError, 333);
                return false; // will never run
            }

            if (FType == FormatType.Export)
            {
                Basin CurrentBasin = CurrentProject.SelectedBasin;

                List<Storm> StormList = CurrentBasin.GetFlatListOfStorms();

                if (StormList.Count == 0)
                {
                    Error.Throw("Warning", "You must have at least one storm to export!", ErrorSeverity.Warning, 340);
                    return false;
                }
                else
                {
                    InitExportUI_OnSuccess(FType, TInstance);
                    return true;
                }
            }
            else
            {
                InitExportUI_OnSuccess(FType, TInstance);
                return true;
            }
        }

        private void InitExportUI_OnSuccess<T>(FormatType FType, T TInstance) where T : new()
        {
            ExportUI ExUI = new ExportUI(FType, (IExportFormat)TInstance);
            ExUI.Owner = this;
            ExUI.Show();
        }
    }
}
