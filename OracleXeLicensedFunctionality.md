# Introduction #
In my reading of the _Guide and Reference_ I have listed, for the most part, what is licensed and available using Oracle Locator.

# Details #

Taken from:
Oracle® Spatial User’s Guide and Reference - 10g Release 2 (10.2)

In general, Locator includes the data types, operators, and indexing capabilities of Oracle Spatial, along with a limited set of the functions and procedures of Spatial. The Locator features include the following:

  * An object type (`SDO_GEOMETRY`) that describes and supports any type of geometry
  * A spatial indexing capability that lets you create spatial indexes on geometry data
  * Spatial operators (described in Chapter 11) that use the spatial index for performing spatial queries
  * Some geometry functions and the `SDO_AGGR_MBR` spatial aggregate function
  * Coordinate system support for explicit geometry and layer transformations (`SDO_CS.TRANSFORM` function and `SDO_CS.TRANSFORM_LAYER` procedure, described in Chapter 13)
  * Tuning functions and procedures (`SDO_TUNE` package, described in Chapter 19)
  * Spatial utility functions (`SDO_UTIL` package, described in Chapter 20)
  * Integration with Oracle Application Server 10g
  * Function-based spatial indexing Section 9.2 Table partitioning support for spatial indexes (including splitting, merging, and exchanging partitions and their indexes)Section 4.1.4 and Section 4.1.5
  * Geodetic data support Section 6.2 and Section 6.7
  * SQL statements for creating, altering, and deleting indexes Chapter 10 Parallel spatial index builds (`PARALLEL` keyword with `ALTER INDEX REBUILD` and `CREATE INDEX` statements) (new with release 9.2) Chapter 10
  * `SDO_GEOMETRY` object type methods Section 2.3 Spatial operators (including `SDO_JOIN`, which is technically a table function but is documented with the operators) Chapter 11
  * Implicit coordinate system transformations for operator calls where a window needs to be converted to the coordinate system of the queried layer.
  * `SDO_AGGR_MBR` spatial aggregate function Chapter 12 Coordinate system support for explicit geometry and layer transformations (`SDO_CS.TRANSFORM` function and `SDO_CS.TRANSFORM_LAYER` procedure) Chapter 13
  * The following `SDO_GEOM` package functions and procedures: `SDO_GEOM.SDO_DISTANCE`, `SDO_GEOM.VALIDATE_GEOMETRY_WITH_CONTEXT`, `SDO_GEOM.VALIDATE_LAYER_WITH_CONTEXT` Chapter 15
  * Package (`SDO_MIGRATE`) to upgrade data from previous Spatial releases to the current release Chapter 17
  * Tuning functions and procedures (`SDO_TUNE` package) Chapter 19 Spatial utility functions (`SDO_UTIL` package) Chapter 20
  * Object replication Oracle Database Advanced Replication
  * Graphical tool for tuning spatial quadtree indexes (Spatial Index Advisor integrated application in Oracle Enterprise Manager) Online help for Oracle


## Operator Description ##

### Spatial Relations ###

