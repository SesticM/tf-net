# Topology Framework .NET (TF.NET) #

Topology Framework .NET represents a topology manipulation API capable of handling managed objects representation of topological entities based on other popular APIs, exposing it's JTS-based common topology manipulation core to them.

JTS Topology Suite is, in fact, Java API providing spatial object model and fundamental geometric functions, providing a complete, consistent, robust implementation of fundamental 2D spatial algorithms. It implements geometry model defined in the OpenGIS Consortium Simple Features Specification for SQL. JTS port for .NET was named NTS, and is fully conformant to Microsoft .NET 2.0 specification. NTS extends JTS with numerous coordinate transformation and other functions, while TF.NET extends NTS further, including additional IO functions and generic classes related to geometry graphs. For details regarding described technologies and their respective authors see acknowledgements below.

## Provided Functions ##

  * Spatial predicates (based on the [DE-9IM](http://docs.codehaus.org/display/GEOTDOC/Point+Set+Theory+and+the+DE-9IM+Matrix) model)
  * Overlay functions (intersection, difference, union, symmetric difference)
  * Buffer
  * Convex hull
  * Area and distance functions
  * Topological validity checking
  * Coordinate systems manipulation (transformations)
  * Topological graphs manipulation
  * Generic geometry I/O support: WKT, WKB, GML, SHP

## Supported External Managed APIs ##

  * OSGeo [Feature Data Objects](http://fdo.osgeo.org/) (FDO) geometries
  * OSGeo [MapGuide Server](http://mapguide.osgeo.org/) (FDO-based) geometries
  * Autodesk [ObjectARX](http://usa.autodesk.com/adsk/servlet/index?id=773204&siteID=123112) geometries (a.k.a. entities)
  * Oracle [Data Provider for .NET](http://www.oracle.com/technology/tech/windows/odpnet/index.html) (ODP.NET) geometries - see related [wiki page](http://code.google.com/p/tf-net/wiki/OdpReaderWriter)

## Package Components Available For Download ##

  * **TF.NET Core Library**, a mandatory component exposing basic topology manipulation API.
  * **[TF.NET Reader/Writer for MapGuide](http://code.google.com/p/tf-net/wiki/MapGuideReaderWriter)**, a reader/writer for geometric primitives exposed by OSGeo MapGuide Server.
  * **[TF.NET Reader/Writer for FDO](http://code.google.com/p/tf-net/wiki/FdoReaderWriter)**, a reader/writer for geometric primitives exposed by OSGeo Feature Data Objects spatial library.
  * **[TF.NET Reader/Writer for ObjectARX](http://code.google.com/p/tf-net/wiki/DwgReaderWriter)**, a reader/writer for geometric entities exposed by Autodesk AutoCAD and it's verticals.

## References And Further Reading ##

  * TF.NET [API Help](http://code.google.com/p/tf-net/downloads/list) (CHM file)
  * JTS Topology Suite [API Help](http://www.vividsolutions.com/jts/javadoc/index.html)
  * JTS Topology Suite [Technical Specifications](http://www.vividsolutions.com/jts/bin/JTS%20Technical%20Specs.pdf) (PDF file)
  * JTS Topology Suite [Developer's Guide](http://www.vividsolutions.com/jts/bin/JTS%20Developer%20Guide.pdf) (PDF file)
  * JTS Topology Suite explained on [The Jump Project](http://www.jump-project.org/project.php?PID=JTS&SID=OVER) page
  * [References and Further Reading](http://code.google.com/p/tf-net/wiki/References) wiki page

## Credits ##

TF.NET is primarily based on other people's work related to topology manipulation, hence special acknowledgement to them. Referred libraries were additionally repackaged and their namespaces renamed for easier use. Please download copyright disclaimer for detailed description.

  * Vivid Solutions' [JTS Topology Suite](http://www.vividsolutions.com/jts/jtshome.htm)
  * Diego Guidi's [NetTopologySuite](http://code.google.com/p/nettopologysuite/)
  * Morten Nielsen's [SharpMap](http://www.codeplex.com/SharpMap/)
  * Urban Science Applications' [GeoTools.NET](http://sourceforge.net/projects/geotoolsnet/)
  * Jonathan de Halleux's [QuickGraph](http://www.codeplex.com/quickgraph/)
  * Ryan Seghers' [RTools.Util](http://www.codeproject.com/cs/library/rtoolsutil.asp)
  * Jason Smith's [Iesi.Collections](http://www.codeproject.com/csharp/sets.asp)
  * Kailuo Wang's [Iesi.Collections.Generic](http://www.codeproject.com/csharp/GenericISet.asp)
  * Bruno Lowagie and Paulo Soares' [iText](http://sourceforge.net/projects/itextdotnet/)
  * Dennis Jonio for articles on ODP.NET implementation

For more information please contact project owner: [Maksim Sestic](mailto:max@geoinova.com)