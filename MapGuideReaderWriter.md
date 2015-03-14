## Introduction ##

A reader/writer for geometric primitives exposed by OSGeo MapGuide Server.
Before further reading, please see [FDO Reader/Writer](http://code.google.com/p/tf-net/wiki/FdoReaderWriter) and [Binary Predicates](http://code.google.com/p/tf-net/wiki/JtsBinaryPredicates) introductory notes. Although MapGuide relays on it's own (FDO-based) spatial filters, there are situations when they are simply not enough - most often FDO providers don't expose necessary binary predicates by design. On the other hand, you can use rich JTS API to perform targeted geospatial analysis and bounce results back to MapGuide in some form (i.e. redlining, feature or spatial filter, etc).

**MapGuide Reader** reads MapGuide Server geometries and creates geometric representation of the features based on JTS model. Curve-based geometries are currently not supported.

**MapGuide Writer** reads features based on JTS model and creates their MapGuide Server representation.


## Components ##

A `Topology.IO.MapGuide.dll` library file available for download [here](http://code.google.com/p/tf-net/downloads/list). Library exposes `MgReader` and `MgWriter` classes residing within `Topology.IO.MapGuide` namespace.


## Prerequisites ##

References MapGuide Server's `MapGuideDotNetApi.dll` library, which in turn references other unmanaged libraries. All necessary support libraries are available for download [here](http://download.osgeo.org/mapguide/releases/1.2.0/mapguide-1.2.0.tar.gz).

If you already have any running version of MapGuide Server (either OSS or Enterprise), you can simply reference it's libraries found in `..\WebServerExtensions\www\mapviewernet\bin` folder, rather than downloading full set of binaries using link above.

Also references TF.NET core library available for download [here](http://code.google.com/p/tf-net/downloads/list).


## Limitations ##

Currently can neither read nor write geometries involving curves.