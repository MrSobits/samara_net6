namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using B4.Utils;

    using Bars.B4.DomainService.BaseParams;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using Castle.Windsor;
    using Overhaul.Entities;
    using CollectionExtensions = Castle.Core.Internal.CollectionExtensions;

    public partial class DpkrService : IDpkrService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<Bars.Gkh.Entities.RealityObject> RoDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> strElDomain { get; set; }

        public IDomainService<VersionRecordStage1> versStage1Domain { get; set; }

        public IDomainService<LoadProgram> LoadProgramDomain { get; set; }
    
        public IDataResult GetYears(BaseParams baseParams)
        {
            // ToDo Активировать блок когда получение данных станет как раньше нормальным, а не через загрузку своих данных
            #region Old
            //var config = Container.GetGkhConfig<OverhaulConfig>();
            //var startYear = config.OverhaulNsoConfig.ProgrammPeriodStart;
            //var endYear = config.OverhaulNsoConfig.ProgrammPeriodEnd;

            //var years = new List<int>();

            //while (startYear <= endYear)
            //{
            //    years.Add(startYear);
            //    startYear++;
            //}

            //return new BaseDataResult(years);
            #endregion

            var yearList =
                LoadProgramDomain.GetAll().Select(x => x.PlanOverhaulYear).AsEnumerable().Distinct().OrderBy(x => x).ToList();

            return new BaseDataResult(yearList);
        }

        public IDataResult GetMunicipality(BaseParams baseParams)
        {
            // ToDo Активировать блок когда получение данных станет как раньше нормальным, а не через загрузку своих данных
            #region Old
            //var municipalityList = MunicipalityDomain.GetAll()
            //                    .Select(x => new { x.Id, x.Name })
            //                    .OrderBy(x => x.Name)
            //                    .ToList();

            //return new BaseDataResult(municipalityList);
            #endregion

            // так как в загружаемом файле нет МО, то фильтруем по населенному пункту 
            var municipalityList = LoadProgramDomain.GetAll()
                                .Select(x => x.Locality)
                                .AsEnumerable()
                                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                                .Select(x => new
                                             {
                                                 Id = x,
                                                 Name = x
                                             })
                                .OrderBy(x => x.Name)
                                .ToList();

            return new BaseDataResult(municipalityList);
        }

        public IDataResult GetRealityObjects(BaseParams baseParams)
        {
            // ToDo Активировать блок когда получение данных станет как раньше нормальным, а не через загрузку своих данных
            #region Old
            //var municipalityId = baseParams.Params.GetAs<long>("muId");
            //var address = baseParams.Params.GetAs("address", string.Empty);

            //if (!string.IsNullOrEmpty(address))
            //{
            //    address = address.Replace(" ", "").Trim().ToUpper();
            //}

            //var roList = RoDomain.GetAll()
            //                    .WhereIf(municipalityId > 0, x => x.Municipality.Id == municipalityId)
            //                    .WhereIf(!string.IsNullOrEmpty(address), x => x.FiasAddress.AddressName.Replace(" ", "").ToUpper().Contains(address))
            //                    .Select(x => new { x.Id, x.Address })
            //                    .OrderBy(x => x.Address)
            //                    .ToList();

            //return new BaseDataResult(roList);
            #endregion

            // так как в загружаемом файле нет МО, то фильтруем по населенному пункту 
            var locality = baseParams.Params.GetAs("muId", string.Empty);
            var address = baseParams.Params.GetAs("address", string.Empty);

            if (!string.IsNullOrEmpty(address))
            {
                address = address.Replace(" ", "").Trim().ToUpper();
            }

            var roList = LoadProgramDomain.GetAll()
                                .WhereIf(!CollectionExtensions.IsNullOrEmpty(locality), x => x.Locality == locality)
                                .WhereIf(!string.IsNullOrEmpty(address), x => x.Address.Replace(" ", "").ToUpper().Contains(address))
                                .Select(x => x.Address)
                                .AsEnumerable()
                                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                                .Select(x => new
                                             {
                                                 Id = x,
                                                 Address = x
                                             })
                                .OrderBy(x => x.Address)
                                .ToList();

            return new BaseDataResult(roList);
        }

        public ListDataResult GetRecords(BaseParams baseParams)
        {
            // ToDo Активировать блок когда получение данных станет как раньше нормальным, а не через загрузку своих данных
            #region Old
            //var loadParams = GetLoadParam(baseParams);

            //var year = baseParams.Params.GetAs<long>("year");
            //var municipalityId = baseParams.Params.GetAs<long>("muId");
            //var roId = baseParams.Params.GetAs<long>("roId");

            //var config = Container.GetGkhConfig<OverhaulConfig>();
            //var startYear = config.OverhaulNsoConfig.ProgrammPeriodStart;

            //var data =
            //    DpkrCorrectionDomain.GetAll()
            //                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
            //                        .WhereIf(year > 0, x => x.PublicationYear == year)
            //                        .WhereIf(municipalityId > 0, x => x.RealityObject.Municipality.Id == municipalityId)
            //                        .WhereIf(roId > 0, x => x.RealityObject.Id == roId)
            //                        .Select(
            //                            x =>
            //                            new
            //                            {
            //                                Id = x.Id,
            //                                x.Stage2.Stage3Version.IndexNumber,
            //                                st2Id = x.Stage2.Id,
            //                                Address = x.RealityObject.FiasAddress.AddressName,
            //                                MunicipalityName = x.RealityObject.Municipality.Name,
            //                                AddrPlace = x.RealityObject.FiasAddress.PlaceName,
            //                                AddrStreet = x.RealityObject.FiasAddress.StreetName,
            //                                AddrHouse = x.RealityObject.FiasAddress.House,
            //                                AddrHousing = x.RealityObject.FiasAddress.Housing,
            //                                YearComissioning = x.RealityObject.BuildYear.HasValue ? x.RealityObject.BuildYear.Value : 0, // Просили поменять год Эксплуатации на год постройки
            //                                CeoName = x.Stage2.CommonEstateObject.Name,
            //                                YearPublication = x.PublicationYear
            //                            })
            //                        .OrderBy(x => x.IndexNumber);

            //var totalCount = data.Count();

            ////поулчаем отрезанные записи по Пейджингу
            //var list = data.Paging(loadParams).ToList();

            //// поулчаем только Id 2 этапа версии
            //var listIds = list.Select(x => x.st2Id).Distinct().ToList();

            //var st1DataList = versStage1Domain.GetAll()
            //                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
            //                    .Where(x => listIds.Contains(x.Stage2Version.Id))
            //                    .WhereIf(municipalityId > 0, x => x.RealityObject.Municipality.Id == municipalityId)
            //                    .WhereIf(roId > 0, x => x.RealityObject.Id == roId)
            //                    .Select(x => new { x.StructuralElement.Id, st2Id = x.Stage2Version.Id })
            //                    .ToList();

            //var listStrElIds = st1DataList.Select(x => x.Id).Distinct().ToList();

            //var st1Data = st1DataList
            //                    .GroupBy(x => x.st2Id)
            //                    .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());

            //var strElData = strElDomain.GetAll()
            //                           .Where(x => listStrElIds.Contains(x.Id))
            //                           .WhereIf(
            //                               municipalityId > 0, x => x.RealityObject.Municipality.Id == municipalityId)
            //                           .WhereIf(roId > 0, x => x.RealityObject.Id == roId)
            //               .Select(x => new { x.Id, x.LastOverhaulYear })
            //               .Where(x => x.LastOverhaulYear > 0)
            //               .AsEnumerable()
            //               .GroupBy(x => x.Id)
            //               .ToDictionary(x => x.Key, y => y.Select(x => x.LastOverhaulYear).First());

            //var result = list.Select(x => new
            //{
            //    x.Id,
            //    x.MunicipalityName,
            //    x.AddrPlace,
            //    x.AddrStreet,
            //    x.AddrHouse,
            //    x.AddrHousing,
            //    x.YearComissioning,
            //    x.CeoName,
            //    YearLastOverhaul = st1Data.ContainsKey(x.st2Id) && strElData.ContainsKey(st1Data[x.st2Id]) && strElData[st1Data[x.st2Id]] < startYear
            //                ? (strElData[st1Data[x.st2Id]] > x.YearComissioning ? strElData[st1Data[x.st2Id]] : 0)
            //                : 0,
            //    x.YearPublication
            //})
            //    .ToList();

            //return new ListDataResult(result, totalCount);
            #endregion

            var loadParams = GetLoadParam(baseParams);

            var year = baseParams.Params.GetAs<long>("year");
            var locality = baseParams.Params.GetAs("muId", string.Empty);
            var address = baseParams.Params.GetAs("roId", string.Empty).ToUpper();

            var data =
                LoadProgramDomain.GetAll()
                                    .WhereIf(year > 0, x => x.PlanOverhaulYear == year)
                                    .WhereIf(!string.IsNullOrEmpty(locality), x => x.Locality == locality)
                                    .WhereIf(!string.IsNullOrEmpty(address), x => x.Address.ToUpper().Contains(address))
                                    .Select(
                                        x =>
                                        new
                                        {
                                            Id = x.IndexNumber,
                                            MunicipalityName = x.Locality,
                                            AddrPlace = x.Address,
                                            AddrStreet = x.Street,
                                            AddrHouse = x.House,
                                            AddrHousing = x.Housing,
                                            YearComissioning = x.CommissioningYear,
                                            CeoName = x.CommonEstateobject,
                                            YearLastOverhaul = x.LastOverhaulYear,
                                            YearPublication = x.PlanOverhaulYear
                                        })
                                    .OrderBy(x => x.Id);

            var totalCount = data.Count();

            //поулчаем отрезанные записи по Пейджингу
            var list = data.Paging(loadParams).ToList();

            return new ListDataResult(list, totalCount);
        }

        private LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
        }
    }
}