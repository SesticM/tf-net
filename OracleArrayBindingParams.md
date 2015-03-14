# Introduction #

Here is how we went to averaging 400+ inserts per second using garden variety equipment and making it all easy.

This article continues to build on previous articles, especially, [Loading a .NET `DataSet` with `SDO\_GEOMETRY` from an AutoDesk DWG](http://code.google.com/p/tf-net/wiki/OracleDataSetFromDwg).

Since we have the dataset loaded with the geometry this illustrates one method of doing the table inserts. It took some desperation to come up with this and in fact I had actually coded-up doing the inserts from the dataset but I only got 8-10 inserts per seconds and with 1MM plus records to insert this would have taken days. I then coded-up SQLLoader scripts. No fun at all and SQLLoader  does not have what I would call a “smooth” interface when using `SDO_GEOMETRY`. Actually I read about “array bound inserts” on one of the Oracle OTN forums. Saved the day! ( or week if you will ).


# Details #

Please note that the only reference to actual table columns within these methods is the SQL statement itself. Not one cast!!! … Not one parameter data conversion!!!

Here is the statement that was used to build that dataset in [Loading a .NET `DataSet` with `SDO\_GEOMETRY` from an AutoDesk DWG](http://code.google.com/p/tf-net/wiki/OracleDataSetFromDwg).
```
public string sqlselect = @"select ""ID"",""ALTKEY"",""VALIDFLG"",""NAME"",""FLOOR"",""SRCNAME"",""LAYER"",""AREASQFT"", ""PERIMETERFT"",""TXTCONTENTS"",""DATESTAMP"",""GEOMETRY"" from SOMECLEVERTABLENAME";
```

I note this, not because it is unusual but because this same exact statement is all that is used as the component to build the dataset and subsequent insert structures and objects.

First the helper method to instantiate the dataset. Nothing really remarkable here; just a generalized method to emit a dataset based upon a select statement.

```
public static DataSet MaterializeDataSet(string _connectionString, string _sqlSelect, bool     _returnProviderSpecificTypes, bool _includeSchema, bool _fillTable)

        {

            DataSet ds = null;

            using (OracleConnection _oraconn = new OracleConnection(_connectionString))

            {

                try

                {

                    _oraconn.Open();

                    using (OracleCommand cmd = new OracleCommand(_sqlSelect, _oraconn))

                    {

                        cmd.CommandType = CommandType.Text;

                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))

                        {

                            da.ReturnProviderSpecificTypes = _returnProviderSpecificTypes;

                            if (_includeSchema == true)

                            {

                                ds = new DataSet("SCHEMASUPPLIED");

                                da.FillSchema(ds, SchemaType.Source);

                                if (_fillTable == true)

                                    da.Fill(ds.Tables[0]);

                            }

                            else

                            {

                                ds = new DataSet("SCHEMANOTSUPPLIED");

                                if (_fillTable == true)

                                    da.Fill(ds);

                            }

                            ds.Tables[0].TableName = "Table";

                        }//using da

                    } //using cmd

                }

                catch (OracleException _oraEx)

                {

                    throw (_oraEx); // Actually rethrow

                }

                catch (System.Exception _sysEx)

                {

                    throw (_sysEx); // Actually rethrow

                }

                finally

                {

                    if (_oraconn.State == ConnectionState.Broken || _oraconn.State == ConnectionState.Open)

                        _oraconn.Close();

                }

            }//using oraconn

            if (ds != null)

            {

                if (ds.Tables != null && ds.Tables[0] != null)

                    return ds;

                else

                    return null;

            }

            else

                return null;

        }
```

After the loading of the dataset the insert structures and objects get dynamically built based upon the same SQL statement used to instantiate the dataset. For me the magic is the `GetInsertCommand()` from `OracleCommandBuilder` and then, of course, building a 2 dimensional `OBJECT` array to hold the ‘column’ BY ‘row’ values. Not one hard-coded field name! Obviously a very generalized method.

The ‘throttle”, if you will, is the parameter `_maxarraybindcount`. This value is used to dimension the ‘rows’ index of the array. Very important to say the least and it is some fun to play with.

```
public static int BulkInsertsToTableViaDataSet(string _connectionString, string _sqlSelect,int _maxarraybindcount, ref DataSet _dataSet)

        {

            int rtn_insert_count = 0;

            int MAX_ARRAYBINDCOUNT = _maxarraybindcount;

            OracleConnection oc = new OracleConnection(_connectionString);

            OracleCommand cm = new OracleCommand(_sqlSelect, oc);

            OracleDataAdapter da = new OracleDataAdapter(cm);

            OracleCommandBuilder cb = new OracleCommandBuilder(da);

            OracleCommand theInsertCommand = cb.GetInsertCommand();

            //

            // build the generic storage for the field values based upon the insert command

            //                     COLUMNS x ROWS

            object[][] holder = new object[theInsertCommand.Parameters.Count][];

            for (int x = 0; x < theInsertCommand.Parameters.Count; x++)

            {

                holder[x] = new object[MAX_ARRAYBINDCOUNT];

            }

            int total_reads = 0;

            int current_reads = 0; // the row counter

            foreach (DataRow d in _dataSet.Tables[0].Rows)

            {

                total_reads = total_reads + 1;

                // populate the column array for this row

                for (int c = 0; c < theInsertCommand.Parameters.Count; c++)

                {

                    holder[c][current_reads] = d[c];

                }

                current_reads = current_reads + 1;

                if (current_reads == MAX_ARRAYBINDCOUNT)

                {

                    int num_inserted = ArrayBoundInsertHelper(ref theInsertCommand, ref holder, current_reads);

                    rtn_insert_count = rtn_insert_count + num_inserted;

                    current_reads = 0;

                }

            }

            //

            // if anything left we insert them now

            //

            if (current_reads > 0)

            {

                int num_inserted = ArrayBoundInsertHelper(ref theInsertCommand, ref holder, current_reads);

                rtn_insert_count = rtn_insert_count + num_inserted;

            }

            theInsertCommand.Dispose();

            cb.Dispose();

            da.Dispose();

            cm.Dispose();

            oc.Dispose();

            return rtn_insert_count;

        }
```

Finally, this is the guy that does the real work. Very short and very straightforward, so there is not much to say. Oracle did all the work.

```
private static int ArrayBoundInsertHelper(ref OracleCommand _theInsertCommand, ref object[][] _holder, int _boundCount)

        {

            int rtn_val = 0;

            try

            {

                _theInsertCommand.ArrayBindCount = _boundCount;

                for (int i = 0; i < _theInsertCommand.Parameters.Count; i++)

                {

                    _theInsertCommand.Parameters[i].Value = _holder[i];

                    _theInsertCommand.Parameters[i].Direction = ParameterDirection.Input;

                }

                _theInsertCommand.Connection.Open();

                rtn_val = _theInsertCommand.ExecuteNonQuery();

            }

            catch (OracleException _oraEx)

            {

                throw (_oraEx); // Actually rethrow

            }

            catch (System.Exception _sysEx)

            {

                throw (_sysEx); // Actually rethrow

            }

            finally

            {

                if (_theInsertCommand.Connection != null && _theInsertCommand.Connection.State == ConnectionState.Open)

                    _theInsertCommand.Connection.Close();

            }

            return rtn_val;

        }
```


# Credits #

This wiki article was kindly contributed by Dennis Jonio.