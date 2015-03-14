# Introduction #

Utilizing Oracle’s latest ODP for .NET the following is one implementation of the `SDO_DIM_ARRAY` and `SDO_DIM_ELEMENT` type(s) as a .NET class.

# Details #

This one is easy to explain. I needed to use `SDO_MIGRATE.TO_CURRENT` to fix some ORA-13367: wrong orientation for interior/exterior rings errors. So I needed `SDO_DIM_ARRAY` and thusly `SDO_DIM_ELEMENT` because `SDO_MIGRATE.TO_CURRENT` requires a `SDO_DIM_ARRAY` as a parameter.

I have included my method of utilizing `SDO_MIGRATE.TO_CURRENT` after the class definitions.

# Implementation #

```
using System;

using System.Collections;

using System.Text;

 

using Oracle.DataAccess.Client;

using Oracle.DataAccess.Types;

 

namespace NetSdoDimArray

{

    /// <summary>

    /// ////////////////////////////////////

    /// sdodimarray

    /// ////////////////////////////////////

    /// </summary>

    public class sdodimarray : IOracleCustomType, INullable

    {

        [OracleObjectMapping(0)]

        public sdodimelement[] _dimelements;

        private bool m_bIsNull;

        public bool IsNull

        {

            get

            {

                return m_bIsNull;

            }

        }

        public sdodimelement[] DimElements

        {

            get

            {

                return _dimelements;

            }

            set

            {

                _dimelements = value;

            }

        }

        public static sdodimarray Null

        {

            get

            {

                sdodimarray obj = new sdodimarray();

                obj.m_bIsNull = true;

                return obj;

            }

        }

        public override string ToString()

        {

            if (m_bIsNull)

                return "sdodimarray.Null";

            else

            {

                StringBuilder sb = new StringBuilder();

                foreach (sdodimelement i in _dimelements)

                {

                    sb.Append("sdodimelement(");

                    sb.Append(i._dimname + "=");

                    sb.Append(string.Format("{0:0.#####}", i._lb));

                    sb.Append(",");

                    sb.Append(string.Format("{0:0.#####}", i._ub));

                    sb.Append(",Tol=");

                    sb.Append(i._tolerance.ToString());

                    sb.Append(")");

                }

                return sb.ToString();

            }

        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)

        {

            _dimelements = (sdodimelement[])OracleUdt.GetValue(con, pUdt, 0);

        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)

        {

            OracleUdt.SetValue(con, pUdt, 0, _dimelements);

        }

    }

    [OracleCustomTypeMapping("MDSYS.SDO_DIM_ARRAY")]

    public class sdodimarrayFactory : IOracleArrayTypeFactory, IOracleCustomTypeFactory

    {

        // IOracleCustomTypeFactory Inteface

        public IOracleCustomType CreateObject()

        {

            return new sdodimarray();

        }

 

        // IOracleArrayTypeFactory.CreateArray Inteface

        public Array CreateArray(int numElems)

        {

            return new sdodimelement[numElems];

        }

 

        // IOracleArrayTypeFactory.CreateStatusArray

        public Array CreateStatusArray(int numElems)

        {

            // An OracleUdtStatus[] is not required to store null status information

            // if there is no NULL attribute data in the element array

            return null;

 

        }

    }

    /// <summary>

    /// ////////////////////////////////////

    /// sdodimelement

    /// ////////////////////////////////////

    /// </summary>

    public class sdodimelement : IOracleCustomType, INullable

    {

        [OracleObjectMapping(0)]

        public string _dimname;

        [OracleObjectMapping(1)]

        public double _lb;

        [OracleObjectMapping(2)]

        public double _ub;

        [OracleObjectMapping(3)]

        public double _tolerance;

        private bool m_bIsNull;

        public bool IsNull

        {

            get

            {

                return m_bIsNull;

            }

        }

        public static sdodimelement Null

        {

            get

            {

                sdodimelement obj = new sdodimelement();

                obj.m_bIsNull = true;

                return obj;

            }

        }

        public override string ToString()

        {

            if (m_bIsNull)

                return "sdodimelement.Null";

            else

            {

                StringBuilder sb = new StringBuilder();

                sb.Append("sdodimelement(");

                sb.Append(_dimname + "=");

                sb.Append(string.Format("{0:0.#####}", _lb));

                sb.Append(",");

                sb.Append(string.Format("{0:0.#####}", _ub));

                sb.Append(",Tol=");

                sb.Append(_tolerance.ToString());

                sb.Append(")");

                return sb.ToString();

            }

        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)

        {

            _dimname = (string)OracleUdt.GetValue(con, pUdt, 0);

            _lb = (double)OracleUdt.GetValue(con, pUdt, 1);

            _ub = (double)OracleUdt.GetValue(con, pUdt, 2);

            _tolerance = (double)OracleUdt.GetValue(con, pUdt, 3);

        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)

        {

            OracleUdt.SetValue(con, pUdt, 0, _dimname);

            OracleUdt.SetValue(con, pUdt, 1, _lb);

            OracleUdt.SetValue(con, pUdt, 2, _ub);

            OracleUdt.SetValue(con, pUdt, 3, _tolerance);

        }

    }

    [OracleCustomTypeMapping("MDSYS.SDO_DIM_ELEMENT")]

    public class sdodimelementFactory : IOracleCustomTypeFactory

    {

        // IOracleCustomTypeFactory Inteface

        public IOracleCustomType CreateObject()

        {

            sdodimelement sdodimelement = new sdodimelement();

            return sdodimelement;

        }

    }

 

}
```

