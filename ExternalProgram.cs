using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Creation = Autodesk.Revit.Creation;

namespace PartCreator
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalProgram : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("Prajwal's Apps");
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Prajwal's Apps", "New Tools");
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dllPath = Path.Combine(appData, @"Autodesk\REVIT\Addins\2021\PartCreator.dll");
            PushButtonData pushButton1 = new PushButtonData("Test1", "PartCreator", dllPath, "PartCreator.InternalProgram");
            pushButton1.AvailabilityClassName = "PartCreator.AvailabilityControl";
            ribbonPanel.AddItem(pushButton1);
            return Result.Succeeded;
        }
    }

    public class AvailabilityControl : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return true;
        }
    }
}