| `SDO_FILTER` | Specifies which geometries may interact with a given geometry. |
|:-------------|:---------------------------------------------------------------|
| `SDO_JOIN` | Performs a spatial join based on one or more topological relationships. |
| `SDO_NN` | Determines the nearest neighbor geometries to a geometry. |
| `SDO_NN_DISTANCE` | Returns the distance of an object returned by the `SDO_NN` operator. |
| `SDO_RELATE` | Determines whether or not two geometries interact in a specified way. (See also Table 11–2 for convenient alternative operators for performing specific mask value operations.) |
| `SDO_WITHIN_DISTANCE` | Determines if two geometries are within a specified distance from one another. |
| `SDO_ANYINTERACT` | Checks if any geometries in a table have the `ANYINTERACT` topological relationship with a specified geometry. |
| `SDO_CONTAINS` | Checks if any geometries in a table have the `CONTAINS` topological relationship with a specified geometry. |
| `SDO_COVEREDBY` | Checks if any geometries in a table have the `COVEREDBY` topological relationship with a specified geometry. |
| `SDO_COVERS` | Checks if any geometries in a table have the `COVERS` topological relationship with a specified geometry. |
| `SDO_EQUAL` | Checks if any geometries in a table have the `EQUAL` topological relationship with a specified geometry. |
| `SDO_INSIDE` | Checks if any geometries in a table have the `INSIDE` topological relationship with a specified geometry. |
| `SDO_ON` | Checks if any geometries in a table have the `ON` topological relationship with a specified geometry. |
| `SDO_OVERLAPBDYDISJOINT` | Checks if any geometries in a table have the `OVERLAPBDYDISJOINT` topological relationship with a specified geometry. |
| `SDO_OVERLAPBDYINTERSECT` | Checks if any geometries in a table have the `OVERLAPBDYINTERSECT` topological relationship with a specified geometry. |
| `SDO_OVERLAPS` | Checks if any geometries in a table overlap (that is, have the `OVERLAPBDYDISJOINT` or `OVERLAPBDYINTERSECT` topological relationship with) a specified geometry. |
| `SDO_TOUCH` | Checks if any geometries in a table have the `TOUCH` topological relationship with a specified geometry. |

### Spatial Relations - Aggregation ###

| `SDO_AGGR_CENTROID` | Returns a geometry object that is the centroid ("center of gravity") of the specified geometry objects. |
|:--------------------|:--------------------------------------------------------------------------------------------------------|
| `SDO_AGGR_CONCAT_LINES` | Returns a geometry that concatenates the specified line or multiline geometries. |
| `SDO_AGGR_CONVEXHULL` | Returns a geometry object that is the convex hull of the specified geometry objects. |
| `SDO_AGGR_LRS_CONCAT` | Returns an LRS geometry object that concatenates specified LRS geometry objects. |
| `SDO_AGGR_MBR` | Returns the minimum bounding rectangle of the specified geometries. |
| `SDO_AGGR_UNION` | Returns a geometry object that is the topological union (OR operation) of the specified geometry objects. |

### Coordinate Reference Systems ###

