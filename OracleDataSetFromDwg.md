# Details #

_This is taken directly out of a working application. Since Maksim and I are not sure were we wish to go with this section I offer this as the next installment._

In this application snippet, but not shown here, I use a database table to store the information necessary to select and load a DWG from the filesystem. In addition the `LAYERS` field of that table holds a delimited string with the names of the DWG layers to use. This supplies the filter mechanism for entity selection. My preference is to use a hashtable for the actual lookup. The rest should be fairly straightforward to follow.

  * `NextValStackHandler` is a helper class to handle the Oracle sequence values used to provide the primary key. It utilizes a `Stack` object to store `NextVal(s)`.
  * `OraFDOInfo` is another helper class for the store of connection information for ODP and `FDO.LogThis()` is a very, very simple text file logger.

As always code speaks for itself.

After this dataset gets loaded I utilize ODP.Net’s array bound parameters to do the inserts into the table. What a remarkable piece of work and a topic in and of itself! Using my garden variety workstation and a reasonable Oracle database server I am getting approx. 400 inserts per second. (this is not a typo) Compare this with an average of 8 inserts per second utilizing the “one at a time” approach.

# Implementation #

```
public void ProcessDWG(Document doc, string fileName, DataRow _dr, ref DataSet GEOMds)

        {

            Editor ed = doc.Editor;

            Hashtable lht = LayersToHashTable((string)_dr["LAYERS"]);

            string just_the_fileName = System.IO.Path.GetFileName(fileName);

            int floor = System.Convert.ToInt32(_dr["FLOOR"]);

 

            int in_selected_layers = 0, not_in_selected_layers = 0, selected_geom = 0, not_selected_geom = 0;

            TIODWG.DwgReader dwgReader = new Topology.IO.Dwg.DwgReader();

            TG.IGeometry just_read = null;

            Database db = new Database(false, false);

            db.ReadDwgFile(fileName, FileShare.ReadWrite, true, "");          

            ed.WriteMessage("\nLoading dataset ... ");

            

            NextValStackHandler NextValStack = new NextValStackHandler(ConnInfo.NetConnectionString,

                "FCLTY" + (string)_dr["FCLTY"] + "_" + floor.ToString() + OraFDOInfo.KOP_SequenceSuffix, scaled_ents);

            NextValStack.FIFO = true;

            

            using (Transaction tr = db.TransactionManager.StartTransaction())

            {

                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                ObjectId msId_IN = bt[BlockTableRecord.ModelSpace];

                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(msId_IN, OpenMode.ForRead);

                foreach (ObjectId entId in btr)

                {

                    DBObject obj = tr.GetObject(entId, OpenMode.ForRead);

                    Entity ent = obj as Entity;

                    if (ent != null)

                    {

                        if (lht.Contains(ent.Layer))

                        {

                            in_selected_layers = in_selected_layers + 1;

                            try

                            {

                                RXClass rxc = ent.GetRXClass();

                                switch (rxc.Name)

                                {

                                    case "AcDbPoint":

                                    case "AcDbArc":

                                    case "AcDbLine":

                                    case "AcDbPolyline":

                                    case "AcDb2dPolyline":

                                    case "AcDb3dPolyline":

                                    case "AcDbMline":

                                    case "AcDbMPolygon":

                                        try

                                        {

                                            selected_geom = selected_geom + 1;

                                            just_read = dwgReader.ReadGeometry(ent);

                                            double area = just_read.Area;

                                            double perimeter = just_read.Length;

                                            bool validflg = just_read.IsValid;

                                            string geometryType_string = just_read.GeometryType;

                                            just_read.SRID = DEFAULT_SRID;

                                            // conversion to SDO_GEOMETRY

                                            sdogeometry sdogeom = GeometryXlate.fromJTSToSDO(just_read);

                                            int ordinate_count = 0;

                                            bool OK_to_Load = false;

                                            if (sdogeom != null)

                                            {

                                                if (sdogeom.OrdinatesArray != null || sdogeom.sdo_point != null)

                                                    OK_to_Load = true;

                                                if (sdogeom.OrdinatesArray != null)

                                                    ordinate_count = sdogeom.OrdinatesArray.GetLength(0);

                                                if (sdogeom.sdo_point != null)

                                                    ordinate_count = 2;

                                            }

                                            if (ordinate_count == 0)

                                            {

                                                LogThis("\t\tNotLoadable - InvalidOrdinateCount_GeometryHandle: " + ent.Handle.ToString() + " Layer: " + ent.Layer + " Type: " + rxc.Name);

                                            }

                                            if (OK_to_Load == true && ordinate_count >= 2)

                                            {

                                                DataRow r = GEOMds.Tables[0].NewRow();

                                                r["ID"] = NextValStack.NextVal();

                                                r["ALTKEY"] = 0;

                                                r["VALIDFLG"] = 1;

                                                r["NAME"] = geometryType_string;

                                                r["FLOOR"] = floor;

                                                r["SRCNAME"] = just_the_fileName;

                                                r["LAYER"] = ent.Layer;

                                                r["AREASQFT"] = area;

                                                r["PERIMETERFT"] = perimeter;

                                                r["TXTCONTENTS"] = DBNull.Value;

                                                r["DATESTAMP"] = System.DateTime.Now;

                                                r["GEOMETRY"] = sdogeom;

                                                GEOMds.Tables[0].Rows.Add(r);

                                            }

                                        }

                                        catch (TG.TopologyException te)

                                        {

                                            ed.WriteMessage("\nTopologyConversionERROR: " + te.Message);

                                            LogThis("\t\tTopologyConversionERROR: " + te.Message);

                                        }

                                        catch (System.Exception se)

                                        {

                                            ed.WriteMessage("\nSystemConversionERROR: " + se.Message);

                                            LogThis("\t\tSystemConversionERROR: " + se.Message);

                                        }

                                        break;

                                    case "AcDbText":

                                    case "AcDbMText":

                                    case "AcDbBlockReference":

                                        selected_geom = selected_geom + 1;

                                        sdogeometry sdogeom_p = new sdogeometry();

                                        SDOPOINT sdoP = new SDOPOINT();

                                        string txt = null;

                                        if (rxc.Name == "AcDbText")

                                        {

                                            DBText p = ent as DBText;

                                            if (p != null)

                                            {

                                                sdoP.XD = p.Position.X;

                                                sdoP.YD = p.Position.Y;

                                                txt = p.TextString;

                                            }

                                        }

                                        if (rxc.Name == "AcDbMText")

                                        {

                                            MText p = ent as MText;

                                            if (p != null)

                                            {

                                                sdoP.XD = p.Location.X;

                                                sdoP.YD = p.Location.Y;

                                                txt = p.Contents;

                                            }

                                        }

                                        if (rxc.Name == "AcDbBlockReference")

                                        {

                                            BlockReference b = ent as BlockReference;

                                            if (b != null)

                                            {

                                                sdoP.XD = b.Position.X;

                                                sdoP.YD = b.Position.Y;

                                                txt = b.Name;

                                            }

                                        }

                                        sdogeom_p.GeometryType = (int)sdogeometryTypes.GTYPE.POINT;

                                        sdogeom_p.Dimensionality = (int)sdogeometryTypes.DIMENSION.DIM2D;

                                        sdogeom_p.LRS = 0;

                                        sdogeom_p.PropertiesToGTYPE();

                                        sdogeom_p.sdo_point = sdoP;

                                        sdogeom_p.sdo_sridAsInt = DEFAULT_SRID;

 

                                        DataRow rp = GEOMds.Tables[0].NewRow();

                                        rp["ID"] = NextValStack.NextVal();

                                        rp["ALTKEY"] = 0;

                                        rp["VALIDFLG"] = 1;

                                        rp["NAME"] = "Point";

                                        rp["FLOOR"] = floor;

                                        rp["SRCNAME"] = just_the_fileName;

                                        rp["LAYER"] = ent.Layer;

                                        rp["AREASQFT"] = 0.0;

                                        rp["PERIMETERFT"] = 0.0;

                                        rp["TXTCONTENTS"] = txt; ;

                                        rp["DATESTAMP"] = System.DateTime.Now;

                                        rp["GEOMETRY"] = sdogeom_p;

                                        GEOMds.Tables[0].Rows.Add(rp);

                                        break;

                                    case "AcDbCircle":

                                        Circle circ = ent as Circle;

                                        if (circ != null)

                                        {

                                            selected_geom = selected_geom + 1;

                                            double radius = circ.Radius;

                                            Point3d center = circ.Center;

                                            CircularArc2d ca2d = new CircularArc2d(new Point2d(center.X, center.Y), radius);

                                            int max_points = System.Convert.ToInt32(Math.Ceiling(circ.Circumference / 2));

                                            max_points = (max_points < 6) ? 6 : max_points;

                                            Point2d[] pts = EmulateACircle(ca2d, max_points);

                                            sdogeometry sdogeom_c = new sdogeometry();

                                            sdogeom_c.GeometryType = (int)sdogeometryTypes.GTYPE.LINE;

                                            sdogeom_c.Dimensionality = (int)sdogeometryTypes.DIMENSION.DIM2D;

                                            sdogeom_c.LRS = 0;

                                            sdogeom_c.PropertiesToGTYPE();

                                            sdogeom_c.sdo_sridAsInt = DEFAULT_SRID;

                                            int[] elem = new int[3];

                                            elem[0] = 1;

                                            elem[1] = (int)sdogeometryTypes.ETYPE_SIMPLE.LINE;

                                            elem[2] = 1;

                                            sdogeom_c.ElemArrayOfInts = elem;

 

                                            double[] ords = new double[pts.Length * 2];

                                            int ords_cnt = 0;

                                            for (int i = 0; i < pts.Length; i++)

                                            {

                                                ords[ords_cnt++] = pts[i].X;

                                                ords[ords_cnt++] = pts[i].Y;

                                            }

                                            sdogeom_c.OrdinatesArrayOfDoubles = ords;

 

                                            DataRow rc = GEOMds.Tables[0].NewRow();

                                            rc["ID"] = NextValStack.NextVal();

                                            rc["ALTKEY"] = 0;

                                            rc["VALIDFLG"] = 1;

                                            rc["NAME"] = "LineString Circle";

                                            rc["FLOOR"] = floor;

                                            rc["SRCNAME"] = just_the_fileName;

                                            rc["LAYER"] = ent.Layer;

                                            rc["AREASQFT"] = circ.Area;

                                            rc["PERIMETERFT"] = circ.Circumference;

                                            rc["TXTCONTENTS"] = DBNull.Value;

                                            rc["DATESTAMP"] = System.DateTime.Now;

                                            rc["GEOMETRY"] = sdogeom_c;

                                            GEOMds.Tables[0].Rows.Add(rc);

                                        }

                                        break;

                                    default:

                                        not_selected_geom = not_selected_geom + 1;

                                        LogThis("\t\tNotUsable: " + ent.Handle.ToString() + " Layer: " + ent.Layer + " Type: " + rxc.Name);

                                        break;

                                }

                            }

                            catch (System.Exception ex)

                            {

                                mssg = string.Format("\nError on Entity Read: {0}. {1}", ent.GetType().ToString(), ex.Message);

                                ed.WriteMessage(mssg);

                                LogThis(mssg);

                            }

                        }

                        else

                        {

                            not_in_selected_layers = not_in_selected_layers + 1;

                        }

                    } // if ent

                } // foreach

                tr.Commit();

            }

            string res = string.Format("SelectedViaLayer:{0} NotSelectedViaLayer:{1} - UsableGeometry:{2} UnusableGeometry:{3}", in_selected_layers, not_in_selected_layers, selected_geom, not_selected_geom);

            LogThis("\t\t" + res);

            db.Dispose();

        }
```


# Credits #

This wiki article was kindly contributed by Dennis Jonio.