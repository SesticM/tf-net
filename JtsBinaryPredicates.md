Although OSGeo [Feature Data Objects](http://fdo.osgeo.org/) (FDO) concept exposes majority of binary predicates via it's geometric data types, their implementation still depends on specialized data provider and underlying technology. FDO relies on it's datastore ability to cope with spatial relations, and provider's designer to tackle with them. Actually, quite few feature datastores are spatially enabled by default. If FDO provider doesn't provide certain binary predicate by design, it's reasonable to turn to specialized TF.NET reader/writer and binary predicates exposed by JTS core.

JTS supports a complete set of binary predicates. Binary predicate methods take two Geometries as arguments and return a boolean indicating whether the Geometries have the named spatial relationship. The relationships supported are:
  * **`Equals`** - geometries are topologically equal
  * **`Disjoint`** - geometries have no point in common
  * **`Intersects`** - geometries have at least one point in common (the inverse of **`Disjoint`**)
  * **`Touches`** - geometries have at least one boundary point in common, but no interior points
  * **`Crosses`** - geometries share some but not all interior points, and the dimension of the intersection is less than that of at least one of the geometries
  * **`Within`** - geometry A lies in the interior of geometry B
  * **`Contains`** - geometry B lies in the interior of geometry A (the inverse of **`Within`**)
  * **`Overlaps`** - geometries share some but not all points in common, and the intersection has the same dimension as the geometries themselves
  * Also, the general **`Relate`** operator is supported. Relate can be used to determine the Dimensionally Extended 9 Intersection Matrix (DE-9IM) which completely describes the relationship of two Geometries.

The algorithm used for computing Binary Predicates in JTS is robust, and is not subject to dimensional collapse problems.

For Example:

| Two Geometries to compare | The Binary Predicates | The DE-9IM returned by relate |
|:--------------------------|:----------------------|:------------------------------|
| ![http://www.geoinova.com/examples/linepolyrelate.gif](http://www.geoinova.com/examples/linepolyrelate.gif) | ![http://www.geoinova.com/examples/binarypredicates.gif](http://www.geoinova.com/examples/binarypredicates.gif) | ![http://www.geoinova.com/examples/relateresults.gif](http://www.geoinova.com/examples/relateresults.gif) |

You may try interactive testing of binary predicates using [JTS Validation Suite](http://www.vividsolutions.com/jts/jtshome.htm).


### Constructed Points and Dimensional Collapse ###

Geometries computed by spatial analysis methods may contain constructed points which are not present in the input Geometries. These new points arise from intersections between line segments in the edges of the input Geometries. In the general case it is not possible to represent constructed points exactly. This is due to the fact that the coordinates of an intersection point may contain twice as many bits of precision as the coordinates of the input line segments. In order to represent these constructed points explicitly, JTS must truncate them to fit the Precision Model.

Unfortunately, truncating coordinates moves them slightly. Line segments which would not be coincident in the exact result may become coincident in the truncated representation. For Line-Area combinations, this can lead to dimensional collapses , which are situations where a computed element has a lower dimension than it would in the exact result.

JTS handles dimensional collapses as gracefully as possible, by forming the lower-dimension Geometry resulting from the collapse.


Part of material provided on this page is Copyright by [The Jump Project](http://www.jump-project.org) and [Vivid Solutions, Inc](http://www.vividsolutions.com).