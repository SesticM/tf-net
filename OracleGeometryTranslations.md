# Introduction #

An example implementation of all these varies geometry translations.


# Details #

Since I am challenged geometrically I wanted to see how they all behaved.


# Implementation #

```
using System;

using System.Text;

using oracle.spatial.geometry;

using oracle.spatial.util;

using TG = Topology.Geometries;

using TIO = Topology.IO;

using OG = OSGeo.FDO.Geometry;

using NetSdoGeometry;

 

namespace OraFDOBase

{

    public class GeometryXlate

    {

        public static TG.IGeometry fromSDOToJTS(sdogeometry sdogeom)

        {

            TG.IGeometry jts = null;

            try

            {

                JGeometry jgeo = fromSDOToJGeometry(sdogeom);

                jts = fromJGeometryToJTS(jgeo);

            }

            catch (System.Exception)

            {

                jts = null;

            }

            return jts;

        }

        public static OG.IGeometry fromSDOToFDO(sdogeometry sdogeom)

        {

            OG.IGeometry fdo = null;

            try

            {

                JGeometry jgeo = fromSDOToJGeometry(sdogeom);

                fdo = fromJGeometryToFDO(jgeo);

            }

            catch (System.Exception)

            {

                fdo = null;

            }

            return fdo;

        }

        public static sdogeometry fromJTSToSDO(TG.IGeometry jts_geometry)

        {

            sdogeometry rtn_val = null;

            try

            {

                JGeometry j_geometry = fromJTSToJGeometry(jts_geometry);

                rtn_val = fromJGeometryToSDO(j_geometry);

            }

            catch (System.Exception)

            {

                rtn_val = null;

            }

            return rtn_val;

        }

        public static sdogeometry fromFDOToSDO(OG.IGeometry fdo_igeometry, int srid)

        {

            sdogeometry rtn_val = null;

            try

            {

                JGeometry j_geometry = fromFDOToJGeometry(fdo_igeometry, srid);

                rtn_val = fromJGeometryToSDO(j_geometry);

            }

            catch (System.Exception)

            {

                rtn_val = null;

            }

            return rtn_val;

        }

        public static sdogeometry fromJGeometryToSDO(JGeometry j_geometry)

        {

            sdogeometry Sgeo = new sdogeometry();

            if (j_geometry.isPoint())

            {

                Sgeo.Dimensionality = j_geometry.getDimensions();

                Sgeo.GeometryType = j_geometry.getType();

                Sgeo.sdo_sridAsInt = j_geometry.getSRID();

                Sgeo.LRS = j_geometry.getLRMDimension();

                SDOPOINT p = new SDOPOINT();

                double[] pts = j_geometry.getPoint();

                p.XD = pts[0];

                p.YD = pts[1];

                Sgeo.sdo_point = p;

                Sgeo.PropertiesToGTYPE();

            }

            else

            {

                Sgeo.Dimensionality = j_geometry.getDimensions();

                Sgeo.GeometryType = j_geometry.getType();

                Sgeo.sdo_sridAsInt = j_geometry.getSRID();

                Sgeo.LRS = j_geometry.getLRMDimension();

                Sgeo.ElemArrayOfInts = j_geometry.getElemInfo();

                Sgeo.OrdinatesArrayOfDoubles = j_geometry.getOrdinatesArray();

                Sgeo.PropertiesToGTYPE();

            }

            return Sgeo;

        }

        public static TG.IGeometry fromJGeometryToJTS(JGeometry j_geometry)

        {

            TG.IGeometry rtn_val = null;

            TG.PrecisionModel pm = new TG.PrecisionModel(TG.PrecisionModels.Floating);

            TG.GeometryFactory gf = new TG.GeometryFactory(pm, j_geometry.getSRID());

            TIO.WKTReader wktreader = new TIO.WKTReader(gf);

            oracle.spatial.util.WKT OracleJGeometry_WKT = new WKT();

            try

            {

                byte[] ba = OracleJGeometry_WKT.fromJGeometry(j_geometry);

                rtn_val = wktreader.Read(Encoding.UTF8.GetString(ba));

            }

            catch (System.Exception)

            {

                rtn_val = null;

            }

            return rtn_val;

        }

        public static OG.IGeometry fromJGeometryToFDO(JGeometry j_geometry)

        {

            OG.IGeometry rtn_val = null;

            OG.FgfGeometryFactory fgf = new OSGeo.FDO.Geometry.FgfGeometryFactory();

            oracle.spatial.util.WKB OracleJGeometry_WKB = new WKB(ByteOrder.LITTLE_ENDIAN);

            try

            {

                byte[] ba = OracleJGeometry_WKB.fromJGeometry(j_geometry);

                rtn_val = fgf.CreateGeometryFromWkb(ba);

            }

            catch (System.Exception)

            {

                rtn_val = null;

            }

            return rtn_val;

        }

        public static JGeometry fromFDOToJGeometry(OG.IGeometry fdo_igeometry, int srid)

        {

            JGeometry rtn_val = null;

            OG.FgfGeometryFactory fgf = new OSGeo.FDO.Geometry.FgfGeometryFactory();

            byte[] ba = fgf.GetWkb(fdo_igeometry);

            oracle.spatial.util.WKB OracleJGeometry_WKB = new WKB(ByteOrder.LITTLE_ENDIAN);

            try

            {

                rtn_val = OracleJGeometry_WKB.toJGeometry(ba);

                rtn_val.setSRID(srid);

            }

            catch (System.Exception)

            {

                rtn_val = null;

            }

            return rtn_val;

        }

        public static JGeometry fromJTSToJGeometry(TG.IGeometry jts_geometry)

        {

            JGeometry rtn_val = null;

            byte[] ba = jts_geometry.AsBinary();

            int srid = jts_geometry.SRID;

            oracle.spatial.util.WKB OracleJGeometry_WKB = new WKB(ByteOrder.LITTLE_ENDIAN);

            try

            {

                rtn_val = OracleJGeometry_WKB.toJGeometry(ba);

                rtn_val.setSRID(srid);

            }

            catch (System.Exception)

            {

                rtn_val = null;

            }

            return rtn_val;

        }

        public static JGeometry fromSDOToJGeometry(sdogeometry sdogeom)

        {

            JGeometry j_geometry = null;

            try

            {

                if (sdogeom.sdo_point == null)

                {

                    j_geometry = new JGeometry(sdogeom.sdo_gtypeAsInt, sdogeom.sdo_sridAsInt, sdogeom.ElemArrayOfInts, sdogeom.OrdinatesArrayOfDoubles);

                }

                else

                {

                    j_geometry = new JGeometry((double)sdogeom.sdo_point.XD, (double)sdogeom.sdo_point.YD, sdogeom.sdo_sridAsInt);

                }

            }

            catch (System.Exception)

            {

                j_geometry = null;

            }

            return j_geometry;

        }

 

    }//eoc

}//eons
```


# Credits #

This wiki article was kindly contributed by Dennis Jonio.