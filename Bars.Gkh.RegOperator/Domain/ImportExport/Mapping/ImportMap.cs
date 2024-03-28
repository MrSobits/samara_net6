namespace Bars.Gkh.RegOperator.Domain.ImportExport.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using B4.Utils;

    public interface ImportMap
    {
        Dictionary<MemberInfo, Dictionary<string, ProviderMapper>> GetCompiledMap();
    }

    public class ImportMap<T> : ImportMap
    {
        private Dictionary<MemberInfo, Dictionary<string, ProviderMapper>> _compiledMap;
        
        public void Map(Expression<Func<T, object>> member, params PropertyMapper[] propMappers)
        {
            if (_compiledMap == null)
            {
                _compiledMap = new Dictionary<MemberInfo, Dictionary<string, ProviderMapper>>();
            }

            var mInfo = member.MemberInfo();

            foreach (var propertyMapper in propMappers)
            {
                var key = CreateKey(propertyMapper.Settings);

                if (_compiledMap.ContainsKey(mInfo))
                {
                    if (_compiledMap[mInfo].ContainsKey(key))
                    {
                        _compiledMap[mInfo][key] = propertyMapper.ProviderMapper;
                    }
                    else
                    {
                        _compiledMap[mInfo].Add(key, propertyMapper.ProviderMapper);
                    }
                }
                else
                {
                    _compiledMap.Add(mInfo, new Dictionary<string, ProviderMapper>(){{key, propertyMapper.ProviderMapper}});
                }
            }
        }

        public Dictionary<MemberInfo, Dictionary<string, ProviderMapper>> GetCompiledMap()
        {
            return _compiledMap;
        }

        private string CreateKey(ImportExportSettings settings)
        {
            return string.Format("{0}|{1}|{2}", settings.Format, settings.Type, settings.Party);
        }
    }
}