As you see, it utilizes both `sdogeometry` and `sdodimarray`...

```
public static sdogeometry MigrateToCurrentSP(string connectstring, sdogeometry _geoIn, sdodimarray _toleranceIn)

        {

            //ORA-13367:      wrong orientation for interior/exterior rings

            sdogeometry rtnval = null;

            using (OracleConnection conn = new OracleConnection(connectstring))

            {

                using (OracleCommand dbcmd = conn.CreateCommand())

                {

                    dbcmd.CommandType = CommandType.StoredProcedure;

                    dbcmd.CommandText = "SDO_MIGRATE.TO_CURRENT";

 

                    OracleParameter cParm1 = new OracleParameter();

                    cParm1.ParameterName = "geometry";

                    cParm1.OracleDbType = OracleDbType.Object;

                    cParm1.Value = _geoIn;

                    cParm1.UdtTypeName = "MDSYS.SDO_GEOMETRY";

                    cParm1.Direction = ParameterDirection.Input;

 

                    OracleParameter cParm2 = new OracleParameter();

                    cParm2.ParameterName = "tolerance";

                    cParm2.OracleDbType = OracleDbType.Object;

                    cParm2.Value = _toleranceIn;

                    cParm2.UdtTypeName = "MDSYS.SDO_DIM_ARRAY";

                    cParm2.Direction = ParameterDirection.Input;

 

                    OracleParameter cParm3 = new OracleParameter();

                    cParm3.ParameterName = "RETURNVALUE";

                    cParm3.OracleDbType = OracleDbType.Object;

                    cParm3.UdtTypeName = "MDSYS.SDO_GEOMETRY";

                    cParm3.Direction = ParameterDirection.ReturnValue;

                    // Note the order

                    dbcmd.Parameters.Add(cParm3);

                    dbcmd.Parameters.Add(cParm1);

                    dbcmd.Parameters.Add(cParm2);

                    try

                    {

                        conn.Open();

                        dbcmd.ExecuteNonQuery();

                        rtnval = (sdogeometry)dbcmd.Parameters["RETURNVALUE"].Value;

                    }

                    catch (Exception _Ex)

                    {

                        throw (_Ex); // Actually rethrow

                    }

                    finally

                    {

                        if (conn != null && conn.State == ConnectionState.Open)

                        {

                            conn.Close();

                        }

                    }

                }//using cmd

            }//using conn

            return rtnval;

        }
```

# Credits #

This wiki article was kindly contributed by Dennis Jonio.