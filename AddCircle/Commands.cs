
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace AddCircle
{
    namespace AddCircle
    {
        public class Commands
        {
            [CommandMethod("DrawCircle")]
            public void Hello()
            {
                var doc = AcAp.DocumentManager.MdiActiveDocument;
                var db = doc.Database;
                var ed = doc.Editor;
                {
                    PromptPointOptions prmptPtOpt = new PromptPointOptions("Select Point");
                    PromptPointResult ptRes = ed.GetPoint(prmptPtOpt);
                    Point3d pt = Point3d.Origin;
                    if(ptRes.Status == PromptStatus.OK)
                    {
                        pt = ptRes.Value;
                    }

                    PromptDoubleOptions prmptDblOpt = new PromptDoubleOptions("Enter Radius");
                    PromptDoubleResult dblRes = ed.GetDouble(prmptDblOpt);
                    double radius = 0;
                    if (dblRes.Status == PromptStatus.OK)
                    {
                        radius = dblRes.Value;
                    }
                    if (radius > 0 && pt != Point3d.Origin)
                    {
                        using (var tr = db.TransactionManager.StartTransaction())
                        {
                            //draw circle
                            Circle circle = new Circle(pt, Vector3d.ZAxis, radius);
                            // grab the block table
                            var blockTable = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                            // grab the model space block record from the block table using the model space objectID
                            var modelSpace
                                = (BlockTableRecord)tr
                                .GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                            // append the circle to the model space block record
                            var objectId = modelSpace.AppendEntity(circle);
                            tr.AddNewlyCreatedDBObject(circle, true);
                            tr.Commit();
                        }
                    }
                }
                
            }
            [CommandMethod("DrawLine")]
            public void Test2()
            {
                var doc = AcAp.DocumentManager.MdiActiveDocument;
                var db = doc.Database;
                var ed = doc.Editor;
                PromptPointOptions prmptPtOpt = new PromptPointOptions("Select start point");
                PromptPointResult ptRes = ed.GetPoint(prmptPtOpt);
                Point3d pt = Point3d.Origin;
                if (ptRes.Status.Equals(PromptStatus.OK))
                {
                    pt = ptRes.Value;
                }
                prmptPtOpt.Message = "Select end point";
                prmptPtOpt.BasePoint = pt;
                prmptPtOpt.UseDashedLine = true;
                ptRes = ed.GetPoint(prmptPtOpt);
                Point3d endPt = Point3d.Origin;
                if (ptRes.Status.Equals(PromptStatus.OK))
                {
                    endPt = ptRes.Value;
                }

                using (var tr = db.TransactionManager.StartTransaction())
                {
                    Line line = new Line(pt, endPt);
                    var blockTable = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    var modelSpace
                        = (BlockTableRecord)tr
                        .GetObject(blockTable[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite);
                    var objectId = modelSpace.AppendEntity(line);
                    tr.AddNewlyCreatedDBObject(line, true);
                    tr.Commit();
                }
            }
        }

    }
}
