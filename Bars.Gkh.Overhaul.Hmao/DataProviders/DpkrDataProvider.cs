namespace Bars.Gkh.Overhaul.Hmao.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.DataProviders.Meta;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Castle.Windsor;
    using NHibernate.Linq;

    public class DpkrDataProvider : BaseCollectionDataProvider<ДПКР>
    {
        public DpkrDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        protected override IQueryable<ДПКР> GetDataInternal(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>(string.Format("{0}_municipality", Key));
            var collectFrom = baseParams.Params.GetAs<DpkrDataSource>(string.Format("{0}_collect_from", Key));

            var municipalityDomain = Container.ResolveDomain<Municipality>();

            using (Container.Using(municipalityDomain))
            {
                var mu = municipalityDomain.FirstOrDefault(x => x.Id == municipalityId);

                switch (collectFrom)
                {
                    case DpkrDataSource.MainVersion:
                        return CollectFromMainVersion(mu);
                    case DpkrDataSource.PublishedProgram:
                        return CollectFromPublishedProgram(mu);
                }
            }
            return new ДПКР[] { }.AsQueryable();
        }

        public override string Name
        {
            get { return "Данные по ДПКР"; }
        }

        public override string Description
        {
            get { return "Данные по ДПКР (по основной и опубликованной версии программы)"; }
        }

        public override IEnumerable<DataProviderParam> Params
        {
            get
            {
                return new[]
                {
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_municipality", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Муниципальное образование",
                        Additional = "Municipality",
                        Required = true,
                    },
                    
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_collect_from", Key),
                        ParamType = ParamType.Enum,
                        Label = "Сборка по",
                        Additional = "DpkrDataSource",
                        Required = true,
                    }
                };
            }
        }

        /// <summary>
        /// Сбор данных по опубликованной программе
        /// </summary>
        /// <param name="mu"></param>
        /// <returns></returns>
        protected IQueryable<ДПКР> CollectFromPublishedProgram(Municipality mu)
        {
            var domain = Container.ResolveDomain<PublishedProgramRecord>();
            var versionRecordDomain = Container.ResolveDomain<VersionRecord>();
            var version1Domain = Container.ResolveDomain<VersionRecordStage1>();
            using (Container.Using(domain, versionRecordDomain, version1Domain))
            {
                var dataPublished =
                    domain.GetAll()
                        .Where(x => x.Stage2 != null)
                        .WhereIf(mu.Id > 0, x => x.PublishedProgram.ProgramVersion.IsMain && x.PublishedProgram.ProgramVersion.Municipality.Id == mu.Id)
                        .Select(x => new {x.Stage2.Stage3Version.Id, x.Stage2.Stage3Version.Year})
                        .ToList()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Year).FirstOrDefault());


                var versionStage2Ids = dataPublished.Keys;

                var volumesByVersion2 = version1Domain.GetAll().Where(x => versionStage2Ids.Contains(x.Stage2Version.Id))
                    .ToList()
                    .GroupBy(x => x.Stage2Version.Id)
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Volume));

                var query =
                    versionRecordDomain.GetAll()
                        .Fetch(x => x.RealityObject)
                        .ThenFetch(x => x.FiasAddress)
                        .WhereIf(mu.Id > 0, x => x.ProgramVersion.IsMain && x.ProgramVersion.Municipality.Id == mu.Id)
                        .Where(x => domain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                        .Select(x => new
                        {
                            x.Id,
                            x.RealityObject,
                            x.Sum,
                            CommonEstateobject = x.CommonEstateObjects,
                            x.IndexNumber
                        })
                        .ToList();

                var result = query
                    .Select(x =>
                        new ДПКР
                        {
                            ПлощадьДома = x.RealityObject.AreaMkd.GetValueOrDefault(),
                            ООИ = x.CommonEstateobject,
                            Стоимость = x.Sum,
                            ГодРемонта = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id] : 0,
                            МО = x.RealityObject.Municipality.Name,
                            Адрес = x.RealityObject.GetLocalAddress(),
                            НаселённыйПункт = x.RealityObject.FiasAddress.PlaceName,
                            Площадь = volumesByVersion2.ContainsKey(x.Id) ? volumesByVersion2[x.Id] : 0
                        }).AsQueryable().ToArray();

                return result.AsQueryable();
            }
        }

        /// <summary>
        /// Сбор данных по основной версии
        /// </summary>
        /// <param name="mu"></param>
        /// <returns></returns>
        protected IQueryable<ДПКР> CollectFromMainVersion(Municipality mu)
        {
            var domain = Container.ResolveDomain<PublishedProgramRecord>();
            var version1Domain = Container.ResolveDomain<VersionRecordStage1>();
            using (Container.Using(domain, version1Domain))
            {
                var mainProgram = domain.GetAll()
                    .WhereIf(mu.Id > 0,
                        x =>
                            x.PublishedProgram.ProgramVersion.IsMain &&
                            x.PublishedProgram.ProgramVersion.Municipality.Id == mu.Id).ToList();

                var versionStage2Ids = mainProgram.Select(x => x.Stage2.Id).ToList();

                var volumesByVersion2 = version1Domain.GetAll()
                    .Where(x => x.Stage2Version != null)
                    .Where(x => versionStage2Ids.Contains(x.Stage2Version.Id))
                    .ToList()
                    .GroupBy(x => x.Stage2Version.Id)
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Volume));

                var result = mainProgram.Select(x => new ДПКР
                {
                    ПлощадьДома = x.RealityObject.AreaMkd.GetValueOrDefault(),
                    ООИ = x.CommonEstateobject,
                    Стоимость = x.Sum,
                    ГодРемонта = x.Stage2?.Stage3Version.Year ?? 0,
                    МО = x.RealityObject.Municipality.Name,
                    Адрес = x.RealityObject.GetLocalAddress(),
                    НаселённыйПункт = x.Locality,
                    Площадь = x.Stage2 != null && volumesByVersion2.ContainsKey(x.Stage2.Id) ? volumesByVersion2[x.Stage2.Id] : 0
                }).AsQueryable().ToArray();

                return result.AsQueryable();
            }
        }
    }
}
