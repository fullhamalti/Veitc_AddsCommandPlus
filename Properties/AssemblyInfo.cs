#region Using Directives
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Sims3.SimIFace;
using System.Diagnostics;
#endregion

#region SimIFace Lib
// SimIFace
[assembly: Tunable]
[assembly: PersistableStatic]
#endregion

#region mscorlib
// mscorlib
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(_thisAssembly._name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Veitc")]
[assembly: AssemblyProduct(_thisAssembly._name)]
[assembly: AssemblyCopyright("Copyright © 2020 Fullham Alfayet")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
 
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("82adf691-f68b-4b48-9cb1-7f72cd4ce527")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")] "1.0.0.0"
[assembly: AssemblyVersion(_thisAssembly._version)]
//[assembly: AssemblyFileVersion("1.0.0.0")]
#endregion