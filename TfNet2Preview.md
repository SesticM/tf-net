## Introduction ##

TF.NET 2.0 is intended to be a J# port of JTS Topology Suite and JTS Conflation Suite products for .NET, in their latest versions. It's still in Alpha and needs to be thoroughly tested against certain disrepancies between generic (Sun) and Microsoft J# Java language implementations. Moreover, it's direct port of Java source and doesn't ressemble to NTS imlementation. Currently TF.NET 2.0 contains only JTS Topology Suite 1.8.0 classes.

TF.NET 2.0 (Alpha) library can be downloaded using following [link](http://tf-net.googlecode.com/files/TF_NET_2_ALPHA.zip).


## Further Steps ##

Running a .NET port of JTS is a hell of a job, hat down to NTS team for that. Alas, JTS is a live project and it's branching may take even more time and efforts during every iteration (release). My intention is to keep JTS/JCS generic core intact, and offer considerable number of geometry readers/writers for .NET projects implementing NTS and other popular APIs.


## Prerequisites ##

If you intend to reference TF.NET 2.0 libraries via your VB.NET or C# project, you will also need to reference `vjslib.dll` file which is part of J# Redistributable Package.

Microsoft J# Redistributable Package can be freely downloaded from [Visual J# Developer Center](http://msdn2.microsoft.com/en-us/vjsharp/bb188598.aspx). Just pick an appropriate version depending on a .NET redistributable you are currently using.


## Limitations ##

None known yet. It should be tested for `Cloneable` and `SoftReference` types where implemented, though.