# Building TF.NET on Windows #

TF.NET consists of a single Core library accompanied by number of Reader/Writer libraries. Building Core library is rather straightforward since it's dependant on .NET 2.0 Framework only. It is Readers/Writers you should understand a bit more in-depth.
TF.NET Readers/Writers are separate libraries, each depending on it's underlying platform/API. To be able to build specific Reader/Writer you need to reference necessary files not found in the SVN Trunk due to the possible copyright issues.

## SVN Folder Structure ##

A short guide to folder structure found in a Trunk:

  * `..\Src` folder - contains source code and project file(s)
  * `..\Inc` folder - keeps referenced (external) libraries, contains References.txt file explaining which ones
  * `..\Bin` folder - build target, that's where compiled files end up

## Building TF.NET Core libraries ##

  1. Open and build Core.sln solution first
  1. Compiled files end up in `..\Core\Bin folder`
  1. Run `TF-MERGE.bat` to link them into a single Topology.dll using VS.NET `ILMerge.exe` tool

## Building TF.NET Readers/Writers libraries ##

  1. Provide necessary referenced files for Readers/Writers you want to build
  1. Copy referenced libraries into Reader/Writer's `..\Inc` folder
  1. Open `ReaderWriter.sln` solution
  1. Unload projects for Readers/Writers you don't want included and run build
  1. Compiled files end up in Readers/Writers' `..\Bin` folder

## Building Help files ##

  1. After both TF.NET Core and Readers/Writers libraries are built...
  1. Go to `..\Docs` folder and open `Topology.ndoc` using NDoc 2.0 or NDoc 2005
  1. Strip libraries you don't want included in a help file and compile it

## Referencing TF.NET Core and Reader/Writer libraries in your solution ##

  1. Simply harvest compiled .dll files from each `..\Bin` folder and reference them in your project
  1. Don't forget to reference files found in `..\Inc` folders too