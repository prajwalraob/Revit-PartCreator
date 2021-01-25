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
            GetIntersectionPoints();
        }

        public bool Run()
        {

            XYZ line1Parallel = Line1.Direction;
            XYZ line1Perpendicular = Transform.CreateRotation(new XYZ(0, 0, 1), Math.PI / 2).OfVector(line1Parallel);
            Line tempLine0 = CreateInfiniteLine(Point1, line1Parallel);
            Line tempLine1 = null;
            Line tempLine2 = null;
            Line tempLine3 = null;
            Line tempCentroidLine = CreateInfiniteLine(GetCentroid(), line1Perpendicular);

            if (Line1.Distance(Point3) > Line1.Distance(Point4))
            {
                tempLine1 = CreateInfiniteLine(Point3, line1Parallel);
            }
            else
            {
                tempLine1 = CreateInfiniteLine(Point4, line1Parallel);
            }

            if(tempCentroidLine.Distance(Point1) > tempCentroidLine.Distance(Point3))
            {
                tempLine2 = CreateInfiniteLine(Point1, line1Perpendicular);
            }
            else
            {
                tempLine2 = CreateInfiniteLine(Point3, line1Perpendicular);
            }

            if (tempCentroidLine.Distance(Point2) > tempCentroidLine.Distance(Point4))
            {
                tempLine3 = CreateInfiniteLine(Point2, line1Perpendicular);
            }
            else
            {
                tempLine3 = CreateInfiniteLine(Point4, line1Perpendicular);
            }

            CreateCurve(tempLine0);CreateCurve(tempLine1);CreateCurve(tempLine2);CreateCurve(tempLine3);



            return true;
        }

        private void GetIntersectionPoints()
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
        }

        private XYZ GetCentroid()
        {
            Line line1 = Line.CreateBound(Line.CreateBound(Point1, Point2).Evaluate(0.5, true),
                Line.CreateBound(Point3, Point4).Evaluate(0.5, true));
            Line line2 = Line.CreateBound(Line.CreateBound(Point1, Point3).Evaluate(0.5, true),
                Line.CreateBound(Point2, Point4).Evaluate(0.5, true));

            //CreateCurve(line1); CreateCurve(line2);
            IntersectionResultArray iresArray;
            SetComparisonResult result = line1.Intersect(line2, out iresArray);
            IntersectionResult ires = iresArray.get_Item(0);
            return ires.XYZPoint;
        }

        private Line CreateInfiniteLine(XYZ point, XYZ direction)
        {
            return Line.CreateBound(point - 10000 * direction, point + 10000 * direction);
        }

        private ModelCurve CreateCurve(Curve curve)
        {
            ModelCurve modelCurve = null;
            using (Transaction tr = new Transaction(PublicVariables.Doc, "Create Test Curve"))
            {
                tr.Start();
                try
                {
                    modelCurve = PublicVariables.Doc.Create.NewModelCurve(curve, PublicVariables.UIDoc.ActiveView.SketchPlane);
                    tr.Commit();
                }
                catch (Exception E)
                {
                    TaskDialog.Show("Error", E.Message);
                    tr.RollBack();
                }
            }

            return modelCurve;
        }



    }
}
