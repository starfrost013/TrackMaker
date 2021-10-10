﻿using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// AssemblyInfo.cs

// Holds Track Maker versioning information.

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

#if DEBUG
[assembly: AssemblyTitle("Track Maker ['Priscilla' Debug]")]
#else
[assembly: AssemblyTitle("Track Maker 2.0")]
#endif

[assembly: AssemblyDescription("Create hurricane tracks easily and quickly.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]

[assembly: AssemblyProduct("Track Maker")]
[assembly: AssemblyCopyright("Copyright © 2019-2021 starfrost")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]


// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.0.674.21283")]
[assembly: AssemblyFileVersion("2.0.674.21283")]    
[assembly: AssemblyInformationalVersion("2.0.2")]

#if DANO
[assembly: XmlnsDefinition("Dano_XAMLDependent", "Namespace")] 
#if DEBUG

[assembly: AssemblyVersion("3.0.484.20290")]
[assembly: AssemblyFileVersion("3.0.484.20290")]

// Track Maker 3.0 - Dano - Debug
[assembly: XmlnsDefinition("Dano_Debug", "Namespace")] 
#else
// Track Maker 3.0 - Dano - Release
[assembly: XmlnsDefinition("Dano_Release", "Namespace")] 

#endif
#endif

