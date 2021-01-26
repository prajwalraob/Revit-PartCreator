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
    public class CreateDividedParts
    {
        Floor Floor { get; set; }
        List<Line> BoundingSquare { get; set; }

        public CreateDividedParts(Floor floor, List<Line> boundingSquare)
        {
            this.Floor = floor;
            this.BoundingSquare = boundingSquare;
        }

        public bool Create()
        {
            IList<ElementId> idList = new List<ElementId>();
            ICollection<ElementId> elementIdsToDivide = new List<ElementId>();

            idList.Add(Floor.Id);
            if (PartUtils.AreElementsValidForCreateParts(PublicVariables.Doc, idList))
            {
                using(Transaction tr = new Transaction(PublicVariables.Doc, "CreatePart"))
                {
                    tr.Start();
                    try
                    {
                        PartUtils.CreateParts(PublicVariables.Doc, idList);
                        tr.Commit();
                    }
                    catch
                    {
                        tr.RollBack();
                    }
                    elementIdsToDivide = PartUtils.GetAssociatedParts(PublicVariables.Doc
                        , Floor.Id, true, true);
                }
            }

            ICollection<ElementId> intersectingReferenceIds = new List<ElementId>();
            var curveList = (from Curve elem in BoundingSquare
                             select elem).ToList();

            using(Transaction tr = new Transaction(PublicVariables.Doc, "DivideParts"))
            {
                tr.Start();
                try
                {
                    SketchPlane sp = SketchPlane.Create(PublicVariables.Doc,
                        Plane.CreateByNormalAndOrigin(new XYZ(0, 0, 1), BoundingSquare[0].GetEndPoint(0)));

                    PartMaker maker = PartUtils.DivideParts(PublicVariables.Doc, 
                        elementIdsToDivide, intersectingReferenceIds, curveList, sp.Id);

                    Parameter partVisInView = PublicVariables.Doc.ActiveView.get_Parameter(BuiltInParameter.VIEW_PARTS_VISIBILITY);
                    partVisInView.Set(0);
                    tr.Commit();
                }
                catch
                {
                    tr.RollBack();
                }
            }

            return true;
        }
    }
}
