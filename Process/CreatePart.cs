using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace PartCreator.Process
{
    public class CreatePart
    {
        private Line Line1 { get; set; }
        private Line Line2 { get; set; }
        private Line Line3 { get; set; }
        private Line Line4 { get; set; }

        private XYZ Point1 { get; set; }
        private XYZ Point2 { get; set; }
        private XYZ Point3 { get; set; }
        private XYZ Point4 { get; set; }

        public CreatePart(ModelLine line1, ModelLine line2, ModelLine line3, ModelLine line4)
        {
            Line1 = line1.GeometryCurve as Line;
            Line2 = line2.GeometryCurve as Line;
            Line3 = line3.GeometryCurve as Line;
            Line4 = line4.GeometryCurve as Line;
        }

        public bool Run()
        {
            IntersectionResultArray iresArray1;
            SetComparisonResult result1 = Line1.Intersect(Line3, out iresArray1);
            IntersectionResult ires1 = iresArray1.get_Item(0);
            Point1 = ires1.XYZPoint;

            IntersectionResultArray iresArray2;
            SetComparisonResult result2 = Line1.Intersect(Line4, out iresArray2);
            IntersectionResult ires2 = iresArray2.get_Item(0);
            Point2 = ires2.XYZPoint;

            IntersectionResultArray iresArray3;
            SetComparisonResult result3 = Line2.Intersect(Line3, out iresArray3);
            IntersectionResult ires3 = iresArray3.get_Item(0);
            Point3 = ires3.XYZPoint;

            IntersectionResultArray iresArray4;
            SetComparisonResult result4 = Line2.Intersect(Line4, out iresArray4);
            IntersectionResult ires4 = iresArray4.get_Item(0);
            Point4 = ires4.XYZPoint;

            double dist1 = Line1.Distance(Point3);
            double dist2 = Line1.Distance(Point4);
            Line tempLine1 = null;
            Line tempLine2 = null;
            XYZ line1Parallel = (Line1.GetEndPoint(0) - Line1.GetEndPoint(1)).Normalize();
            XYZ line1Parallel1 = Line1.Direction;
            XYZ line1Perpendicular = Transform.CreateRotation(new XYZ(0, 0, 1), Math.PI / 2).OfVector(line1Parallel);
            Line tempLine0 = Line.CreateBound(Point1 - 10000 * line1Parallel, Point1 + 10000 * line1Parallel);
            Line tempLine3 = null;

            if (dist1 > dist2)
            {
                tempLine1 = Line.CreateBound(Point3 - 10000 * line1Parallel, Point3 + 10000 * line1Parallel);
                tempLine2 = Line.CreateBound(Point4 - 10000 * line1Perpendicular, Point4 + 10000 * line1Perpendicular);
                double dist3 = tempLine2.Distance(Point1);
                double dist4 = tempLine2.Distance(Point3);
                if(dist3 > dist4)
                {
                    tempLine3 = Line.CreateBound(Point1 - 10000 * line1Perpendicular, Point1 + 10000 * line1Perpendicular);
                }
                else
                {
                    tempLine3 = Line.CreateBound(Point3 - 10000 * line1Perpendicular, Point3 + 10000 * line1Perpendicular);
                }
            }
            else
            {
                tempLine1 = Line.CreateBound(Point4 - 10000 * line1Parallel, Point4 + 10000 * line1Parallel);
                tempLine2 = Line.CreateBound(Point3 - 10000 * line1Perpendicular, Point3 + 10000 * line1Perpendicular);
                double dist3 = tempLine2.Distance(Point2);
                double dist4 = tempLine2.Distance(Point4);
                if (dist3 > dist4)
                {
                    tempLine3 = Line.CreateBound(Point2 - 10000 * line1Perpendicular, Point2 + 10000 * line1Perpendicular);
                }
                else
                {
                    tempLine3 = Line.CreateBound(Point4 - 10000 * line1Perpendicular, Point4 + 10000 * line1Perpendicular);
                }
            }

            IntersectionResultArray resultArray;
            IntersectionResult result = null;
            SetComparisonResult compResult = tempLine1.Intersect(tempLine2, out resultArray);
            if (compResult == SetComparisonResult.Overlap) result = resultArray.get_Item(0);
            


            using (Transaction tr = new Transaction(PublicVariables.Doc, "Create Test Line"))
            {
                tr.Start();
                try
                {
                    PublicVariables.Doc.Create.NewModelCurve(tempLine0, PublicVariables.UIDoc.ActiveView.SketchPlane);
                    PublicVariables.Doc.Create.NewModelCurve(tempLine1, PublicVariables.UIDoc.ActiveView.SketchPlane);
                    PublicVariables.Doc.Create.NewModelCurve(tempLine2, PublicVariables.UIDoc.ActiveView.SketchPlane);
                    PublicVariables.Doc.Create.NewModelCurve(tempLine3, PublicVariables.UIDoc.ActiveView.SketchPlane);
                    tr.Commit();
                }
                catch(Exception E)
                {
                    TaskDialog.Show("Error", E.Message);
                    tr.RollBack();                
                }
            }

            return true;
        }


    }
}
