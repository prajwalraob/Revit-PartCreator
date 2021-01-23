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
        public ModelLine Line1
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
        public ModelLine Line2
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
        public ModelLine Line3
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
        public ModelLine Line4
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

            for(int k = 1; k <=2; k++)
            {
                var reference = PublicVariables.UIDoc.Selection.PickObject(ObjectType.Element);
                typeof(ViewModel).GetProperty("Line" + k.ToString()).SetValue(this, PublicVariables.Doc.GetElement(reference) as ModelLine);
            }

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

            mainWindow.ShowDialog();
        }

        public void PartCreate(object wnd)
        {
            CreatePart create = new CreatePart(Line1, Line2, Line3, Line4);
            create.Run();
        }

        public bool CanExecutePartCreate(object wnd)
        {
            return (Line1 != null && Line2 != null && Line3 != null && Line4 != null);
        }
    }
}