| `SDO_CS.ADD_PREFERENCE_FOR_OP` | Adds a preference for an operation between a source coordinate system and a target coordinate system. |
|:-------------------------------|:------------------------------------------------------------------------------------------------------|
| `SDO_CS.CONVERT_NADCON_TO_XML` | Converts a NADCON (North American Datum Conversion) grid in ASCII format to an Oracle Spatial XML representation. |
| `SDO_CS.CONVERT_NTV2_TO_XML` | Converts an NTv2 (National Transformation Version 2) grid in ASCII format to an Oracle Spatial XML representation. |
| `SDO_CS.CONVERT_XML_TO_NADCON` | Converts an Oracle Spatial XML representation of a NADCON (North American Datum Conversion) grid to NADCON ASCII format. |
| `SDO_CS.CONVERT_XML_TO_NTV2` | Converts an Oracle Spatial XML representation of an NTv2 (National Transformation Version 2) grid to NTv2 ASCII format. |
| `SDO_CS.CREATE_CONCATENATED_OP` | Creates a concatenated operation. |
| `SDO_CS.CREATE_OBVIOUS_EPSG_RULES` | Creates a basic set of EPSG rules to be applied in certain transformations. |
| `SDO_CS.CREATE_PREF_CONCATENATED_OP` | Creates a concatenated operation, associating it with a transformation plan and making it preferred either systemwide or for a specified use case. |
| `SDO_CS.DELETE_ALL_EPSG_RULES` | Deletes the basic set of EPSG rules to be applied in certain transformations. |
| `SDO_CS.DELETE_OP` | Deletes a concatenated operation. |
| `SDO_CS.DETERMINE_CHAIN` | Returns the query chain, based on the system rule set, to be used in transformations from one coordinate reference system to another coordinate reference system. |
| `SDO_CS.DETERMINE_DEFAULT_CHAIN` | Returns the default chain of SRID values in  transformations from one coordinate reference system to another coordinate reference system. |
| `SDO_CS.FIND_GEOG_CRS` | Returns the SRID values of geodetic (geographic) coordinate reference systems that have the same well-known text (WKT) numeric values as the coordinate reference system with the specified reference SRID value. |
| `SDO_CS.FIND_PROJ_CRS` | Returns the SRID values of projected coordinate reference systems that have the same well-known text (WKT) numeric values as the coordinate reference system with the specified reference SRID value. |
| `SDO_CS.FROM_OGC_SIMPLEFEATURE_SRS` | Converts a well-known text string from the Open Geospatial Consortium simple feature format without the TOWGS84 keyword to the format that includes the TOWGS84 keyword. |
| `SDO_CS.MAP_EPSG_SRID_TO_ORACLE` | Returns the Oracle Spatial SRID values corresponding to the specified EPSG SRID value. |
| `SDO_CS.MAP_ORACLE_SRID_TO_EPSG` | Returns the EPSG SRID value corresponding to the specified Oracle Spatial SRID value. |
| `SDO_CS.REVOKE_PREFERENCE_FOR_OP` | Revokes a preference for an operation between a source coordinate system and a target coordinate system. |
| `SDO_CS.TO_OGC_SIMPLEFEATURE_SRS` | Converts a well-known text string from the Open Geospatial Consortium simple feature format that includes the TOWGS84 keyword to the format without the TOWGS84 keyword. |
| `SDO_CS.TRANSFORM` | Transforms a geometry representation using a coordinate system (specified by SRID or name). |
| `SDO_CS.TRANSFORM_LAYER` | Transforms an entire layer of geometries (that is, all geometries in a specified column in a table). |
| `SDO_CS.UPDATE_WKTS_FOR_ALL_EPSG_CRS` | Updates the well-known text (WKT) description for all EPSG coordinate reference systems. |
| `SDO_CS.UPDATE_WKTS_FOR_EPSG_CRS` | Updates the well-known text (WKT) description for the EPSG coordinate reference system associated with a specified SRID. |
| `SDO_CS.UPDATE_WKTS_FOR_EPSG_DATUM` | Updates the well-known text (WKT) description for all EPSG coordinate reference systems associated with a specified datum. |
| `SDO_CS.UPDATE_WKTS_FOR_EPSG_ELLIPS` | Updates the well-known text (WKT) description for all EPSG coordinate reference systems associated with a specified ellipsoid. |
| `SDO_CS.UPDATE_WKTS_FOR_EPSG_OP` | Updates the well-known text (WKT) description for all EPSG coordinate reference systems associated with a specified coordinate transformation operation. |
| `SDO_CS.UPDATE_WKTS_FOR_EPSG_PARAM` | Updates the well-known text (WKT) description for all EPSG coordinate reference systems associated with a specified coordinate transformation operation and parameter for transformation operations. |
| `SDO_CS.UPDATE_WKTS_FOR_EPSG_PM` | Updates the well-known text (WKT) description for all EPSG coordinate reference systems associated with a specified prime meridian. |
| `SDO_CS.VALIDATE_WKT` | Validates the well-known text (WKT) description associated with a specified SRID. |
| `SDO_CS.VIEWPORT_TRANSFORM` | (deprecated) Transforms an optimized rectangle into a valid polygon for use with Spatial operators and functions. |

### Tuning And Indexing ###

| `SDO_TUNE.AVERAGE_MBR` | Calculates the average minimum bounding rectangle for geometries in a layer. |
|:-----------------------|:-----------------------------------------------------------------------------|
| `SDO_TUNE.ESTIMATE_RTREE_INDEX_SIZE` | Estimates the maximum number of megabytes needed for an R-tree spatial index table. |
| `SDO_TUNE.EXTENT_OF` | Returns the minimum bounding rectangle of the data in a layer. |
| `SDO_TUNE.MIX_INFO` | Calculates geometry type information for a spatial layer, such as the percentage of each geometry type. |
| `SDO_TUNE.QUALITY_DEGRADATION` | Returns the quality degradation for an index or the average quality degradation for all index tables for an index, or returns the quality degradation for a specified index table. |

