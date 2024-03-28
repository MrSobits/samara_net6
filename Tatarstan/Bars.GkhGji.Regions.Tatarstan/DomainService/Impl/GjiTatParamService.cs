namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.Utils;

    using Castle.MicroKernel.Lifestyle;
    using Entities;
    using Gkh.Domain;

    public class GjiTatParamService : IGjiTatParamService
    {
        private const string Prefix = "GjiTat";

        private readonly List<string> _keys = new List<string>
        {
            "GisGmpEnable",

            "GisGmpProxy",
            "GisGmpProxyUser",
            "GisGmpProxyPassword",

            "GisGmpPatternCode",
            "GisGmpSystemCode",

            "GisGmpUriUpload",

            "GisGmpUriLoad",
            "GisGmpLoadTime",
            "GisGmpPayeeInn",
            "GisGmpPayeeKpp",
            "GisGmpLogEnable"
        };

        public DynamicDictionary GetConfig()
        {
            var config = GetConfigInternal();

            var dict = new DynamicDictionary();

            foreach (var key in _keys)
            {
                dict[key] = config.Get(key).Return(x => x.Value);
            }

            return dict;
        }

        public IDataResult SaveConfig(BaseParams baseParams)
        {
            var paramsToSave = baseParams.Params;

            var config = GetConfigInternal();

            var listToSave = new List<GjiTatParam>();

            var domain = ApplicationContext.Current.Container.ResolveDomain<GjiTatParam>();

            foreach (var key in _keys)
            {
                var parameter = config.Get(key) ?? new GjiTatParam
                {
                    Key = key,
                    Prefix = Prefix
                };

                parameter.Value = Cut(paramsToSave.GetAs<string>(key));

                listToSave.Add(parameter);
            }

            SaveOrUpdate(domain, listToSave);

            return new BaseDataResult();
        }

        public IDataResult SaveDict(BaseParams baseParams)
        {
            var entities = baseParams.Params.GetAs<List<GisGmpPatternDict>>("records"); 
            if (entities.Count > 0)
            {
                var domain = ApplicationContext.Current.Container.ResolveDomain<GisGmpPatternDict>();

                ApplicationContext.Current.Container.InTransaction(() =>
                {
                    foreach (var entity in entities)
                    {
                        if (entity.Id > 0)
                        {
                            domain.Update(entity);
                        }
                        else
                        {
                            domain.Save(entity);
                        }
                    }
                });
            }

            return new BaseDataResult();
        }

        private Dictionary<string, GjiTatParam> GetConfigInternal()
        {
            var domain = ApplicationContext.Current.Container.ResolveDomain<GjiTatParam>();
            var resultDict = new Dictionary<string, GjiTatParam>();
            using (ApplicationContext.Current.Container.BeginScope())
            {
                resultDict = domain.GetAll()
                    .Where(x => x.Prefix == Prefix)
                    .ToDictionary(x => x.Key);
            }

            return resultDict;
        }

        private string Cut(string value, int length = 2000)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > length)
            {
                return value.Substring(0, 2000);
            }

            return value;
        }

        private void SaveOrUpdate(IDomainService<GjiTatParam> domain, IEnumerable<GjiTatParam> entities)
        {
            ApplicationContext.Current.Container.InTransaction(() =>
            {
                foreach (var entity in entities)
                {
                    if (entity.Id > 0)
                    {
                        domain.Update(entity);
                    }
                    else
                    {
                        domain.Save(entity);
                    }
                }
            });
        }
    }
}