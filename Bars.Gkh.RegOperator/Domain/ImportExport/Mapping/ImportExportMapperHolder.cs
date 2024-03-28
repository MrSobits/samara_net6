namespace Bars.Gkh.RegOperator.Domain.ImportExport.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using B4.Utils;
    using Castle.Windsor;

    public class ImportExportMapperHolder
    {
        private readonly Dictionary<string, ProviderMapper> _mapInfo;

        public ImportExportMapperHolder(IWindsorContainer container)
        {
            _mapInfo = new Dictionary<string, ProviderMapper>();

            var maps = container.ResolveAll<IImportMap>();

            foreach (var importMap in maps)
            {
                foreach (var providerMapper in importMap.GetMap())
                {
                    var key = CreateKey(importMap.ObjectType, providerMapper.Key, importMap);

                    _mapInfo[key] = providerMapper.Value;
                }
            }
        }

        public ProviderMapper GetMapper<T>(MemberInfo mInfo, IImportMap format)
        {
            var key = CreateKey(typeof (T), mInfo.Name, format);

            return _mapInfo.Get(key);
        }

        private string CreateKey(Type type, string memberName, IImportMap format)
        {
            return "{0}|{1}|{2}|{3}|{4}".FormatUsing(type.Name, memberName, format.ProviderCode, format.Format, format.Direction);
        }
    }
}