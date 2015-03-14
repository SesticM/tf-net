## Introduction ##

A reader/writer for Feature Geometry Factory-based geometric primitives exposed by OSGeo Feature Data Objects spatial library. It's purpose is to provide additional set of topological functions in addition to existing ones already provided by FDO geometry core. For example, if FDO provider fails to perform certain spatial query (due to it's design limitations), one can always read FGF geometries to TF.NET and perform the query outside feature source, then write resulting geometries back to FGF. If needed, link between FDO-based feature class and TF.NET geometry can be maintained via `UserData` property, which may carry feature identifier from a feature source. Still, this issue is very very implementation specific.

**FGF Reader/Writer**
relies on open source OSGeo Feature Data Objects spatial library:

  * **FGF Reader** reads OSGeo Feature Data Objects spatial library Feature Geometry Factory-based geometries and creates geometric representation of the features based on JTS model. Curve-based geometries are currently not supported.

  * **FGF Writer** reads features based on JTS model and creates their OSGeo Feature Data Objects spatial library Feature Geometry Factory-based representation.

**AGF Reader/Writer**
references Autodesk's Feature Data Objects spatial library implementation being shipped with Autodesk Map 3D (pre-2008 versions):

  * **AGF Reader** reads Autodesk Feature Data Objects spatial library Feature Geometry Factory-based geometries and creates geometric representation of the features based on JTS model. Curve-based geometries are currently not supported.

  * **AGF Writer** reads features based on JTS model and creates their Autodesk Feature Data Objects spatial library Feature Geometry Factory-based representation.


## Prerequisites ##

All necessary support FDO libraries are available for download [here](http://download.osgeo.org/fdo/3.2.3/fdosdk-3.2.3_win32_release.tar.gz).

Also references TF.NET core library available for download [here](http://code.google.com/p/tf-net/downloads/list).


## Limitations ##

Currently can neither read nor write geometries involving curves.