using System;
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
    class PublicVariables
    {
        public static UIApplication UIApp { get; private set; }
        public static  Application App { get; private set; }
        public static UIDocument UIDoc { get; private set; }
        public static Document Doc { get; private set; }



        public static double f2m(double feets)
        {
            return feets * 0.3049;
        }

        public static double m2f(double meters)
        {
            return meters * 3.28084;
        }

        public static void SetUiApplication(UIApplication inputUIApp)
        {
            UIApp = inputUIApp;
            App = UIApp.Application;
        }
        public static void SetUiDocument(UIDocument inputUIDoc)
        {
            UIDoc = inputUIDoc;
            Doc = UIDoc.Document;
        }

        public static void Initialize(UIApplication inputUIApp, UIDocument inputUIDoc)
        {
            SetUiApplication(inputUIApp);
            SetUiDocument(inputUIDoc);
        }

    }
}
