using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using Creation = Autodesk.Revit.Creation;

namespace PartCreator
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class InternalProgram : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            PublicVariables.SetUiApplication(commandData.Application);
            PublicVariables.SetUiDocument(commandData.Application.ActiveUIDocument);
            RevitOperation revOp = new RevitOperation();
            revOp.Operation();

            return Result.Succeeded;
        }
    }
}
