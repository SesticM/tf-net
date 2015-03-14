# Introduction #

Utilizing Oracle’s latest [ODP for .NET](http://www.oracle.com/technology/tech/windows/odpnet/index.html)
the following is one implementation of the SDO\_GEOMETRY type as a .NET class.

# Details #

A major issue in using Oracle XE for Spatial is that it does not come with the built-in JVM so consequently a great many geometry methods are not available. In particular the WKB/WKT methods and functions. In addition I utilize .Net datasets frequently and I want to be able to store the SDO\_GEOMETRY object within this architectural scheme.

With the aid of the Oracle Technology Network (OTN) forums all the required source follows. It is split into 4 different files. After installing ODP.Net just create a new project in VS2005 and build the classes.

# Implementation #

File: `OracleCustomTypeBase.cs`

```
using System;

using Oracle.DataAccess.Types;

using Oracle.DataAccess.Client;

 

namespace NetSdoGeometry

{

    public abstract class OracleCustomTypeBase<T> : INullable, IOracleCustomType, IOracleCustomTypeFactory

    where T : OracleCustomTypeBase<T>, new()

    {

        private static string errorMessageHead = "Error converting Oracle User Defined Type to .Net Type " + typeof(T).ToString() + ", oracle column is null, failed to map to . NET valuetype, column ";

 

        private OracleConnection connection;

        private IntPtr pUdt;

 

 

        private bool isNull;

 

        public virtual bool IsNull

        {

            get

            {

                return isNull;

            }

        }

 

        public static T Null

        {

            get

            {

                T t = new T();

                t.isNull = true;

                return t;

            }

        }

 

        public IOracleCustomType CreateObject()

        {

            return new T();

        }

 

        protected void SetConnectionAndPointer(OracleConnection _connection, IntPtr _pUdt)

        {

            connection = _connection;

            pUdt = _pUdt;

        }

 

        public abstract void MapFromCustomObject();

        public abstract void MapToCustomObject();

 

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)

        {

            SetConnectionAndPointer(con, pUdt);

            MapFromCustomObject();

        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)

        {

            SetConnectionAndPointer(con, pUdt);

            MapToCustomObject();

        }

 

        protected void SetValue(string oracleColumnName, object value)

        {

            if (value != null)

            {

                OracleUdt.SetValue(connection, pUdt, oracleColumnName, value);

            }

        }

        protected void SetValue(int oracleColumn_Id, object value)

        {

            if (value != null)

            {

                OracleUdt.SetValue(connection, pUdt, oracleColumn_Id, value);

            }

        }

 

        protected U GetValue<U>(string oracleColumnName)

        {

 

            if (OracleUdt.IsDBNull(connection, pUdt, oracleColumnName))

            {

                if (default(U) is ValueType)

                {

                    throw new Exception(errorMessageHead + oracleColumnName.ToString() + " of value type " + typeof(U).ToString());

                }

                else

                {

                    return default(U);

                }

            }

            else

            {

                return (U)OracleUdt.GetValue(connection, pUdt, oracleColumnName);

            }

        }

 

        protected U GetValue<U>(int oracleColumn_Id)

        {

            if (OracleUdt.IsDBNull(connection, pUdt, oracleColumn_Id))

            {

                if (default(U) is ValueType)

                {

                    throw new Exception(errorMessageHead + oracleColumn_Id.ToString() + " of value type " + typeof(U).ToString());

                }

                else

                {

                    return default(U);

                }

            }

            else

            {

                return (U)OracleUdt.GetValue(connection, pUdt, oracleColumn_Id);

            }

        }

    }

 

}
```


File: `OracleArrayTypeFactory.cs`

```
using System;

using Oracle.DataAccess.Types;

using Oracle.DataAccess.Client;

 

namespace NetSdoGeometry

{

    public abstract class OracleCustomTypeBase<T> : INullable, IOracleCustomType, IOracleCustomTypeFactory

    where T : OracleCustomTypeBase<T>, new()

    {

        private static string errorMessageHead = "Error converting Oracle User Defined Type to .Net Type " + typeof(T).ToString() + ", oracle column is null, failed to map to . NET valuetype, column ";

 

        private OracleConnection connection;

        private IntPtr pUdt;

 

 

        private bool isNull;

 

        public virtual bool IsNull

        {

            get

            {

                return isNull;

            }

        }

 

        public static T Null

        {

            get

            {

                T t = new T();

                t.isNull = true;

                return t;

            }

        }

 

        public IOracleCustomType CreateObject()

        {

            return new T();

        }

 

        protected void SetConnectionAndPointer(OracleConnection _connection, IntPtr _pUdt)

        {

            connection = _connection;

            pUdt = _pUdt;

        }

 

        public abstract void MapFromCustomObject();

        public abstract void MapToCustomObject();

 

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)

        {

            SetConnectionAndPointer(con, pUdt);

            MapFromCustomObject();

        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)

        {

            SetConnectionAndPointer(con, pUdt);

            MapToCustomObject();

        }

 

        protected void SetValue(string oracleColumnName, object value)

        {

            if (value != null)

            {

                OracleUdt.SetValue(connection, pUdt, oracleColumnName, value);

            }

        }

        protected void SetValue(int oracleColumn_Id, object value)

        {

            if (value != null)

            {

                OracleUdt.SetValue(connection, pUdt, oracleColumn_Id, value);

            }

        }

 

        protected U GetValue<U>(string oracleColumnName)

        {

 

            if (OracleUdt.IsDBNull(connection, pUdt, oracleColumnName))

            {

                if (default(U) is ValueType)

                {

                    throw new Exception(errorMessageHead + oracleColumnName.ToString() + " of value type " + typeof(U).ToString());

                }

                else

                {

                    return default(U);

                }

            }

            else

            {

                return (U)OracleUdt.GetValue(connection, pUdt, oracleColumnName);

            }

        }

 

        protected U GetValue<U>(int oracleColumn_Id)

        {

            if (OracleUdt.IsDBNull(connection, pUdt, oracleColumn_Id))

            {

                if (default(U) is ValueType)

                {

                    throw new Exception(errorMessageHead + oracleColumn_Id.ToString() + " of value type " + typeof(U).ToString());

                }

                else

                {

                    return default(U);

                }

            }

            else

            {

                return (U)OracleUdt.GetValue(connection, pUdt, oracleColumn_Id);

            }

        }

    }

 

}
```

File: `SDOPOINT.cs`

```
using System;

using Oracle.DataAccess.Types;

using Oracle.DataAccess.Client;

 

namespace NetSdoGeometry

{

    [OracleCustomTypeMappingAttribute("MDSYS.SDO_POINT_TYPE")]

    public class SDOPOINT : OracleCustomTypeBase<SDOPOINT>

    {

        private decimal? x;

        [OracleObjectMappingAttribute("X")]

        public decimal? X

        {

            get { return x; }

            set { x = value; }

        }

        public double? XD

        {

            get { return System.Convert.ToDouble(x); }

            set { x = System.Convert.ToDecimal(value); }

        }

 

 

        private decimal? y;

        [OracleObjectMappingAttribute("Y")]

        public decimal? Y

        {

            get { return y; }

            set { y = value; }

        }

        public double? YD

        {

            get { return System.Convert.ToDouble(y); }

            set { y = System.Convert.ToDecimal(value); }

        }

 

 

        private decimal? z;

        [OracleObjectMappingAttribute("Z")]

        public decimal? Z

        {

            get { return z; }

            set { z = value; }

        }

        public double? ZD

        {

            get { return System.Convert.ToDouble(z); }

            set { z = System.Convert.ToDecimal(value); }

        }

 

 

        public override void MapFromCustomObject()

        {

            SetValue("X", x);

            SetValue("Y", y);

            SetValue("Z", z);

        }

 

        public override void MapToCustomObject()

        {

            X = GetValue<decimal?>("X");

            Y = GetValue<decimal?>("Y");

            Z = GetValue<decimal?>("Z");

        }

    }

}
```

File: `sdogeometry.cs`

```
using System;

using Oracle.DataAccess.Types;

 

namespace NetSdoGeometry

{

    public static class sdogeometryTypes

    {

        //Oracle Documentation for SDO_ETYPE - SIMPLE

        //Point//Line//Polygon//exterior counterclockwise - polygon ring = 1003//interior clockwise  polygon ring = 2003

        public enum ETYPE_SIMPLE { POINT = 1, LINE = 2, POLYGON = 3, POLYGON_EXTERIOR = 1003, POLYGON_INTERIOR = 2003 }

        //Oracle Documentation for SDO_ETYPE - COMPOUND

        //1005: exterior polygon ring (must be specified in counterclockwise order)

        //2005: interior polygon ring (must be specified in clockwise order)

        public enum ETYPE_COMPOUND { FOURDIGIT = 4, POLYGON_EXTERIOR = 1005, POLYGON_INTERIOR = 2005 }

        //Oracle Documentation for SDO_GTYPE.

        //This represents the last two digits in a GTYPE, where the first item is dimension(ality) and the second is LRS

        public enum GTYPE { UNKNOWN_GEOMETRY = 00, POINT = 01, LINE = 02, CURVE = 02, POLYGON = 03, COLLECTION = 04, MULTIPOINT = 05, MULTILINE = 06, MULTICURVE = 06, MULTIPOLYGON = 07 }

        public enum DIMENSION { DIM2D = 2, DIM3D = 3, LRS_DIM3 = 3, LRS_DIM4 = 4 }

    }

    [OracleCustomTypeMappingAttribute("MDSYS.SDO_GEOMETRY")]

    public class sdogeometry : OracleCustomTypeBase<sdogeometry>

    {

        private enum OracleObjectColumns { SDO_GTYPE, SDO_SRID, SDO_POINT, SDO_ELEM_INFO, SDO_ORDINATES }

 

        private decimal? _sdo_gtype;

        [OracleObjectMappingAttribute(0)]

        public decimal? sdo_gtype

        {

            get { return _sdo_gtype; }

            set { _sdo_gtype = value;}

        }

        public int sdo_gtypeAsInt

        {

            get { return System.Convert.ToInt32(sdo_gtype); }

        }

 

        private decimal? _sdo_srid;

        [OracleObjectMappingAttribute(1)]

        public decimal? sdo_srid

        {

            get { return _sdo_srid; }

            set { _sdo_srid = value; }

        }

        public int sdo_sridAsInt

        {

            get { return System.Convert.ToInt32(sdo_srid); }

        }

 

        private SDOPOINT _sdopoint;

        [OracleObjectMappingAttribute(2)]

        public SDOPOINT sdo_point

        {

            get { return _sdopoint; }

            set { _sdopoint = value; }

        }

 

        private decimal[] elemArray;

        [OracleObjectMappingAttribute(3)]

        public decimal[] ElemArray

        {

            get { return elemArray; }

            set { elemArray = value; }

        }

 

        private decimal[] ordinatesArray;

        [OracleObjectMappingAttribute(4)]

        public decimal[] OrdinatesArray

        {

            get { return ordinatesArray; }

            set { ordinatesArray = value; }

        }

 

        [OracleCustomTypeMappingAttribute("MDSYS.SDO_ELEM_INFO_ARRAY")]

        public class ElemArrayFactory : OracleArrayTypeFactoryBase<decimal> { }

 

        [OracleCustomTypeMappingAttribute("MDSYS.SDO_ORDINATE_ARRAY")]

        public class OrdinatesArrayFactory : OracleArrayTypeFactoryBase<decimal> { }

 

        public override void MapFromCustomObject()

        {

            SetValue((int)OracleObjectColumns.SDO_GTYPE, sdo_gtype);

            SetValue((int)OracleObjectColumns.SDO_SRID, sdo_srid);

            SetValue((int)OracleObjectColumns.SDO_POINT, sdo_point);

            SetValue((int)OracleObjectColumns.SDO_ELEM_INFO, ElemArray);

            SetValue((int)OracleObjectColumns.SDO_ORDINATES, OrdinatesArray);

        }

 

        public override void MapToCustomObject()

        {

            sdo_gtype = GetValue<decimal?>((int)OracleObjectColumns.SDO_GTYPE);

            sdo_srid = GetValue<decimal?>((int)OracleObjectColumns.SDO_SRID);

            sdo_point = GetValue<SDOPOINT>((int)OracleObjectColumns.SDO_POINT);

            ElemArray = GetValue<decimal[]>((int)OracleObjectColumns.SDO_ELEM_INFO);

            OrdinatesArray = GetValue<decimal[]>((int)OracleObjectColumns.SDO_ORDINATES);

        }

 

        public int[] ElemArrayOfInts

        {

            get

            {

                int[] elems = null;

                if (this.elemArray != null)

                {

                    elems = new int[this.elemArray.Length];

                    for (int k = 0; k < this.elemArray.Length; k++)

                    {

                        elems[k] = System.Convert.ToInt32(this.elemArray[k]);

                    }

                }

                return elems;

            }

            set

            {

                if (value != null)

                {

                    int c = value.GetLength(0);

                    this.elemArray = new decimal[c];

                    for (int k = 0; k < c; k++)

                    {

                        elemArray[k] = System.Convert.ToDecimal(value[k]);

                    }

                }

                else

                {

                    this.elemArray = null;

                }

            }

        }

        public double[] OrdinatesArrayOfDoubles

        {

            get

            {

                double[] elems = null;

                if (this.OrdinatesArray != null)

                {

                    elems = new double[this.ordinatesArray.Length];

                    for (int k = 0; k < this.ordinatesArray.Length; k++)

                    {

                        elems[k] = System.Convert.ToDouble(this.ordinatesArray[k]);

                    }

                }

                return elems;

            }

            set

            {

                if (value != null)

                {

                    int c = value.GetLength(0);

                    this.ordinatesArray = new decimal[c];

                    for (int k = 0; k < c; k++)

                    {

                        ordinatesArray[k] = System.Convert.ToDecimal(value[k]);

                    }

                }

                else

                {

                    this.ordinatesArray = null;

                }

            }

        }

        private int _Dimensionality;

        public int Dimensionality

        {

            get { return _Dimensionality; }

            set { _Dimensionality = value; }

        }

        private int _LRS;

        public int LRS

        {

            get { return _LRS; }

            set { _LRS = value; }

        }

        private int _GeometryType;

        public int GeometryType

        {

            get { return _GeometryType; }

            set { _GeometryType = value; }

        }

        public int PropertiesFromGTYPE()

        {

            if ((int)this._sdo_gtype != 0)

            {

                int v = (int)this._sdo_gtype;

                int dim = v / 1000;

                Dimensionality = dim;

                v -= dim * 1000;

                int lrsDim = v / 100;

                LRS = lrsDim;

                v -= lrsDim * 100;

                GeometryType = v;

                return (Dimensionality * 1000) + (LRS * 100) + GeometryType;

            }

            else

                return 0;

        }

        public int PropertiesToGTYPE()

        {

            int v = Dimensionality * 1000;

            v = v + (LRS * 100);

            v = v + GeometryType;

            this._sdo_gtype = System.Convert.ToDecimal(v);

            return (v);

        }

 

    }

}
```

# Credits #

This wiki article was kindly contributed by Dennis Jonio.