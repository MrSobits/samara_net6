namespace Bars.Gkh.DomainService.GkhParam.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.Gkh.Extensions;

    using Castle.Windsor;
    using Entities;

    using Microsoft.AspNetCore.SignalR;

    using Newtonsoft.Json;
    using SignalR;

    public class GkhParamService : IGkhParamService
    {
        private Dictionary<string, string> _paramCache; 

        public GkhParamService()
        {
            _paramCache = new Dictionary<string, string>();
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<GkhParam> GkhParamDomain { get; set; }

        public IDataResult SaveParams(string prefix, object parameters)
        {
            var @params = new BaseParams
            {
                Params =
                    DynamicDictionary.Create()
                        .SetValue(
                            "params",
                            DynamicDictionary.FromJson(
                                JsonConvert.SerializeObject(parameters),
                                new[] {new DynamicDictionaryJsonConverter()}))
                        .SetValue("prefix", prefix)
            };

            return SaveParams(@params);
        }

        public IDataResult SaveParams(BaseParams baseParams)
        {
            _paramCache.Clear();

            var regopparams = baseParams.Params.GetAs<DynamicDictionary>("params");
            var prefix = baseParams.Params.GetAs<string>("prefix");

            if (regopparams == null)
            {
                return new BaseDataResult(false, "Ошибка получения параметров");
            }

            var paramsDict = GkhParamDomain.GetAll()
                .Where(x => x.Prefix == prefix)
                .AsEnumerable()
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.First());

            foreach (var regopparam in regopparams)
            {
                var paramValue = regopparam.Value.ToStr();

                if (paramsDict.ContainsKey(regopparam.Key))
                {
                    var paramToUpdate = paramsDict[regopparam.Key];

                    if (paramToUpdate.Value != paramValue)
                    {
                        paramToUpdate.Value = paramValue;

                        GkhParamDomain.Update(paramToUpdate);

                        paramsDict[regopparam.Key] = paramToUpdate;
                    }
                }
                else
                {
                    var newParam = new GkhParam
                    {
                        Prefix = prefix, 
                        Key = regopparam.Key, 
                        Value = paramValue
                    };

                    GkhParamDomain.Save(newParam);

                    paramsDict[regopparam.Key] = newParam;
                }
            }

            UpdateClientParams();

            return new BaseDataResult();
        }

        public IDataResult GetParams(string prefix = "")
        {
            var result = GkhParamDomain.GetAll()
                .WhereIf(!prefix.IsEmpty(), x => x.Prefix == prefix)
                .Select(x => new { x.Key, x.Value })
                .AsEnumerable()
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x =>
                {
                    object res;
                    int intValue;
                    var value = x.First().Value;

                    if (int.TryParse(value, out intValue))
                    {
                        res = intValue;
                    }
                    else
                    {
                        res = value;
                    }

                    return res;
                });

            return new BaseDataResult(result);
        }

        public IDataResult GetClientParams()
        {
            return new BaseDataResult(CreateJsonObjectFromParameters());
        }

        public string GetParamByKey(string key, string prefix)
        {
            var cacheKey = string.Concat(prefix, key);

            string cachedValue;
            if (!_paramCache.TryGetValue(cacheKey, out cachedValue))
            {
                cachedValue = GkhParamDomain.GetAll()
                    .Where(x => x.Key == key)
                    .WhereIf(!prefix.IsEmpty(), x => x.Prefix == prefix)
                    .Select(x => x.Value)
                    .FirstOrDefault();

                _paramCache[cacheKey] = cachedValue;
            }

            return cachedValue;
        }

        private void UpdateClientParams()
        {
            object parameters = CreateJsonObjectFromParameters();

            this.Container.UsingForResolved<IHubContext<GkhParamsHub, IGkhParamsHubClient>>((_, context) =>
            {
                context
                    .Clients.All
                    .UpdateParams(JsonConvert.SerializeObject(parameters, new DynamicDictionaryJsonConverter()))
                    .GetResultWithoutContext();
            });
        }

        private object CreateJsonObjectFromParameters()
        {
            var allParameters = GkhParamDomain.GetAll().ToList().GroupBy(x => x.Prefix);

            dynamic result = DynamicDictionary.Create();

            foreach (var parameterNs in allParameters)
            {
                dynamic values = DynamicDictionary.Create();
                result[parameterNs.Key.IsEmpty() ? "Common" : parameterNs.Key] = values;

                foreach (var parameter in parameterNs)
                {
                    values[parameter.Key] = parameter.Value;
                }
            }

            return result;
        }
    }
}