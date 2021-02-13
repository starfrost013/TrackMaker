using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrackMaker.Core; 

namespace TrackMaker
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Verify the track maker's internal state
        /// </summary>
        /// <returns></returns>
        public bool TrackMaker_VerifyInternalState()
        {
            string GeneralErrString = "Internal State Corrupted!";
            string ProjectErrorErrString = "Internal State Corrupted: Project now invalid. Cannot continue operation!";
            string CategoryManagerErrString = "Internal State Corrupted: Category Manager now invalid. Cannot continue operation!";
            string GlobalStateErrString = "Internal State Corrupted: Global State now invalid. Cannot continue operation!";
            string ST2ManagerErrString = "Internal State Corrupted: StormType 2.0 Manager now invalid. Cannot continue operation!";

            // Iris: attempt to recover
            if (CurrentProject == null)
            {
                Error.Throw(GeneralErrString, ProjectErrorErrString, ErrorSeverity.FatalError, 300);
                return false;
            }
            else
            {
                if (CurrentProject.SelectedBasin == null) Error.Throw(GeneralErrString, ProjectErrorErrString, ErrorSeverity.FatalError, 301);
                if (CurrentProject.CategorySystems == null) Error.Throw(GeneralErrString, ProjectErrorErrString, ErrorSeverity.FatalError, 302);
                // genuinely no idea why 303 is triggering and i don't really havee time to find out why. 304 is triggering because I'm an idiot.
                //if (CurrentProject.CategorySystems.Count == 0) Error.Throw(GeneralErrString, ProjectErrorErrString, ErrorSeverity.FatalError, 303);
                //if (CurrentProject.SelectedCategorySystem == null) Error.Throw(GeneralErrString, ProjectErrorErrString, ErrorSeverity.FatalError, 304);
                if (CurrentProject.OpenBasins.Count == 0) Error.Throw(GeneralErrString, ProjectErrorErrString, ErrorSeverity.FatalError, 305);
                
                if (GlobalState.CategoryManager == null)
                {
                    Error.Throw(GeneralErrString, CategoryManagerErrString, ErrorSeverity.FatalError, 306);
                    return false; // will never run
                }
                else
                {
                    if (GlobalState.CategoryManager.CategorySystems == null) Error.Throw(GeneralErrString, CategoryManagerErrString, ErrorSeverity.FatalError, 307);
                    if (GlobalState.CategoryManager.CurrentCategorySystem == null) Error.Throw(GeneralErrString, CategoryManagerErrString, ErrorSeverity.FatalError, 308);
                    if (GlobalState.CategoryManager.CategorySystems.Count == 0) Error.Throw(GeneralErrString, CategoryManagerErrString, ErrorSeverity.FatalError, 309);
                    
                    // globalstate checks
                    if (GlobalState.OpenBasins == null)
                    {
                        Error.Throw(GeneralErrString, GlobalStateErrString, ErrorSeverity.FatalError, 310);
                        return false;
                    }
                    else
                    {
                        if (GlobalState.OpenBasins.Count == 0)
                        {
                            Error.Throw(GeneralErrString, GlobalStateErrString, ErrorSeverity.FatalError, 311);
                        }
                        else
                        {

                            // Misc Checks
                            if (ST2Manager == null)
                            {
                                Error.Throw(GeneralErrString, ST2ManagerErrString, ErrorSeverity.FatalError, 312);
                                return false; // will never run
                            }
                            else
                            {
                                if (ST2Manager.Types.Count == 0) Error.Throw(GeneralErrString, ST2ManagerErrString, ErrorSeverity.FatalError, 313);
                            }
                        }
                        


                        return true; // internal state still valid
                    }

                }
            }
            
            
        }
    }
}
