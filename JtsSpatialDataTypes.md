The most used taxonomy to characterize the problems of spatial analysis
consider three types of data:

  * **Events or point patterns** – phenomena expressed through occurrences identified as points in space, denominated point processes. Some examples are: crime spots, disease occurrences, and the localization of vegetal species.

  * **Continuous surfaces** – estimated from a set of field samples that can be regularly or irregularly distributed. Usually, this type of data results from natural resources survey, which includes geological, topographical, ecological, phitogeographic, and pedological maps.

  * **Areas with Counts and Aggregated Rates** – means data associated to population surveys, like census and health statistics, and that are originally referred to individuals situated in specific points in space. For confidentiality reasons these data are aggregated in analysis units, usually delimited by closed polygons (census tracts, postal addressing zones, municipalities).

JTS provides the following spatial data types, according to whom TF.NET provides specialized external API readers/writers:

| **Point** | **MultiPoint** |
|:----------|:---------------|
| ![http://www.geoinova.com/examples/geometrypoint.gif](http://www.geoinova.com/examples/geometrypoint.gif) | ![http://www.geoinova.com/examples/geometrymultipoint.gif](http://www.geoinova.com/examples/geometrymultipoint.gif) |

| **LineString** | **MultiLineString** |
|:---------------|:--------------------|
| ![http://www.geoinova.com/examples/geometrylinestring.gif](http://www.geoinova.com/examples/geometrylinestring.gif) | ![http://www.geoinova.com/examples/geometrymultilinestring.gif](http://www.geoinova.com/examples/geometrymultilinestring.gif) |

| **LinearRing** |
|:---------------|
| ![http://www.geoinova.com/examples/geometrylinearring.gif](http://www.geoinova.com/examples/geometrylinearring.gif) |

| **Polygon** | **MultiPolygon** |
|:------------|:-----------------|
| ![http://www.geoinova.com/examples/geometrypolygon.gif](http://www.geoinova.com/examples/geometrypolygon.gif) | ![http://www.geoinova.com/examples/geometrymultipolygon.gif](http://www.geoinova.com/examples/geometrymultipolygon.gif) |

| **GeometryCollection** |
|:-----------------------|
| ![http://www.geoinova.com/examples/geometrycollection.gif](http://www.geoinova.com/examples/geometrycollection.gif) |

As per the OpenGIS Consortium's [Simple Features Specification for SQL](http://portal.opengeospatial.org/files/?artifact_id=829), geometries in JTS possess an `Interior`, a `Boundary`, and an `Exterior`.

Part of material provided on this page is Copyright by [The Jump Project](http://www.jump-project.org) and [Vivid Solutions, Inc](http://www.vividsolutions.com). Intoductory notes taken from "Spatial Analysis and GIS: A Primer" G. Câmara, A. M. Monteiro, S. D. Fucks, M. Carvalho et al.