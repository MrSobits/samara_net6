namespace Bars.Gkh.Gji.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;

    public class GjiParamsService : IGjiParamsService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<GjiParam> GjiParamDomain { get; set; }

        public IDataResult SaveParams(BaseParams baseParams)
        {
            var gjipparams = baseParams.Params.GetAs<DynamicDictionary>("gjiparams");

            if (gjipparams == null)
            {
                return new BaseDataResult(false, "Ошибка получения параметров");
            }

            var paramsDict = GjiParamDomain.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.First());

            foreach (var gjiparam in gjipparams)
            {
                var paramValue = gjiparam.Value.ToStr();

                if (paramsDict.ContainsKey(gjiparam.Key))
                {
                    var paramToUpdate = paramsDict[gjiparam.Key];

                    if (paramToUpdate.Value != paramValue)
                    {
                        paramToUpdate.Value = paramValue;

                        GjiParamDomain.Update(paramToUpdate);

                        paramsDict[gjiparam.Key] = paramToUpdate;
                    }
                }
                else
                {
                    var newParam = new GjiParam{ Key = gjiparam.Key, Value = paramValue };

                    GjiParamDomain.Save(newParam);

                    paramsDict[gjiparam.Key] = newParam;
                }
            }

            return new BaseDataResult();
        }

        public IDataResult GetParams()
        {
            var result = GjiParamDomain.GetAll()
                .Select(x => new { x.Key, x.Value })
                .AsEnumerable()
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.First().Value);

            return new BaseDataResult(result);
        }

        public string GetParamByKey(string key)
        {
            return GjiParamDomain.GetAll().Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault();
        }

    }
}