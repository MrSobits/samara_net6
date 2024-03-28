namespace Bars.Gkh.RegOperator.Domain.ImportExport.Mapping
{
    using System.Linq;
    using B4.Utils;
    using Bars.B4.IoC;
    using Castle.Windsor;

    public static class ImportMapHelper
    {
        public static string GetKey(this IImportMap map)
        {
            return "{0}|{1}|{2}|{3}".FormatUsing(map.ObjectType.Name, map.ProviderCode, map.Format, map.Direction);
        }

        public static IImportMap GetMapByKey(string key, IWindsorContainer container)
        {
            var maps = container.ResolveAll<IImportMap>();

            using (container.Using(maps))
            {
                return maps.FirstOrDefault(x => x.GetKey() == key);
            }
        }
    }
}