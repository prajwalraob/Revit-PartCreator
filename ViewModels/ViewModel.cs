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

namespace PartCreator.ViewModels
{
    public class ViewModel : NotifyPropertyChanged
    {
        public Command SelectSet1Command { get; set; }
        public Command SelectSet2Command { get; set; }
        public Command PartCreateCommand { get; set; }

        private bool _isCreateButtonEnabled;
        public bool IsCreateButtonEnabled
        {
            get => _isCreateButtonEnabled;
            set
            {
                _isCreateButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private ModelLine _line1;
        private ModelLine Line1
        {
            get => _line1;
            set
            {                
                _line1 = value;
                if(Line1 != null && Line2 != null && Line3 != null && Line4 != null)
                {
                    IsCreateButtonEnabled = true;
                }
            }
        }

        private ModelLine _line2;
        private ModelLine Line2
        {
            get => _line2;
            set
            {
                _line2 = value;
                if (Line1 != null && Line2 != null && Line3 != null && Line4 != null)
                {
                    IsCreateButtonEnabled = true;
                }
            }
        }

        private ModelLine _line3;
        private ModelLine Line3
        {
            get => _line3;
            set
            {
                _line3 = value;
                if (Line1 != null && Line2 != null && Line3 != null && Line4 != null)
                {
                    IsCreateButtonEnabled = true;
                }
            }
        }

        private ModelLine _line4;
        private ModelLine Line4
        {
            get => _line4;
            set
            {
                _line4 = value;
                if (Line1 != null && Line2 != null && Line3 != null && Line4 != null)
                {
                    IsCreateButtonEnabled = true;
                }
            }
        }

        public ViewModel()
        {
            SelectSet1Command = new Command (SelectSet1);
            SelectSet2Command = new Command (SelectSet2);
            PartCreateCommand = new Command(PartCreate, CanExecutePartCreate);
        }

        public void SelectSet1(object wnd)
        {
            MainWindow mainWindow = wnd as MainWindow;
            mainWindow.Hide();
            var refs = PublicVariables.UIDoc.Selection.PickObjects(ObjectType.Element);
            if(refs.ElementAtOrDefault(0) != null)
            {
                Line1 = PublicVariables.Doc.GetElement(refs[0].ElementId) as ModelLine;
            }
            if (refs.ElementAtOrDefault(1) != null)
            {
                Line2 = PublicVariables.Doc.GetElement(refs[1].ElementId) as ModelLine;
                TaskDialog.Show("Elements Selected", "Line1 and Line2 selected");
            }
            mainWindow.ShowDialog();
        }
        
        public void SelectSet2(object wnd)
        {
            MainWindow mainWindow = wnd as MainWindow;
            mainWindow.Hide();
            var refs = PublicVariables.UIDoc.Selection.PickObjects(ObjectType.Element);
            if (refs.ElementAtOrDefault(0) != null)
            {
                Line3 = PublicVariables.Doc.GetElement(refs[0].ElementId) as ModelLine;
            }
            if (refs.ElementAtOrDefault(1) != null)
            {
                Line4 = PublicVariables.Doc.GetElement(refs[1].ElementId) as ModelLine;
                TaskDialog.Show("Elements Selected", "Line3 and Line4 selected");
            }
            mainWindow.ShowDialog();
        }

        public void PartCreate(object wnd)
        {

        }

        public bool CanExecutePartCreate(object wnd)
        {
            return (Line1 != null && Line2 != null && Line3 != null && Line4 != null);
        }
    }
}
