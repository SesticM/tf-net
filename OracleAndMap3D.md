# Summary #

Within this series of articles I have pretty well illustrated Oracle’s ODP.NET working in "harmony" with Autodesk’s Map3d 2008. Also note that I have used Oracle XE throughout. The functionality Locator brings is certainly more than enough to build a reasonable GIS data store. I have not spent any time in outlining any of the other Oracle stored procedures like `SDO_GEOM.VALIDATE_GEOMETRY_WITH_CONTEXT()` these are all well documented across the internet. As I have said before, FDO is the way to go. But you still need to get that geometry built somewhere and put into a data store. "Map Export to FDO" is obviously a work in progress but hopefully will become a useful tool down the road.

# Lessons Learned #

  * Always turn Connection Pooling OFF when working with UDT’s.
  * Do NOT use the App.config method for UDT discovery that is described in the ODP.NET documentation. This does no work in all cases. It seems nested classes are not always resolved.
  * Since your UDT class assembly(s) will be referenced from, in the default case: "C:\Program Files\AutoCAD Map 3D 2008", be careful of your naming convention.

Here is an example sequence of events:

```
using Autodesk.AutoCAD.ApplicationServices;

using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.AutoCAD.EditorInput;

using Autodesk.AutoCAD.Runtime;

using Autodesk.AutoCAD.Geometry;

 

using System.Reflection;

using System.Collections;

using System.IO;

using System.Text;

using System.Data;

using System;

 

using Oracle.DataAccess.Client;

using Oracle.DataAccess.Types;

 

using NetSdoGeometry;

 

[assembly: ExtensionApplication(typeof(YOURCLEVERAPPNAME.Initialization))]

[assembly: CommandClass(typeof(YOURCLEVERAPPNAME.YOURCLEVERAPPNAME))]

namespace YOURCLEVERAPPNAME

{

    public class YOURCLEVERAPPNAME

    {

        public static string LocationOfDll;

        public static string forReadDwgFileInit = "forReadDwgFileInit.dwg";

        public YOURCLEVERAPPNAME()

        {

            Assembly callee = Assembly.GetExecutingAssembly();

            LocationOfDll = Path.GetDirectoryName(callee.Location);

        }

       [CommandMethod("YOURCLEVERAPPNAME")]

        public void mycleverapp()

        {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            Editor ed = doc.Editor;

            //

            // Must do this before any Oracle.DataAccess method calls.

            // If you start getting “First chance exceptions” in VS you know things are hosed.

            // 

            Database Xdb = new Database(false, false);

            Xdb.ReadDwgFile(LocationOfDll + "\\" + forReadDwgFileInit,FileShare.ReadWrite, false, null);

            Xdb.Dispose();

            //

            // I do not understand but I needed this HERE to resolve sdo_geometry

            //

            sdogeometry NEED_HERE_TO_RESOLVE_sdogeometry = new sdogeometry();

            using (DocumentLock docLock = doc.LockDocument())

            {

                  // do whatever needs doing

      }

 

  }

    }//eoc YOURCLEVERAPPNAME

 

    public class Initialization : Autodesk.AutoCAD.Runtime.IExtensionApplication

    {

        public void Initialize()

        {

            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;

            Assembly caller = Assembly.GetCallingAssembly();

            Assembly callee = Assembly.GetExecutingAssembly();

            string callername = caller.FullName;

            string calleename = callee.FullName;

            ed.WriteMessage("Loader: " + callername + " initialize " + calleename);

        }

        public void Terminate()

        {

            Assembly callee = Assembly.GetExecutingAssembly();

            string calleename = callee.FullName;

            Console.WriteLine("Cleaning up..." + calleename);

        }

    }//eoc Initialize

}//eons
```

Hope you have enjoyed...

# Credits #

This wiki article was kindly contributed by Dennis Jonio.