### Utility Operators ###

| `SDO_UTIL.APPEND` | Appends one geometry to another geometry to create a new geometry. |
|:------------------|:-------------------------------------------------------------------|
| `SDO_UTIL.CIRCLE_POLYGON` | Returns the polygon geometry that approximates and is covered by a specified circle. |
| `SDO_UTIL.CONCAT_LINES` | Concatenates two line or multiline two-dimensional geometries to create a new geometry. |
| `SDO_UTIL.CONVERT_UNIT` | Converts values from one angle, area, or distance unit of measure to another. |
| `SDO_UTIL.ELLIPSE_POLYGON` | Returns the polygon geometry that approximates and is covered by a specified ellipse. |
| `SDO_UTIL.EXTRACT` | Returns the geometry that represents a specified element (and optionally a ring) of the input geometry. |
| `SDO_UTIL.FROM_WKBGEOMETRY` | Converts a geometry in the well-known binary (WKB) format to a Spatial geometry object. |
| `SDO_UTIL.FROM_WKTGEOMETRY` | Converts a geometry in the well-known text (WKT) format to a Spatial geometry object. |
| `SDO_UTIL.GETNUMELEM` | Returns the number of elements in the input geometry. |
| `SDO_UTIL.GETNUMVERTICES` | Returns the number of vertices in the input geometry. |
| `SDO_UTIL.GETVERTICES` | Returns the coordinates of the vertices of the input geometry. |
| `SDO_UTIL.INITIALIZE_INDEXES_FOR_TTS` | Initializes all spatial indexes in a tablespace that was transported to another database. |
| `SDO_UTIL.POINT_AT_BEARING` | Returns a point geometry that is at the specified distance and bearing from the start point. |
| `SDO_UTIL.POLYGONTOLINE` | Converts all polygon-type elements in a geometry to line-type elements, and sets the SDO\_GTYPE value accordingly. |
| `SDO_UTIL.PREPARE_FOR_TTS` | Prepares a tablespace to be transported to another database, so that spatial indexes will be preserved during the transport operation. |
| `SDO_UTIL.RECTIFY_GEOMETRY` | Fixes certain problems with the input geometry, and returns a valid geometry. |
| `SDO_UTIL.REMOVE_DUPLICATE_VERTICES` | Removes duplicate (redundant) vertices from a geometry. |
| `SDO_UTIL.REVERSE_LINESTRING` | Returns a line string geometry with the vertices of the input geometry in reverse order. |
| `SDO_UTIL.SIMPLIFY` | Simplifies the input geometry, based on a threshold value, using the Douglas-Peucker algorithm. |
| `SDO_UTIL.TO_GMLGEOMETRY` | Converts a Spatial geometry object to a geography markup language (GML 2.0) fragment based on the geometry types defined in the Open GIS geometry.xsd schema document. |
| `SDO_UTIL.TO_WKBGEOMETRY` | Converts a Spatial geometry object to the well-known binary (WKB) format. |
| `SDO_UTIL.TO_WKTGEOMETRY` | Converts a Spatial geometry object to the well-known text (WKT) format. |
| `SDO_UTIL.VALIDATE_WKBGEOMETRY` | Validates the input geometry, which is in the standard well-known binary (WKB) format; returns the string TRUE if the geometry is valid or FALSE if the geometry is not valid. |
| `SDO_UTIL.VALIDATE_WKTGEOMETRY` | Validates the input geometry, which is of type CLOB or VARCHAR2 and in the standard well-known text (WKT) format; returns the string TRUE if the geometry is valid or FALSE if the geometry is not valid. |
| `SDO_GEOM.SDO_DISTANCE` | TBD |
| `SDO_GEOM.VALIDATE_GEOMETRY_WITH_CONTEXT` | TBD |
| `SDO_GEOM.VALIDATE_LAYER_WITH_CONTEXT` | TBD |
| `SDO_MIGRATE.TO_CURRENT` | TBD |


# Credits #

This wiki article was kindly contributed by Dennis Jonio.