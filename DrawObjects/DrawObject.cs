using System;
using System.Diagnostics;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

namespace DrawLine
{
    public class DrawObject
    {
        [CommandMethod("shapr")]
        public void StartApplication()
        {
            var document = Application.DocumentManager.MdiActiveDocument;
            var database = document.Database;

            using (var transaction = database.TransactionManager.StartTransaction())
            {
                try
                {
                    var blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    if (blockTable != null)
                    {
                        var blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        Point3d lineStartPoint = new Point3d(0, 0, 0);
                        Point3d lineEndPoint = new Point3d(100, 100, 0);

                        Line line = new Line(lineStartPoint, lineEndPoint);
                        line.Color = Color.FromRgb(0, 200, 200);
                        if (blockTableRecord != null) blockTableRecord.AppendEntity(line);
                        transaction.AddNewlyCreatedDBObject(line, true);
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Abort();
                }
            }
        }
    }
}