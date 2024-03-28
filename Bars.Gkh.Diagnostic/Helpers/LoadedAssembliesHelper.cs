namespace Bars.Gkh.Diagnostic.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Diagnostic.DomainServices;

    internal static class LoadedAssembliesHelper
    {
        public static List<SerializableAssemblyInfo> GetLoadedAssemblies()
        {

            return  AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .Where(x => !x.IsDynamic)
                    .Select(x => new SerializableAssemblyInfo(x))
                    .ToList();
        }
    }
}
