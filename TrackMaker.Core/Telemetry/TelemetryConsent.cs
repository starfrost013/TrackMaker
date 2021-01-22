using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; 

namespace TrackMaker.Core
{
    public static class TelemetryConsentAcquirer
    {
        /// <summary>
        /// Determine the users' update check/telemetry consent status.
        /// </summary>
        public static void Init_DetermineTelemetryConsentStatus()
        {
            // Ask the user.
            if (ApplicationSettings.TelemetryConsent == TelemetryConsent.NotDone)
            {
                if (MessageBox.Show("The Track Maker has auto-updating functionality.\n\n" +
                    "To determine if an update is available, the Track Maker must connect to the Internet.\n\n" +
                    "As a result of this a small amount of information is sent to the update server,\n" +
                    "and it can be used to determine certain aspects of your activity - for example the date of each Track Maker start.\n" +
                    "ABSOLUTELY NO personal information is sent to the update server and NO PERSONAL INFORMATION WILL EVER BE SENT.\n\n" +
                    "This information is only a byproduct of the auto-update functionality and absolutely no information is\n" +
                    "directly sent by the Track Maker.\n\n" +
                    "Please note that if you choose not to check for updates, you will not be able to automatically update the Track Maker,\n" +
                    "and you must do so manually.\n\n" +
                    "Do you wish to check for updates at each start of the Track Maker?", "Check for Updates?", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Logging.Log("User accepted indirect telemetry, setting TelemetryConsent to TelemetryConsent.Yes...");
                    SettingsAPI.SetSetting("TelemetryConsent", "Yes");
                }
                else
                {
                    Logging.Log("User denied indirect telemetry, setting TelemetryConsent to TelemetryConsent.No...");
                    SettingsAPI.SetSetting("TelemetryConsent", "No");
                }
            }
            else
            {
                if (ApplicationSettings.TelemetryConsent == TelemetryConsent.Yes)
                {
                    Logging.Log("TelemetryConsent = TelemetryConsent.Yes, running updater..."); 
                    RunUpdater();
                }
                else
                {
                    Logging.Log("TelemetryConsent = TelemetryConsent.No, skipping updater...");
                    return;
                }
            }
        }

        private static void RunUpdater() => Process.Start("Updater.exe"); 
    }
}
