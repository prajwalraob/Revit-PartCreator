using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using Forms = System.Windows.Forms;
using Microsoft.Win32;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using Creation = Autodesk.Revit.Creation;
using System.Collections.ObjectModel;
using PartCreator.Views;
using PartCreator.Process;

namespace PartCreator.ViewModels
{
    public class ViewModel : NotifyPropertyChanged
    {
        public Command SelectSet1Command { get; set; }
        public Command SelectSet2Command { get; set; }
        public Command PartCreateCommand { get; set; }
        public Command SelectFloorCommand { get; set; }

        public ModelLine Line1 { get; set; }
        public ModelLine Line2 { get; set; }
        public ModelLine Line3 { get; set; }
        public ModelLine Line4 { get; set; }
        public Floor SelectedFloor { get; set; }

        public ViewModel()
        {
            SelectSet1Command = new Command (SelectSet1);
            SelectSet2Command = new Command (SelectSet2);
            PartCreateCommand = new Command(PartCreate, CanExecutePartCreate);
            SelectFloorCommand = new Command(SelectFloor);
        }

        public void SelectSet1(object wnd)
        {
            MainWindow mainWindow = wnd as MainWindow;
            mainWindow.Hide();

            for(int k = 1; k <=2; k++)
            {
                var reference = PublicVariables.UIDoc.Selection.PickObject(ObjectType.Element);
                typeof(ViewModel).GetProperty("Line" + k.ToString()).SetValue(this, PublicVariables.Doc.GetElement(reference) as ModelLine);
            }

            TaskDialog.Show("Selection", "Selected Lines 1 and 2");
            mainWindow.ShowDialog();
        }
        
        public void SelectSet2(object wnd)
        {
            MainWindow mainWindow = wnd as MainWindow;
            mainWindow.Hide();
            for (int k = 3; k <= 4; k++)
            {
                var reference = PublicVariables.UIDoc.Selection.PickObject(ObjectType.Element);
                typeof(ViewModel).GetProperty("Line" + k.ToString()).SetValue(this, PublicVariables.Doc.GetElement(reference) as ModelLine);
            }

            TaskDialog.Show("Selection", "Selected Lines 3 and 4");
            mainWindow.ShowDialog();
        }

        public void PartCreate(object wnd)
        {
            CreatePart create = new CreatePart(Line1, Line2, Line3, Line4);
            var lines = create.GetBoudingSquare();

            CreateDividedParts dividedParts = new CreateDividedParts(SelectedFloor, lines);
            dividedParts.Create();
        }

        public void SelectFloor(object wnd)
        {
            MainWindow mainWindow = wnd as MainWindow;
            mainWindow.Hide();

            SelectedFloor = PublicVariables.Doc.GetElement(
                PublicVariables.UIDoc.Selection.PickObject(ObjectType.Element)) as Floor;

            mainWindow.ShowDialog();
        }

        public bool CanExecutePartCreate(object wnd)
        {
            return (Line1 != null && Line2 != null && Line3 != null && Line4 != null && SelectedFloor != null);
        }
    }
}
