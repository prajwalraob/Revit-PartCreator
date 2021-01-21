using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using PartCreator.Views;

namespace PartCreator
{
    class RevitOperation
    {
        public bool Operation()
        {
            MainWindow wnd = new MainWindow();
            wnd.ShowDialog();
            return true;
        }
    }
}
