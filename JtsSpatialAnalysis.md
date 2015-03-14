The emphasis of spatial analysis is to measure properties and relationships,
taking into account the spatial localization of the phenomenon under study in a
direct way. That is, the central idea is to incorporate space into the analysis to be
made. JTS supports the fundamental spatial analysis methods that try to address these issues. It is intended to help those interested to study, explore and model processes that
express themselves through a distribution in space, here called geographic phenomena. Spatial analysis methods take one or two Geometries as arguments and return a new constructed Geometry.

The spatial analysis methods in the JTS are:

| **Intersection** | **Union** |
|:-----------------|:----------|
| ![http://www.geoinova.com/examples/intersection.gif](http://www.geoinova.com/examples/intersection.gif) | ![http://www.geoinova.com/examples/union.gif](http://www.geoinova.com/examples/union.gif) |

| **Difference** | **Symmetric Difference** |
|:---------------|:-------------------------|
| ![http://www.geoinova.com/examples/difference.gif](http://www.geoinova.com/examples/difference.gif) | ![http://www.geoinova.com/examples/symmetricdifference.gif](http://www.geoinova.com/examples/symmetricdifference.gif) |

| **Convex Hull** |
|:----------------|
| ![http://www.geoinova.com/examples/convexhull.gif](http://www.geoinova.com/examples/convexhull.gif) |

| **Buffer (positive)** | **Buffer (negative)** |
|:----------------------|:----------------------|
| ![http://www.geoinova.com/examples/posbuffer.gif](http://www.geoinova.com/examples/posbuffer.gif) | ![http://www.geoinova.com/examples/negbuffer.gif](http://www.geoinova.com/examples/negbuffer.gif) |

All of the JTS binary methods support both heterogenous as well as homogeneous arguments:

Intersection of a LineString and a Polygon:

![http://www.geoinova.com/examples/linepolyintersection.gif](http://www.geoinova.com/examples/linepolyintersection.gif)

Union of a MultiPoint and a LineString:

![http://www.geoinova.com/examples/pointlineunion.gif](http://www.geoinova.com/examples/pointlineunion.gif)


### Precision Model ###

JTS supports the concept of an explicit precision model for specifying the geometries' coordinates. Precision models supported are:

  * **`Fixed`** - coordinates are represented as points on a grid with uniform spacing. (This can be assumed to be the integer grid, with the use of appropriate scale and offset factors). Computed coordinates are rounded to this grid.

  * **`Floating`** - coordinates are represented as floating-point numbers. Computed coordinates may have more digits of precision than the input values (up the maximum allowed by the finite floating-point representation).

Implementing a precision model specifies how JTS is to correctly handle constructed points. It also allows control over how the algorithms handle robustness issues.

Please see [A Problem of Accuracy](http://www-sop.inria.fr/prisme/fiches/Arithmetique/index.html.en) article for more info.


### Robustness issues ###

Geometric algorithms involve a combination of combinatorial and numerical computation. As with all numerical computation using finite-precision numbers, the algorithms chosen are susceptible to problems of robustness. A robustness problem occurs when a numerical calculation produces an incorrect answer for some inputs due to round-off errors. Robustness problems are especially serious in geometric computation, since the numerical errors can propagate into the combinatorial computations and result in complete failure of the algorithm.

There are many approaches to dealing with the problem of robustness in geometric computation. Not surprisingly, most robust algorithms are substantially more complex and less performant than the non-robust versions. JTS attempts to deal with the problem of robustness in two ways:

  * The important fundamental geometric algorithms (such as Line Orientation, Line Intersection and the Point-In-Polygon test) have been implemented using robust algorithms. In particular, the implementation of several algorithms relies on the robust determinant evaluation presented in Avnaim et al.

  * The algorithms used to implement the SFS predicates and functions have been developed to eliminate or minimize robustness problems. The binary predicate algorithm is completely robust. The spatial overlay and buffer algorithms are non-robust, but should return correct answers in the majority of cases.

Please see [Robustness and Degeneracies](http://www-sop.inria.fr/prisme/fiches/Robustesse/index.html.en) article for more info.

Part of material provided on this page is Copyright by [The Jump Project](http://www.jump-project.org) and [Vivid Solutions, Inc](http://www.vividsolutions.com).